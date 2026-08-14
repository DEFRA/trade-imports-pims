# US-009: Auto-Create Import Record from ITAHC

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically create an associated Import Record when an ITAHC is received from TRACES Classic,  
So that I can save the effort of manually creating an Import Record and duplicating key ITAHC data.

## Description

When an ITAHC is auto-created in PIMS by the TRACES Classic integration ([US-007](US-007-Receive-ITAHC-From-TRACES.md)), PIMS should automatically create a linked Import Record with key ITAHC fields mapped across. The Import Record is only auto-created when the ITAHC was created by the integration service account, not when manually created by a caseworker.

**Note:** The specific field mapping from ITAHC to Import Record is not specified in this baseline. Attribute-level traceability remains incomplete until the agreed schema is available.

## Acceptance Criteria

- [x] **AC-1:** When an ITAHC is created in PIMS by the TRACES integration service account, PIMS automatically creates a linked Import Record with key fields copied from the ITAHC. The specific field list is not specified in this baseline.

- [x] **AC-2:** The auto-created Import Record has the ITAHC set as the Primary ITAHC.

- [x] **AC-3:** The auto-creation does not trigger when an ITAHC is manually created by a business user.

## Business Rules

- [BR-008](../business-rules.md#br-008) — P3 random inspection counter incremented on Import Record creation
- [BR-022](../business-rules.md#br-022) — Unique reference number generated on Import Record creation

## Dependencies

- [US-007](US-007-Receive-ITAHC-From-TRACES.md) (ITAHC received from TRACES is prerequisite)
- [US-027](US-027-Auto-Assign-ITAHC-DOCOM-to-Region.md) (Auto-assigned Import Record to regional team)

## Traceability

### Source Jira Issues

- IMTA-6600

### Original Links

- IMTA-6600
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/ITAHC-CreateImportRecordFromITAHC-35DDCA57-47E9-459D-9900-7505B63F87CD.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: Field mapping schema is not specified in this baseline.

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                             |
| -------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| AC-1                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/ITAHC-CreateImportRecordFromITAHC-35DDCA57-47E9-459D-9900-7505B63F87CD.xaml creates linked Import Record with ITAHC fields copied |
| AC-2                 | ✅ Implemented | Auto-created Import Record has Primary ITAHC set                                                                                                                                     |
| AC-3                 | ✅ Implemented | Auto-creation conditional on TRACES service account ownership                                                                                                                        |
