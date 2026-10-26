# Developer Worker briefs

The Developer Worker runs on a cheaper model and knows nothing about the task except its brief. It does exactly what the brief says and stops at the first ambiguity. The brief must therefore carry every fact the worker needs. Fill in every `<…>` placeholder; a brief with a placeholder left in is not ready to send.

## Rules for the Developer

- **One objective per brief, one brief per worker call.**
- **Concurrency.**
  - Run briefs that edit, build or test one at a time.
  - Only the read-only briefs (W1 and W7) may run in parallel, with each other.
- **Exact names only.** Give types, namespaces, test method names, generated property names and `Fields` constants. Never ask the worker to find them in a generated model file.
- **Explicit case lists.** List the test cases one by one. "Cover the handler" is not a specification.
- **Check every result.** After each worker, follow "Delegation" in the skill:
  - check `git --no-optional-locks status --short` against the allow-list;
  - read the changed production files;
  - check the tests against the case list.

## Template

Send the brief as the whole prompt, with the sections in this order:

```text
OBJECTIVE: <one sentence>
READ FIRST: <files to read before starting, including the instructions file for the area>
CONTEXT: <facts the worker needs and must not infer: names, approved decisions, generated property names>
SPECIFICATION:
1. <exact item>
ALLOWED FILES:
- Create: <repository-relative paths, or none>
- Modify: <paths, or none>
- Delete: <paths, or none>
MUST NOT: <task-specific prohibitions; the worker's standing rules apply as well>
ACCEPTANCE CHECK: <exact command and expected result>
REPORT: the standard Developer Worker report.
```

**Test acceptance.** Most briefs use this check, written in full:

`pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1 -PathFilter '<test files touched, comma-separated>' -TestFilter 'FullyQualifiedName~<namespace or class>'`

- It builds once, runs the filtered tests against that build and lists the warnings in the test files touched. It must print:
  - `WARNINGS  … | added 0 | removed 0`;
  - `IN SCOPE  0 warning(s) in files matching: <the test files>`;
  - a `TESTS` line with the expected number passed (give that number in the brief) and no failures;
  - `RESULT    PASS (tests filtered)`.
- Give the paths with forward slashes, for example `tests/Defra.Imports.UnitTests/Dataverse/Queries/WatchFlagQueriesTests.cs`. The test project picks up new files without a project entry.
- Any other `RESULT` line, an added warning or an `IN SCOPE` warning means the brief is not met. Fix the warning; do not suppress it.

## W1: usage sweep (read-only)

- OBJECTIVE: list every reference to `<TypeA>`, `<TypeB>` in the source, tests and project files.
- CONTEXT: the files planned for deletion: `<paths>`.
- SPECIFICATION:
  1. From the repository root, run `Get-ChildItem -Path src, tests -Recurse -File -Include *.cs, *.projitems, *.csproj | Where-Object FullName -notmatch '\\(bin|obj)\\' | Select-String -Pattern '\b(<TypeA>|<TypeB>)\b'`.
  2. Report every match as `path:line: text`, grouped by file. Mark each file INTERNAL if it is in the deletion list and EXTERNAL otherwise.
- ALLOWED FILES: none.
- MUST NOT:
  - use `git grep`, which skips untracked files;
  - drop the `\b` word boundaries, because a bare `WatchFlag` also matches `AssociateWatchFlags*`.
- ACCEPTANCE CHECK: the command ran without error, and every match is in the report.

## W2: extension tests

- OBJECTIVE: write the unit tests for `<Entity>Extensions.<Method>`.
- READ FIRST:
  - `.github/instructions/unit-tests.instructions.md`;
  - `src/common/Defra.Imports.Dataverse/Extensions/<Entity>Extensions.cs` and `src/common/Defra.Imports.Dataverse/Projections/<Context>.cs`;
  - the style example, `tests/Defra.Imports.UnitTests/Dataverse/Extensions/ItahcExtensionsTests.cs`.
- CONTEXT: each mapping as `<entity property> (<Entity>.Fields.<constant>) -> <context property>`.
- SPECIFICATION: the class `<Entity>ExtensionsTests` in `tests/Defra.Imports.UnitTests/Dataverse/Extensions/<Entity>ExtensionsTests.cs`, namespace `Defra.Imports.UnitTests.Dataverse.Extensions`, with:
  1. `<Method>_<Scenario>_<Result>` for each mapping, or one test asserting all mappings;
  2. unsupplied attributes project as null, from an entity holding only `<attributes>`;
  3. the source entity is not modified.
- ALLOWED FILES: create that test file.
- MUST NOT: change production code or `Support`.
- ACCEPTANCE CHECK: test acceptance with the class name; `<n>` tests passed.

## W3: query tests

- OBJECTIVE: write the unit tests for `<Area>Queries`.
- READ FIRST:
  - the unit-test instructions;
  - `src/common/Defra.Imports.Dataverse/Queries/<Area>Queries.cs`;
  - `tests/Defra.Imports.UnitTests/Support/WatchListServiceMockExtensions.cs` and `QueryExpressionExtensions.cs`;
  - the style example, `tests/Defra.Imports.UnitTests/Dataverse/Queries/WatchFlagQueriesTests.cs`.
- CONTEXT:
  - for each method, the table, the conditions (attribute as `<Entity>.Fields.<constant>`, operator and value), the columns and the mapping;
  - the early-bound types to return from `ReturnsRecords`.
- SPECIFICATION: the class `<Area>QueriesTests` in `tests/Defra.Imports.UnitTests/Dataverse/Queries/`. For each method:
  1. the conditions (`GetAllConditions()`) and `UsesOnlyAndFilters()`;
  2. the columns retrieved;
  3. the `NoTracking` merge option;
  4. the mapping of a returned record;
  5. an empty result;
  6. each switch branch, listed by name.
- ALLOWED FILES: create that test file.
- MUST NOT: change production code; return `new Entity(<name>)` from a mock.
- ACCEPTANCE CHECK: test acceptance with the class name; `<n>` tests passed.

## W4: handler, composition-root and constants tests

- OBJECTIVE: write the handler tests for `<Handler>`, and add the composition-root and constants tests.
- READ FIRST:
  - the unit-test instructions and the handler;
  - the style examples `tests/Defra.Imports.UnitTests/Plugins/Itahc/FlagRecordOnWatchListHandlerTests.cs`, `Plugins/PluginCompositionRootTests.cs` and `Infrastructure/Plugins/PluginConstantsTests.cs`.
- CONTEXT:
  - the handler's constructor parameters;
  - the registered step (message, entity, stage, mode and images);
  - the application call and the values it should receive;
  - whether a preserved catch-and-continue exists;
  - any new constants and their values.
- SPECIFICATION:
  1. One null-check test per constructor parameter.
  2. The single application call, with values.
  3. Message matching that ignores case.
  4. No catch, or the preserved catch-and-continue traced through `RecordingTracingService`.
  5. One test per contract failure (message, entity, stage, mode, `Target`, image) throwing `InvalidOperationException` without calling the service.
  6. A resolution test for each new handler and service in `PluginCompositionRootTests`.
  7. A pin for each new constant in `PluginConstantsTests`.
- ALLOWED FILES:
  - Create: `tests/Defra.Imports.UnitTests/Plugins/<Area>/<Handler>Tests.cs`.
  - Modify: `tests/Defra.Imports.UnitTests/Plugins/PluginCompositionRootTests.cs` and `tests/Defra.Imports.UnitTests/Infrastructure/Plugins/PluginConstantsTests.cs`.
- MUST NOT: change production code; remove or rewrite existing tests in the modified files.
- ACCEPTANCE CHECK: test acceptance with `FullyQualifiedName~Defra.Imports.UnitTests.Plugins|FullyQualifiedName~Defra.Imports.UnitTests.Infrastructure.Plugins`, with no failures.

## W5: port legacy test cases

- OBJECTIVE: re-express each listed legacy test case against the new layers.
- READ FIRST: the unit-test instructions, each legacy test file in the mapping and each new test file it maps to.
- CONTEXT: the characterisation mapping, as rows of `<legacy class.method> -> <new class.method>: <behaviour asserted>`.
- SPECIFICATION:
  1. For each row, write the new test so that it asserts the same observable behaviour: the writes, values, exceptions and traces.
  2. Name it as given.
- ALLOWED FILES: modify or create only the new test files named in the mapping.
- MUST NOT: delete or change the legacy tests; W6 removes them.
- ACCEPTANCE CHECK: test acceptance for each new test class; `<n>` tests passed.

## W6: delete the legacy code

- OBJECTIVE: delete the legacy classes and tests that are no longer used.
- CONTEXT:
  - the files to delete;
  - the `.projitems` file and the exact entry for each, in the form `<Compile Include="$(MSBuildThisFileDirectory)<relative path with backslashes>" />`.
- SPECIFICATION:
  1. Delete each file with `Remove-Item -LiteralPath <absolute path>`.
  2. For each entry, view the `.projitems` file, then remove that one line with `edit`.
  3. Delete each listed legacy test file. The test project needs no edit, because it globs its files.
- ALLOWED FILES:
  - Delete: `<paths>`.
  - Modify: `<.projitems path>`.
- MUST NOT: use `git rm`; delete anything not listed, even if it looks unused.
- ACCEPTANCE CHECK:
  - `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1` prints `RESULT    PASS`. Removed warnings are expected; added warnings are not.
  - `git --no-optional-locks status --short` shows only the listed deletions and the `.projitems` change.

## W7: stale-reference sweep (read-only)

- OBJECTIVE: find any remaining reference to the deleted, renamed or moved types `<TypeA>`, `<TypeB>`.
- SPECIFICATION:
  1. From the repository root, run `Get-ChildItem -Path src, tests, .github, architecture, docs, pipelines -Recurse -File -Include *.cs, *.projitems, *.csproj, *.md, *.yml, *.yaml, *.json, *.xml | Where-Object FullName -notmatch '\\(bin|obj|node_modules)\\' | Select-String -Pattern '\b(<TypeA>|<TypeB>)\b'`.
  2. Report every match as `path:line: text`, marking each as EXPECTED (an ADR adoption record or other history) or STALE.
- ALLOWED FILES: none.
- MUST NOT: edit anything; use `git grep`.
- ACCEPTANCE CHECK: the command ran without error, and every match is classified.

## W8: mechanical analyser fixes

- OBJECTIVE: fix the listed analyser warnings without changing behaviour.
- CONTEXT: each warning as `file|code|message`, from the gate's `IN SCOPE` output.
- SPECIFICATION:
  1. Fix each listed warning, and only those, for example:
     - `using` order (SA1210);
     - blank lines and spacing;
     - missing or malformed XML documentation, in British English except StyleCop-mandated wording.
  2. If a fix would change logic, signatures or names, stop and report BLOCKED.
- ALLOWED FILES: modify only the files in the list.
- MUST NOT: add `#pragma`, `[SuppressMessage]`, `.editorconfig` or `NoWarn` changes.
- ACCEPTANCE CHECK: `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1 -SkipTests -PathFilter '<files>'` prints `IN SCOPE  0 warning(s)`, whether the files are source or test files, and `added 0` on the `WARNINGS` line.
