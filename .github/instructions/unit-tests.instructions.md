---
description: "Use when writing, reviewing or updating tests in Defra.Imports.UnitTests, including plug-in, custom workflow activity and business logic tests. Covers the xUnit and Moq conventions, the workflow activity test base class, mocking the organisation service and how to run the tests."
applyTo: "tests/Defra.Imports.UnitTests/**"
---

# Unit Test Standards

`Defra.Imports.UnitTests` targets net462 with C# 7.3 and uses xUnit, Moq and the StyleCop and Creedengo analysers. It builds in the `UnitTest` configuration, in which the plug-in and workflow heads reference `Defra.Imports.Container` instead of compiling the shared projects.

The production code conventions are in [csharp-source.instructions.md](./csharp-source.instructions.md).

## Conventions

- Mirror the source folders and namespaces:
  - `Defra.Imports.BusinessLogic.<Area>` is tested in `BusinessLogic/<Area>`, namespace `Defra.Imports.UnitTests.BusinessLogic.<Area>`;
  - `Defra.Imports.Plugins.<Table>` is tested in `Plugins/<Table>`;
  - `Defra.Imports.Workflows.<Area>` is tested in `Workflows/<Area>`.
- Make test classes `public sealed` and name them `<Subject>Tests`.
- Name test methods `Method_Scenario_ExpectedResult`, for example `Execute_OnCreateWithNumberAnimalKey_UpdatesCommodityComplement`. CA1707 is suppressed for the test namespace.
- Use `[Fact]`, or `[Theory]` with `[InlineData]` for a set of equivalent cases. Assert with xUnit's `Assert`; the project has no assertion library.
- Structure each test with `// Arrange`, `// Act` and `// Assert` comments.
- Put `using` directives inside the namespace, prefix instance members with `this.`, and document the class, constructor and test methods with XML comments, as `PopulateAnimalNumberTests` does. SA1600 is enabled, and new tests must add no warnings.
- Use constants, generated names and `nameof` rather than literals where the production code does, for example `defraimp_itahc.EntityLogicalName` and `defraimp_itahc.Fields.defraimp_name`.
- Add no analyser warnings and no suppressions.

## Mocks and test data

- Create `IOrganizationService` and `ILogWriter` mocks with Moq. Existing tests use loose mocks; use `MockBehavior.Strict` for a service when the test must prove that nothing else was called. Set up every call that you assert on, and assert writes with `Verify` and `Times`.
- Return early-bound entities from mocks (`new defraimp_WatchList { … }`), never `new Entity(logicalName)`, because the code under test calls `ToEntity<T>()` and projects typed properties.
- Set up the service call that the code under test makes. A `QueryExpression` goes through `RetrieveMultiple`, but a query that is issued through LINQ (for example `CrmRepository`) reaches the service as `Execute(RetrieveMultipleRequest)`.
- Capture entities passed to `Create` or `Update` with `Callback` and assert on their attributes.
- Build `Create`-shaped targets with only the attributes that the scenario supplies. Every other attribute must read as null, as it does in Dataverse.
- Do not share mutable state between tests. Create the subject and its mocks in the constructor, which xUnit runs once for each test.

## What to test

- **Business logic class:** construct it with mocks. Test each rule, the writes it makes (assert the entities passed to `Create` or `Update`), the cases where it must write nothing (`Times.Never`), the values it logs, and that an input entity is not modified unless that is the method's purpose.
- **Plug-in, end to end:**
  - Mock `IServiceProvider` to return `ITracingService`, `IPluginExecutionContext` and an `IOrganizationServiceFactory` whose `CreateOrganizationService` returns the organisation service mock.
  - Set `MessageName`, `InputParameters` (`Target`) and any `PreEntityImages` or `PostEntityImages` on the context mock, then call `Execute(serviceProvider)` on the plug-in.
  - Cover each message, stage or image variation that the plug-in handles, the case where it must do nothing, and its error handling.
  - Example: `PopulateAnimalNumberTests`.
- **Custom workflow activity:**
  - Derive the test class from `WorkflowActivityTests<TCodeActivity>`. It provides `WorkflowInvoker`, `OrgSvcMock`, `TracingSvcMock`, `OrgSvcFactoryMock` and `WorkflowContextMock`.
  - Invoke the activity with `this.WorkflowInvoker.Invoke(inputs)`, where `inputs` is a dictionary keyed by the activity's argument property names, and read outputs from the returned dictionary.
  - Test the required arguments, the outputs and the service calls.
  - Example: `IncrementAutonumberTests`.
- **Utilities and extensions:** test each mapping and edge case with `[Theory]` where the cases are equivalent.

## Running the tests

Run the tests through the `dotnet-build-verification` skill. It builds both heads and this project, compares the warnings with a baseline, including this project's own, and summarises the test results:

- One area while iterating: `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1 -SkipBuild -TestFilter 'FullyQualifiedName~Defra.Imports.UnitTests.Plugins.ParameterSetKeyDataPair'`. It does not build, so it checks no warnings.
- The full gate: `pwsh -NoProfile -File .github/skills/dotnet-build-verification/scripts/Test-Change.ps1`. It fails if a warning is added in a test file.
- To list the warnings in the test files you touched, run a build with a path filter, for example `Test-Change.ps1 -PathFilter 'tests/Defra.Imports.UnitTests/Plugins/ParameterSetKeyDataPair/*' -TestFilter 'FullyQualifiedName~Defra.Imports.UnitTests.Plugins.ParameterSetKeyDataPair'`. `IN SCOPE` must show none. Use `-PathFilter 'tests/Defra.Imports.UnitTests/<folder>/*'` for any folder; a run with `-SkipBuild` ignores it.

The direct equivalent is `dotnet test tests\Defra.Imports.UnitTests\Defra.Imports.UnitTests.csproj -p:Configuration=UnitTest -nologo`.
