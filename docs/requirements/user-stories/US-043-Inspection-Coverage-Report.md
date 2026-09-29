# US-043: Inspection Coverage Audit Report

## Summary

As a Team Leader / Data Team Member,  
I want to be able to view an Inspection Coverage Audit Report,  
So that I can verify that the 2% P3 and 10% P2 random inspection rules are being applied correctly.

## Description

A report showing daily Import Record creation volumes, risk level breakdown and actual vs expected random inspection flagging counts. This supports compliance demonstration and identification of anomalies in the inspection counter logic.

**Note:** IMTA-6658 did not include a formal user story or acceptance criteria. These requirements are inferred from the field list provided and may not be a direct one-to-one match to the original report specification.

## Acceptance Criteria

- **AC-1 (Daily Import Record volume by risk level):**  
  The report shows, for each day in the selected date range:
  - Number of Import Records created
  - Of which: number of P1, P2, P3

- **AC-2 (Random inspection flagging — actual vs expected):**  
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
