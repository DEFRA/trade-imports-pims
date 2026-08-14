# US-022: Defer Post Import Check Counter

## Summary

As an EU Imports Caseworker,  
I want to defer the Post Import Check for the 10th Import Record when I manually schedule one earlier than planned by PIMS,  
So that I can manage available Post Import Check resource effectively.

## Description

When a caseworker manually sets Post Import Check Required? = Yes on an Import Record where PIMS had set it to No (for a Gold Place of Origin where the counter has not yet reached 10), PIMS resets the counter to 0. This ensures the 10th inspection obligation is "used up" by the manual early inspection.

## Acceptance Criteria

- [x] **AC-1 (Reset counter when manual early inspection scheduled):**  
  When a user sets Post Import Check Required? = Yes on an Import Record where:
  - The previous value was No, AND
  - The linked Place of Origin has Trust Level = Gold, AND
  - The Number of Import Records Since Last Post Import Check is between 1 and 9 (inclusive),
  
  PIMS resets the Number of Import Records Since Last Post Import Check on the Place of Origin to 0.

## Business Rules

- [BR-021](../business-rules.md#br-021) — Manual early Post Import Check resets Gold counter

## Dependencies

- [US-017](US-017-Manage-Place-of-Origin.md), [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md) (Place of Origin and Trust Level)
- [US-024](US-024-Manual-Post-Import-Check-Override.md) (Manual Post Import Check override)

## Traceability

### Source Jira Issues

- IMTA-6680

### Original Links

- IMTA-6680
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/ImportApplication-ManualPostImportCheckDecision-7436FAAE-9A22-4821-9A1B-2AA5A22BE272.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                                                                               |
| -------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/ImportApplication-ManualPostImportCheckDecision-7436FAAE-9A22-4821-9A1B-2AA5A22BE272.xaml detects: Post Import Checks Required? changed from No to Yes + Gold Place of Origin + counter 1-9 → reset counter to 0 on Place of Origin |
