---
name: plugin-di-migration
description: 'Migrate one or more legacy Dataverse plug-in classes in src/pluginassemblies/Defra.Imports.Plugins (classes deriving from Defra.Imports.BusinessLogic.Plugin) to the ADR 0001 handler-first dependency-injection pattern: PluginBase<THandler>, a handler, an application service, narrow input contexts, early-bound LINQ queries and composition-root registrations, with behaviour and step registrations frozen. Use when asked to migrate, refactor or modernise a legacy plug-in to ADR 0001, PluginBase or the DI pattern, or to port its business logic and tests. Covers intake, the design-approval gate, the warning baseline, characterisation, implementation order, tests, legacy deletion, the ADR adoption record and the final report, with ready-made Developer Worker briefs. Plug-ins only; not custom workflow activities.'
---

# Plug-in DI migration

Refactors legacy plug-in classes in `src/pluginassemblies/Defra.Imports.Plugins` (classes deriving from `Defra.Imports.BusinessLogic.Plugin`) to the handler-first dependency-injection pattern in [ADR 0001](../../../architecture/adr/0001-plugin-workflow-di-scaffold.md). Neither their behaviour nor their step registrations change.

Plug-ins only. Custom workflow activities use `WorkflowActivityBase<THandler>` and are out of scope; raise them with the user.

## Inputs

Ask for both before reading any code, with the ask-questions tool if it is available:

- **Plug-in class names.** Group plug-ins that share business logic into one run, as `FlagRecordOnWatchList` and `FlagRecordOnWatchListNotification` were. Migrate unrelated plug-ins in separate runs. Never migrate a legacy class that is not in scope.
- **The Azure Boards work item** (`AB#12345`) for the pull request. Ask now, not at the end. If there is none, carry on and list it as an open question.

## Ground rules

1. **Design first, then stop.** Read, post a design proposal (step 3) and wait for the user's explicit approval.
   - An automated "keep going" or "task not complete" message is not approval.
   - Before approval, make no edits and create no files other than session scratch files.
   - Ask every unresolved design question. Never pick a default and carry on.
2. **Behaviour is frozen.** Read [behaviour-freeze.md](./references/behaviour-freeze.md) before designing. The only pre-approved change is the handler's step-contract validation.
3. **Do not work around a gap in the generated model.** If a table or column is missing, stop and ask the user to regenerate the model.
4. **Leave git to the user.**
   - Do not stage, unstage, commit, reset, stash or check out. Delete files with file operations, not `git rm`.
   - Leave changes you did not make untouched, and list them in the report.
   - Commit only if asked. Use a `refactor: …` title, `+solutionVer(defra_Imports): patch` in the body and the Copilot `Co-authored-by` trailer.
5. **No analyser suppressions.** Migrated and new code adds no warnings.
6. **British English** in documentation and comments, except wording that StyleCop mandates, such as "Initializes a new instance of…".
7. **Code conventions** are in [csharp-source.instructions.md](../../instructions/csharp-source.instructions.md) and [unit-tests.instructions.md](../../instructions/unit-tests.instructions.md). They apply to every file you write, and this skill does not repeat them.
8. **Tool and shell pitfalls** that have damaged files before are in [gotchas.md](./references/gotchas.md). Read it before the first edit.

## Delegation

`[D]` steps are the Developer's own. `[W]` steps may go to the Developer Worker with the matching brief in [worker-briefs.md](./references/worker-briefs.md). Without a worker, do them yourself.

- Run workers that edit files, build or test **one at a time**, because they share `bin`, `obj` and the build lock. Only read-only workers run in parallel.
- After each worker:
  - compare `git --no-optional-locks status --short` with the brief's allow-list;
  - read every changed production file in full;
  - check the test changes against the case list.
- If a worker fails, re-brief it once with the missing information. If it fails again, do the step yourself.

## Procedure

At the start, record these steps as todos. Once the design is approved, record it in the session notes. Both must survive context compaction. After a compaction, re-read this file and the notes before continuing.

### 1. Intake `[D]`

- Collect the inputs.
- Run `git --no-optional-locks status --short` and record the changes already present. They are not yours.

### 2. Read `[D]`, sweep `[W]`

- **Registrations.** The source of truth is the XML in `src/solutions/defra_Imports/src/SdkMessageProcessingSteps`; `[CrmPluginRegistration]` attributes may have drifted.
  `pwsh -NoProfile -File .github/skills/plugin-di-migration/scripts/Get-PluginStepRegistration.ps1 -PluginTypeName '<PluginA>,<PluginB>'`
  A plug-in reported as `NOT REGISTERED` needs a decision from the user before you design anything.
- **Legacy code:**
  - the plug-in class;
  - every business-logic class it reaches in `src/common/Defra.Imports.BusinessLogic` or `Defra.Imports.Repositories`;
  - their tests in `tests/Defra.Imports.UnitTests`.
- **ADR 0001, targeted.**
  - List the headings with `Select-String -Path architecture/adr/0001-plugin-workflow-di-scaffold.md -Pattern '^#{2,3} '`, then read by line range.
  - Read from the start to the end of "Execution identity", then "Testing the pattern", "Adoption Guidance", every "Adoption Record" section and "Open Questions / Assumptions".
  - Skip "Illustrative example"; the reference implementations supersede it.
- **Reference implementations:** the files listed in [reference-implementations.md](./references/reference-implementations.md) for each layer you will create.
- **Test support** in `tests/Defra.Imports.UnitTests/Support`: `PluginTestBase`, `PluginRuntime`, `RecordingTracingService`, `WatchListServiceMockExtensions` and `QueryExpressionExtensions`.
- **Generated model.**
  - Never open whole generated files; `defraimp_importernotification.cs` is about 200 KB.
  - Look up property names, `Fields` casing and CLR types instead:
    `pwsh -NoProfile -Command "& ./.github/skills/scenario-builder-implementation/scripts/Get-DataverseEntityPropertyClrType.ps1 -EntityLogicalName <entity> -AttributeName <attribute1>,<attribute2>"`
- `[W]` **Usage sweep** (brief W1, read-only): find every reference to each legacy type you expect to delete, so that the design lists everything that must move or go.

### 3. Design proposal and approval gate `[D]`

- Write the proposal with [design-proposal-template.md](./references/design-proposal-template.md). It covers:
  - layers and files;
  - preserved behaviour and suspected bugs;
  - behaviour changes needing approval;
  - questions, including the `AB#` if it is still missing.
- Post it and stop. Continue only after explicit approval, and record the approved decisions in the session notes.

### 4. Baseline `[D]`

- Before the first edit, run `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/New-WarningBaseline.ps1`. All three builds (both heads and the unit test project) must succeed.
- If it reports `RESULT FAIL`, stop and report the red build. Stale `.projitems` entries are a common cause.

### 5. Characterise `[D]`

- Run the legacy tests, which must pass before you change anything:
  `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1 -SkipBuild -TestFilter 'FullyQualifiedName~<legacy test class or namespace>'`
- Map every legacy test case to its new test class and method. Every case must be ported before its legacy test is deleted.
- Keep the mapping in the session notes; brief W5 uses it.

### 6. Implement `[D]`

Work in this order, following the instructions and the reference implementations:

1. Queries, in `Defra.Imports.Dataverse.Queries`.
2. Projections and extensions, in `Defra.Imports.Dataverse.Projections` and `.Extensions`.
3. The application service, in `Defra.Imports.Application.<Area>`.
4. The handler, beside the plug-in.
5. The plug-in class.
   - Make it `public sealed class X : PluginBase<XHandler>`, with a public parameterless constructor calling `base(PluginCompositionRoot.Root)`.
   - Remove the `[CrmPluginRegistration]` attributes.
   - Describe the registration in the XML summary, as the reference plug-ins do.
6. The registrations, in `PluginCompositionRoot.BuildRoot()`.

Then:

- Add a `TODO:` comment for each preserved suspected bug, worded as in [behaviour-freeze.md](./references/behaviour-freeze.md).
- Check the build and the new files: run `Test-Change.ps1 -SkipTests -PathFilter '<new folders and files>'`. `IN SCOPE` must show no warnings in the files you created or moved.

### 7. Tests `[D]` then `[W]`

Cover every item in [test-checklist.md](./references/test-checklist.md).

- `[D]` Write the plug-in end-to-end tests and the application service tests yourself, because they encode the business behaviour.
- `[W]` Delegate the rest one brief at a time, each with an explicit case list:
  - extension tests (W2);
  - query tests (W3);
  - handler and composition-root tests (W4);
  - ported legacy cases (W5).
- Check each area with `Test-Change.ps1 -PathFilter '<test files touched>' -TestFilter '<filter>'`. It builds once, runs the filtered tests and lists the warnings in those test files under `IN SCOPE`, which must show none. Use `-SkipBuild` only for a quick run that checks no warnings.

### 8. Delete the legacy code `[W]`

- Re-run the usage sweep (W1).
- Once nothing outside the legacy code uses a class, delete it, its `.projitems` entry and its tests (W6). Delete a legacy test only after all its cases have been ported.
- Then run the stale-reference sweep (W7) for every type you deleted, renamed or moved.

### 9. ADR adoption record `[D]`

- Add an adoption record after the last "Adoption Record" section of ADR 0001, using [adoption-record-template.md](./references/adoption-record-template.md).
- Change convention text only where the approved design changed a convention. That text is "Layer ownership" and the `Defra.Imports.Application` and `Defra.Imports.Dataverse` rows of the project table in [Decision](../../../architecture/adr/0001-plugin-workflow-di-scaffold.md#decision).
- If the migration introduced a shape that the reference implementations do not show, add it to [reference-implementations.md](./references/reference-implementations.md).
- Create no other documents.

### 10. Verify `[D]`

- **Gate.** Run `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1 -PathFilter '<new and changed files>'`, with no skip switches and no `-TestFilter`, listing the source and test files you created or changed. It must print `RESULT PASS`, and `IN SCOPE` must show no warnings. Warnings removed from deleted legacy files are expected; none may be added. Fix every warning in those files; use W8 for mechanical fixes.
- **Stale references.** Confirm that the stale-reference sweep (W7) is clean.
- **Self-review.**
  - Review `git --no-optional-locks diff` for every changed production file, and new files in full, against [behaviour-freeze.md](./references/behaviour-freeze.md).
  - Review the tests against [test-checklist.md](./references/test-checklist.md).
  - Check for stray runs of spaces and missing blank lines.

### 11. Report `[D]`

Write the final report with [final-report-template.md](./references/final-report-template.md).
