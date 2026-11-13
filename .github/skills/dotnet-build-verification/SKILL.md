---
name: dotnet-build-verification
description: 'Build both Dataverse plug-in assembly heads (Defra.Imports.Plugins and Defra.Imports.Workflows) in Release and the Defra.Imports.UnitTests project, record a normalised warning baseline, diff warnings after a change (including new and changed test files) and run the unit tests, with a compact PASS/FAIL report. Use before changing any C# under src or tests (to take the baseline), while iterating on build errors, analyser warnings or failing unit tests, and as the verification gate before reporting a C# change as complete.'
---

# .NET build verification

## Why this exists

The two plug-in assembly heads compile the shared projects (`Defra.Imports.Model`, `Defra.Imports.BusinessLogic` and `Defra.Imports.Repositories`), and `Defra.Imports.UnitTests` adds its own legacy warnings. Together they carry thousands of existing analyser warnings, so "no new warnings" cannot be checked by eye. These scripts:

- build both heads with the agreed flags (`--configuration Release -v q -nologo --no-incremental -clp:ForceNoAlign`), then `tests/Defra.Imports.UnitTests` with the same flags and `-p:Configuration=UnitTest`;
- normalise each warning to `file|code|message`, with repository-relative paths and no line numbers, so moving code does not create noise;
- record only the test-project warnings that MSBuild attributes to the test project. In the `UnitTest` configuration its build repeats the shared-source and head warnings under `Defra.Imports.Container` and the heads, which the head builds already cover;
- diff the warnings against a baseline as a multiset, so a second occurrence of an existing warning still counts as added;
- run the unit tests with `dotnet test --no-build` against that build, holding one lock across the builds and the test run;
- print a short report, and keep the full logs in a folder outside the repository.

## Scripts

Run them with PowerShell 7 from anywhere in the repository, for example `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1 -SkipTests`.

| Script | Purpose | Duration |
| --- | --- | --- |
| [New-WarningBaseline.ps1](./scripts/New-WarningBaseline.ps1) | Builds both heads and the unit test project, and saves the warning baseline. Saves nothing if any build is red. | About 15 seconds |
| [Test-Change.ps1](./scripts/Test-Change.ps1) | Builds both heads and the unit test project, diffs the warnings, runs the unit tests and prints `RESULT PASS`, `FAIL` or `INCOMPLETE`. | About 15 seconds |
| [BuildVerification.psm1](./scripts/BuildVerification.psm1) | Shared functions used by both scripts. Do not call it directly. | Not applicable |

`Test-Change.ps1` options:

- `-SkipTests` or `-SkipBuild` for a quicker partial check. With `-SkipBuild`, `dotnet test` compiles the test project itself, and its warnings are not diffed.
- `-TestFilter '<dotnet test filter>'`, for example `'FullyQualifiedName~Defra.Imports.UnitTests.Plugins.ParameterSetKeyDataPair'`. The `RESULT` line then says `tests filtered`.
- `-PathFilter '<pattern>,<pattern>'` to list every current warning in the files you touched, in the source or the tests. It needs the build: with `-SkipBuild` it is not checked and the result is `INCOMPLETE`.
- `-BaselineName` and `-OutputDirectory` to choose a different baseline.

## Procedure

1. **Baseline before any C# edit.** Run `New-WarningBaseline.ps1`. Take a fresh baseline at the start of every task; never reuse one from an earlier task or session. If it reports `RESULT FAIL`, stop and report the red build before changing anything. Stale `.projitems` entries are a common cause; so is a compile error in the test project.
2. **Iterate with the cheapest run that answers the question.**
   - Build and warnings only: `Test-Change.ps1 -SkipTests -PathFilter '<paths you changed>'`. Include test files in the paths.
   - Tests for one area, without a build: `Test-Change.ps1 -SkipBuild -TestFilter '<filter>'`. `dotnet test` compiles the test project, and its warnings are not checked.
   - Tests for one area, with the warnings in the test files you touched: `Test-Change.ps1 -PathFilter 'tests/Defra.Imports.UnitTests/<folder>/*' -TestFilter '<filter>'`. It builds once, runs only the filtered tests and prints `IN SCOPE`.
3. **Gate.** Before reporting the change as complete, run `Test-Change.ps1` with no skip switches and no `-TestFilter`. Only a bare `RESULT PASS` from this full run counts as verified. Add `-PathFilter '<new and changed files>'` and check that `IN SCOPE` shows no warnings in them.

## Reading the report

- `BUILD`: the result for each of the three projects: both heads, then `Defra.Imports.UnitTests`. When a build fails, the first 20 distinct errors from every project in that build follow, and the tests are not run. A compile error in the test project fails the build phase.
- `WARNINGS baseline N -> current M | added A | removed R`:
  - Up to 30 added warnings follow, with occurrence counts. Any added warning fails the run.
  - Removed warnings are grouped by file. They are expected when legacy files are deleted.
  - If the baseline records a different set of projects, or none, the diff is skipped and the result is `INCOMPLETE`. Take a new baseline.
- `IN SCOPE`: every current warning, new or pre-existing, in files matching `-PathFilter`. Use it to confirm that new files are warning-free. With `-SkipBuild` it prints `not checked`.
- `TESTS total | passed | failed | skipped`: up to 30 failed tests follow, each with the first line of its message. `skipped` counts xUnit `Skip` tests, which are expected. `TESTS no results` now occurs only on `-SkipBuild` runs, where `dotnet test` compiles the test project itself; its compile errors are listed.
- `RESULT` and `DETAILS <run folder>`. The run folder holds the full build and test logs, the TRX file, `warnings-added.txt` and `warnings-removed.txt`. Read those files only when the summary is not enough. Only a bare `RESULT PASS` means that the gate passed. A note such as `(build skipped; tests filtered)` marks a partial run.

| Exit code | Result | Meaning |
| --- | --- | --- |
| 0 | `PASS` | The builds are green, no warnings were added and no tests failed. `(build skipped)`, `(tests skipped)` or `(tests filtered)` marks a partial check, not the gate. |
| 1 | `FAIL` | A build error in a head or the test project, added warnings, failed tests, or a test run that produced no results (only possible with `-SkipBuild`; the errors are listed). |
| 2 | `INCOMPLETE` | No baseline with that name, a baseline taken with a different set of projects, no tests ran or matched `-TestFilter`, `-PathFilter` with `-SkipBuild`, or both skip switches were used. |

## Path filters

- Use repository-relative paths with forward slashes and PowerShell `-like` wildcards, separated by commas inside one quoted string: `-PathFilter 'src/common/Defra.Imports.BusinessLogic/Parameter Set Key Data Pair/*,src/pluginassemblies/Defra.Imports.Plugins/ParameterSetKeyDataPair/*,tests/Defra.Imports.UnitTests/Plugins/ParameterSetKeyDataPair/*'`.
- Prefer exact folders or file names. A broad fragment such as `*WatchFlag*` also matches unrelated files such as `AssociateWatchFlags*`.
- Warnings in test files appear once, because only the test project reports them. Files in the shared projects compile into both heads, so each of their warnings still appears twice.

## Rules

- **One build at a time per working tree.** Concurrent builds collide on the `obj` and `bin` folders. The scripts queue behind each other (`WAITING` is printed), but a build started any other way, for example by an IDE or a direct `dotnet build`, is not queued. Never run code-changing workers that build or test in parallel in the same working tree.
- **Same machine and SDK** for the baseline and the gate. The baseline records the commit, the SDK version and the projects built in `<name>.json` next to the baseline file.
- **Fix warnings; do not suppress them.** Do not add `#pragma`, `[SuppressMessage]`, `.editorconfig` or `NoWarn` changes to reach `PASS` unless the user has approved that specific suppression.
- **Trust the script output, not a summary of it.** A Developer Worker that runs these scripts quotes the `RESULT` line and any failure lines verbatim. The Developer runs the final gate itself, because its output is only a few lines.
- Output goes to a per-checkout folder under the system temporary directory, which keeps the ten most recent runs. `-OutputDirectory` must be outside the repository.
