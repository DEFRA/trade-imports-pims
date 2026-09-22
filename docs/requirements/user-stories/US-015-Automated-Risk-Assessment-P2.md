# US-015: Automated Risk Assessment — P2 Random 10%

## Summary

As an EU Imports Caseworker,  
I want PIMS to flag whether the Defra policy-based 10% P2 random inspection rule applies to Import Records,  
So that I can schedule a Post Import Check if required.

## Description

PIMS applies the 10% P2 inspection coverage rule using two counters (Auto Numbers in the Reference Data area):
- **Import Application Priority 2 Counter** — counts Import Records since the last P2 inspection.
- **Import Application Priority 2 Quota Counter** — tracks inspections owed due to cases that were previously flagged for inspection but subsequently had their Risk Level changed away from P2.

The rule runs whenever an Import Record's Risk Level is set to or changed from P2. If a case moves from P2 before being inspected, the inspection obligation is preserved for a future case.

## Acceptance Criteria

- **AC-1 (On deployment):** Two counters are initialised: Import Application Priority 2 Counter = 0 and Import Application Priority 2 Quota Counter = 0.

- **AC-2 (Risk Level changed to P2 — Quota Counter > 0):**  
  Decrement the Quota Counter by 1. Flag the Import Record for Post Import Check:
  - Post Import Checks Required? = **Yes**
  - Post Import Checks Required Reason = **10% P2-Case Rule**

- **AC-3 (Risk Level changed to P2 — Quota Counter = 0, counter below limit):**  
  Increment the Import Application Priority 2 Counter by 1. Do not flag for inspection.

- **AC-4 (Risk Level changed to P2 — Quota Counter = 0, counter reaches limit of 10):**  
  Reset the Import Application Priority 2 Counter to 0. Flag the Import Record for Post Import Check:
  - Post Import Checks Required? = **Yes**
  - Post Import Checks Required Reason = **10% P2-Case Rule**

- **AC-5 (Risk Level changed away from P2 — record was flagged for inspection):**  
  Increment the Quota Counter by 1 (to ensure a future case covers this inspection).

- **AC-6 (Risk Level changed away from P2 — record was not flagged for inspection):**  
  Decrement the Priority 2 Counter by 1 (to maintain the inspection ratio; counter can go negative).

- **AC-7 (Bulk rebalancing):**  
  If the Quota Counter > 0 AND the Priority 2 Counter ≤ -10: decrement the Quota Counter by 1 and add 10 to the Priority 2 Counter.

## Business Rules

- [BR-007](../business-rules.md#br-007) — P2 10% random inspection coverage

## Dependencies

- [US-013](US-013-Manage-Inspection-Coverage-Rules.md) (Inspection Coverage Rules define the 10% threshold)
- [US-011](US-011-Manage-Commodity-Risk-Levels.md) (P2 classification is a prerequisite)

## Traceability

### Source Jira Issues

- IMTA-5895
