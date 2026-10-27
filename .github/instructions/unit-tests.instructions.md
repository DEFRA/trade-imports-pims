---
description: "Use when writing, reviewing or updating tests in Defra.Imports.UnitTests, including plug-in, handler, application service, query, extension and composition-root tests. Covers the xUnit and strict Moq conventions, the plug-in test support helpers, mocking early-bound LINQ queries and how to run the tests."
applyTo: "tests/Defra.Imports.UnitTests/**"
---

# Unit Test Standards

`Defra.Imports.UnitTests` targets net462 with C# 7.3 and uses xUnit, Moq and the StyleCop and Creedengo analysers. It builds in the `UnitTest` configuration, in which the plug-in and workflow heads reference `Defra.Imports.Container` instead of compiling the shared projects.

The production code conventions are in [csharp-source.instructions.md](./csharp-source.instructions.md). [Testing the pattern](../../architecture/adr/0001-plugin-workflow-di-scaffold.md#testing-the-pattern) in ADR 0001 explains the test support.

## Conventions

- Mirror the source folders and namespaces. For example, the tests for `Defra.Imports.Dataverse.Queries.WatchFlagQueries` are in `Dataverse/Queries/WatchFlagQueriesTests.cs`, in the `Defra.Imports.UnitTests.Dataverse.Queries` namespace.
- Make test classes `public sealed` and name them `<Subject>Tests`.
- Name test methods `Method_Scenario_ExpectedResult`, for example `Execute_WhenTheTargetIsMissing_ThrowsInvalidPluginExecutionWithoutCallingTheService`.
- Use `[Fact]`, or `[Theory]` with `[InlineData]` for a set of equivalent cases. Assert with xUnit's `Assert`; the project has no assertion library.
- Prefix instance members with `this.`. Tests need no XML documentation (SA1600 is disabled here), so choose names that describe the behaviour.
- Use constants, generated names and `nameof` rather than literals, for example `PluginMessages.Create`, `PluginStages.PostOperation`, `defraimp_itahc.EntityLogicalName` and `defraimp_itahc.Fields.defraimp_name`. A value that the test invents, such as an unsupported message name, goes in a named constant.
- Add no analyser warnings and no suppressions.

## Mocks and test data

- Create `IOrganizationService` and application-service mocks with `MockBehavior.Strict`, so that any unexpected call fails the test. A strict service mock is also the proof that an invalid invocation read and wrote nothing.
- Return early-bound entities from mocks (`new defraimp_WatchList { … }`), never `new Entity(logicalName)`. The sandbox returns typed entities, and LINQ projections fail on untyped ones.
- The early-bound LINQ provider sends `Execute(RetrieveMultipleRequest)`, not `RetrieveMultiple`. Set up query results with `ReturnsRecords(logicalName, records)` from `WatchListServiceMockExtensions`. It returns the captured `QueryExpression` objects for assertions. Count queries with `VerifyQueried` or `VerifyQueriedAny`.
- Build `Create`-shaped targets with only the attributes that the scenario supplies. Every other attribute must read as null, as it does in Dataverse.
- Put reusable test data builders in `Support`, for example `WatchFlagTestData`. Do not share mutable state between tests.

## What to test

- **Plug-in, end to end:**
  - Derive the test class from `PluginTestBase<TPlugin>`. Describe the invocation with `this.Runtime` (a `PluginRuntime`): `ForStep(message, entity, stage, mode)`, `WithTarget`, `WithoutTarget`, `WithPreImage` and `WithLogger`. Then call `this.Execute()`.
  - Assert that the plug-in is a `PluginBase<THandler>`.
  - For each invalid step contract (message, entity, stage, mode and a missing `Target`), call `this.AssertExecuteFailsWithoutCallingService()`.
  - Assert that an unexpected failure surfaces as `InvalidPluginExecutionException`, with the cause as its inner exception.
  - Example: `FlagRecordOnWatchListTests`.
- **Handler:**
  - Construct the handler with a strict mock of the application interface and `this.runtime.CreateExecution()`.
  - Test the constructor null checks, the single application call and the values passed to it, and message matching that ignores case.
  - Test that failures are not caught, or that a preserved catch-and-continue traces the failure (assert through `RecordingTracingService`).
  - Test that each contract failure throws `InvalidOperationException` without calling the service.
  - Example: `FlagRecordOnWatchListHandlerTests`.
- **Application service:** test the constructor null checks, each business rule and the explicit writes (assert the entities passed to `Create` or `Update`). Also test that the input entity or context is not modified.
- **Query:**
  - Use a real `ImportsContext` over the strict service mock.
  - Assert the `NoTracking` merge option, the conditions (with `GetAllConditions()` and `UsesOnlyAndFilters()` from `QueryExpressionExtensions`), the columns retrieved and how the results are mapped.
  - Example: `WatchFlagQueriesTests`.
- **Extension:** test every field mapping, that unsupplied attributes project as null, and that the source entity is not modified. Example: `ItahcExtensionsTests`.
- **Composition root:**
  - Create an execution scope from `PluginCompositionRoot.Root` and register the runtime services from a `PluginRuntime`, such as `IPluginExecution` and `IOrganizationService`.
  - Then resolve each new handler and service.
  - Example: `PluginCompositionRootTests`.
- **Constants:** pin each new platform constant in `PluginConstantsTests`.

## Running the tests

Run the tests through the `dotnet-build-verification` skill. It builds both heads and this project, compares the warnings with a baseline, including this project's own, and summarises the test results:

- One area while iterating: `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1 -SkipBuild -TestFilter 'FullyQualifiedName~Defra.Imports.UnitTests.Plugins.Itahc'`. It does not build, so it checks no warnings.
- The full gate: `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1`. It fails if a warning is added in a test file.
- To list the warnings in the test files you touched, run a build with a path filter, for example `Test-Change.ps1 -PathFilter 'tests/Defra.Imports.UnitTests/Plugins/Itahc/*' -TestFilter 'FullyQualifiedName~Defra.Imports.UnitTests.Plugins.Itahc'`. `IN SCOPE` must show none. Use `-PathFilter 'tests/Defra.Imports.UnitTests/<folder>/*'` for any folder; a run with `-SkipBuild` ignores it.

The direct equivalent is `dotnet test tests\Defra.Imports.UnitTests\Defra.Imports.UnitTests.csproj -p:Configuration=UnitTest -nologo`. A few tests are skipped by design, including some `DetermineInspection` cases.
