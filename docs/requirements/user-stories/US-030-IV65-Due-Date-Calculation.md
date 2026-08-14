# US-030: IV65 Response Due Date Calculation

## Summary

As an EU Imports Caseworker,  
I want Dynamics 365 to calculate the date a response to an IV65 is due,  
So that I can follow up on overdue responses without manually calculating dates.

## Description

When a caseworker sets the IV65 Sent Date on an Import Record, PIMS automatically calculates the IV65 Response Due Date as IV65 Sent Date + 14 calendar days. The calculated date remains editable by caseworkers to support exceptional circumstances.

## Acceptance Criteria

- [x] **AC-1 (Auto-calculate IV65 Response Due Date):**  
  When a user changes the IV65 Sent Date field on an Import Record, PIMS calculates the IV65 Response Due Date = IV65 Sent Date + 14 calendar days.

- [x] **AC-2 (Response Due Date remains editable):**  
  The IV65 Response Due Date field remains editable by EU Imports Caseworkers after it has been auto-calculated.

## Business Rules

- [BR-024](../business-rules.md#br-024) — IV65 Response Due Date = IV65 Sent Date + 14 calendar days

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record — IV65 fields reside on the Import Record)

## Traceability

### Source Jira Issues

- IMTA-6166

### Original Links

- IMTA-6166
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/ImportApplication-IV65CalculateDueDate-3E5B8300-01BB-44AF-9E18-373C5BD9FC04.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                        |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/ImportApplication-IV65CalculateDueDate-3E5B8300-01BB-44AF-9E18-373C5BD9FC04.xaml auto-calculates: IV65 Response Due Date = IV65 Sent Date + 14 calendar days |
| AC-2                 | ✅ Implemented | IV65 Response Due Date field remains editable after auto-calculation                                                                                                                                            |
