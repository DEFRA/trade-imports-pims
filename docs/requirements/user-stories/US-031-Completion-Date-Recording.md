# US-031: Completion Date Recording

## Summary

As an EU Caseworker,  
I want Dynamics 365 to record the date an Import Record is moved to Completion,  
So that Defra can measure the end-to-end processing time from receipt to post-import check scheduling.

## Description

When a caseworker marks an Import Record as complete (Moved to Completion? = Yes), PIMS automatically records the current date and time in a read-only Moved to Completion Date field. If the caseworker reverses this (sets to No), the date is cleared.

## Acceptance Criteria

- [x] **AC-1 (Journal completion date):**  
  When a user sets the Moved to Completion? field to Yes, PIMS records the current date and time in the Moved to Completion Date field. This field is read-only.

- [x] **AC-2 (Clear completion date on reversal):**  
  When a user sets the Moved to Completion? field back to No, PIMS clears the Moved to Completion Date field.

## Business Rules

- [BR-026](../business-rules.md#br-026) — Moved to Completion Date journalled automatically

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record)

## Traceability

### Source Jira Issues

- IMTA-6180

### Original Links

- IMTA-6180
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/ImportApplication-ChangeStatusReasontoCompletion-9813A24F-0C73-45F1-B0F0-C1D7F239417F.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                                      |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/ImportApplication-ChangeStatusReasontoCompletion-9813A24F-0C73-45F1-B0F0-C1D7F239417F.xaml records current date/time in read-only Moved to Completion Date when Moved to Completion? = Yes |
| AC-2                 | ✅ Implemented | Moved to Completion Date cleared when Moved to Completion? set to No                                                                                                                                                                          |
