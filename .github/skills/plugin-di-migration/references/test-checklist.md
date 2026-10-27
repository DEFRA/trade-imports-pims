# Test checklist

Every migration needs the tests below. How to write each kind (helpers, mocks and naming) is in [unit-tests.instructions.md](../../../instructions/unit-tests.instructions.md); this list says what must exist before the migration is complete. Tick it in the design proposal's test plan and again before the final gate.

## Plug-in end to end (`PluginTestBase<TPlugin>`, strict service mock)

- [ ] The plug-in is a `PluginBase<THandler>` of the expected handler type.
- [ ] A happy path for every registered step (message and stage) reported by `Get-PluginStepRegistration.ps1`, asserting the writes made through the service.
- [ ] Each contract failure (wrong message, entity, stage or mode, and a missing `Target` or image) calls `AssertExecuteFailsWithoutCallingService()`.
- [ ] An unexpected failure surfaces as `InvalidPluginExecutionException` with the cause as its inner exception. Where catch-and-continue is preserved, the failure is traced (through `RecordingTracingService`) and swallowed instead.

## Handler

- [ ] Constructor null checks, one test per parameter.
- [ ] A single application call with the projected values.
- [ ] Message matching ignores case.
- [ ] No catch, so a service failure propagates; or the preserved catch-and-continue traces the failure.
- [ ] Each contract failure throws `InvalidOperationException` without calling the application service.

## Application service

- [ ] Constructor null checks.
- [ ] Each business rule and each branch of each rule map.
- [ ] The explicit writes: the exact entities passed to `Create` or `Update`, and their order where the legacy code interleaved reads and writes.
- [ ] The input context or entity is not modified.
- [ ] Each record created from one context gets its own `EntityReference`.
- [ ] Each preserved suspected bug is pinned by a test, so a later fix is a visible behaviour change.

## Query

- [ ] A real `ImportsContext` over a strict service mock, with results supplied by `ReturnsRecords`.
- [ ] The `NoTracking` merge option.
- [ ] The conditions, through `GetAllConditions()` and `UsesOnlyAndFilters()`.
- [ ] The columns retrieved.
- [ ] How results are mapped, and an empty result.
- [ ] Every branch of a switch on a business key.

## Extension

- [ ] Every field mapping.
- [ ] Unsupplied attributes project as null, using a `Create`-shaped entity holding only the supplied attributes.
- [ ] The source entity is not modified.

## Composition root and constants

- [ ] Each new handler and service resolves from an execution scope in `PluginCompositionRootTests`.
- [ ] Each new platform constant is pinned in `PluginConstantsTests`.

## Characterisation

- [ ] Every legacy test case in the characterisation mapping has a passing counterpart before its legacy test is deleted.
- [ ] The final test count is reported, with the legacy count it replaced.
