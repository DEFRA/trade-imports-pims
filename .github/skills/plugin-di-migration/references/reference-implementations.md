# Reference implementations

Copy the shape of these files, not their business content. Read only the layers you will create. Paths are relative to the repository root.

## Production code

| Layer | Files | What it shows |
| --- | --- | --- |
| Entry point | `src/pluginassemblies/Defra.Imports.Plugins/Itahc/FlagRecordOnWatchList.cs`, `ImporterNotification/FlagRecordOnWatchListNotification.cs`, `ParameterSetKeyDataPair/PopulateAnimalNumber.cs` | `public sealed class X : PluginBase<XHandler>`; a parameterless constructor calling `base(PluginCompositionRoot.Root)`; the registration described in the XML summary; no `[CrmPluginRegistration]` |
| Handler, no catch | `src/pluginassemblies/Defra.Imports.Plugins/Itahc/FlagRecordOnWatchListHandler.cs`, `ImporterNotification/FlagRecordOnWatchListNotificationHandler.cs` | Step-contract validation; copying the `Target` with `ToEntity`; projecting it to a context; one application call; no catch and no logger, because the legacy code had neither |
| Handler, catch-and-continue | `src/pluginassemblies/Defra.Imports.Plugins/ParameterSetKeyDataPair/PopulateAnimalNumberHandler.cs` | Validation before the preserved catch; `ILogWriter`, because the legacy code logged; the `Update` pre-image merge as a host concern |
| Application service that creates | `src/common/Defra.Imports.Application/WatchLists/IWatchFlagCreator.cs`, `WatchFlagCreator.cs` | One overload per context; the rule map private in the class; an explicit `Create` per record; a new `EntityReference` copy for each record created from one context |
| Application service that updates | `src/common/Defra.Imports.Application/CommodityComplements/ICommodityComplementKeyDataApplier.cs`, `CommodityComplementKeyDataApplier.cs` | The key-to-field map private in the class; an explicit `Update`; warnings through `ILogWriter` |
| Input contexts | `src/common/Defra.Imports.Dataverse/Projections/CreateItahcWatchFlagsContext.cs`, `CreateNotificationWatchFlagsContext.cs`, `ApplyCommodityComplementKeyDataContext.cs` | Immutable `sealed` classes holding only what the service reads |
| Extensions | `src/common/Defra.Imports.Dataverse/Extensions/ItahcExtensions.cs`, `ImporterNotificationExtensions.cs`, `ParameterSetKeyDataPairExtensions.cs` | One class per typed entity; `To<Context>()` copies values and does not validate that they are present |
| Queries | `src/common/Defra.Imports.Dataverse/Queries/WatchFlagQueries.cs`, `CommodityComplementQueries.cs` | Early-bound LINQ over `ImportsContext` with `MergeOption.NoTracking`; `FindEconomicOperatorId` chooses the table with a switch on the watch type |
| Composition root | `src/pluginassemblies/Defra.Imports.Plugins/PluginCompositionRoot.cs` | `Register<I, T>()` for services; `Register<T>()` for queries and handlers |
| Plug-in constants | `src/common/Defra.Imports.Infrastructure/Plugins/PluginMessages.cs`, `PluginStages.cs`, `PluginModes.cs`, `PluginInputParameters.cs` | Shared message, stage, mode and parameter names |
| Base types (do not change) | `src/common/Defra.Imports.Infrastructure/Plugins/PluginBase.cs`, `IPluginHandler.cs`, `IPluginExecution.cs`, `PluginExecution.cs` | What every migrated plug-in inherits: scope creation, tracing and failure wrapping |
| Entry point with three steps and images | `src/pluginassemblies/Defra.Imports.Plugins/ImportApplication/DetermineInspectionRequirement.cs`, `DetermineInspectionRequirementHandler.cs` | Per-message stage, `Target` or `EntityMoniker` and image validation; pre and post images projected onto one context; `ILogWriter` because the legacy plug-in traced |
| Workflow activity and handler | `src/pluginassemblies/Defra.Imports.Workflows/ImportApplication/DetermineInspectionRequirement.cs`, `DetermineInspectionRequirementHandler.cs`, `InspectionPlaceOfOriginChange.cs`, `InspectionPlaceOfOriginChangeHandler.cs` | `WorkflowActivityBase<THandler>` over a frozen argument; the handler passes the identifier and the application service retrieves with the legacy columns |
| Stateful, interleaved use case | `src/common/Defra.Imports.Application/InspectionRequirements/` | Strategies and counter managers as injected collaborators; a working record that tracks a changed flag so that inputs are never modified; the auditor called directly in place of an event |

## Tests

All paths are under `tests/Defra.Imports.UnitTests`.

| Kind | Files |
| --- | --- |
| Plug-in end to end | `Plugins/Itahc/FlagRecordOnWatchListTests.cs`, `Plugins/ImporterNotification/FlagRecordOnWatchListNotificationTests.cs`, `Plugins/ParameterSetKeyDataPair/PopulateAnimalNumberTests.cs` |
| Handler | `Plugins/Itahc/FlagRecordOnWatchListHandlerTests.cs`, `Plugins/ImporterNotification/FlagRecordOnWatchListNotificationHandlerTests.cs`, `Plugins/ParameterSetKeyDataPair/PopulateAnimalNumberHandlerTests.cs` |
| Application service | `Application/WatchLists/WatchFlagCreatorTests.cs`, `Application/CommodityComplements/CommodityComplementKeyDataApplierTests.cs` |
| Query | `Dataverse/Queries/WatchFlagQueriesTests.cs`, `Dataverse/Queries/CommodityComplementQueriesTests.cs` |
| Extension | `Dataverse/Extensions/ItahcExtensionsTests.cs`, `ImporterNotificationExtensionsTests.cs`, `ParameterSetKeyDataPairExtensionsTests.cs` |
| Composition root and constants | `Plugins/PluginCompositionRootTests.cs`, `Infrastructure/Plugins/PluginConstantsTests.cs` |
| Stateful logic over an in-memory store | `Support/InspectionDataverse.cs`, `InspectionTestData.cs`, `InspectionServices.cs`; `Application/InspectionRequirements/P1InspectionStrategyTests.cs`, `InspectionRequirementDeterminerTests.cs` |
| Support | `Support/PluginTestBase.cs`, `PluginRuntime.cs`, `RecordingTracingService.cs`, `WatchListServiceMockExtensions.cs`, `QueryExpressionExtensions.cs` |

## Not a reference

The watch list example in ADR 0001 ("Illustrative example") and its types are illustrative only:

- `WatchListInitialisationPlugin` and `WatchListInitialisationHandler`, in `src/pluginassemblies/Defra.Imports.Plugins/WatchLists`;
- `IWatchListInitialiser` and `WatchListInitialiser`, in `Defra.Imports.Application.WatchLists`;
- `WatchListQueries` and `WatchListInitialisationContext`, in `Defra.Imports.Dataverse.Queries`.

Do not extend them, copy their private constants or treat them as precedent.

## Keeping this list current

When a migration introduces a shape that is not shown here, such as a first post-image or a first catch-and-continue around a create, add a row for it in the same change, as step 9 of the skill requires.
