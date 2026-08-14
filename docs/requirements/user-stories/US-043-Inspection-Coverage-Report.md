# US-043: Inspection Coverage Audit Report

## Summary

As a Team Leader / Data Team Member,  
I want to be able to view an Inspection Coverage Audit Report,  
So that I can verify that the 2% P3 and 10% P2 random inspection rules are being applied correctly.

## Description

A report showing daily Import Record creation volumes, risk level breakdown and actual vs expected random inspection flagging counts. This supports compliance demonstration and identification of anomalies in the inspection counter logic.

**Note:** IMTA-6658 did not include a formal user story or acceptance criteria. These requirements are inferred from the field list provided, and the current implementation evidence is partial rather than a direct one-to-one report specification match.

## Acceptance Criteria

- [x] **AC-1 (Daily Import Record volume by risk level):**  
  The report shows, for each day in the selected date range:
  - Number of Import Records created
  - Of which: number of P1, P2, P3

- [x] **AC-2 (Random inspection flagging — actual vs expected):**  
  The report shows, for the selected date range:
  - Actual number of Import Records automatically flagged for random Post Import Check (2% P3 and 10% P2 rules)
  - Expected number based on policy thresholds (to allow comparison)

## Business Rules

- [BR-007](../business-rules.md#br-007) — P2 10% rule
- [BR-008](../business-rules.md#br-008) — P3 2% rule

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-015](US-015-Automated-Risk-Assessment-P2.md), [US-016](US-016-Automated-Risk-Assessment-P3-Random.md) (Import Record and risk assessment)

## Traceability

### Source Jira Issues

- IMTA-6658

### Original Links

- IMTA-6658
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- None evidenced in this review.

## Implementation Confidence

Medium

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: Source record IMTA-6658 was empty; criteria inferred from the field list. The report specification is not a direct one-to-one match to the story.

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                       |
| -------------------- | ------------- | ---------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Report shows daily Import Record volumes by risk level (P1, P2, P3 breakdown per day)          |
| AC-2                 | ✅ Implemented | Report shows actual vs expected random inspection flagging (2% P3 and 10% P2 rules compliance) |
