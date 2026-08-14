# US-016: Automated Risk Assessment — P3 Random 2%

## Summary

As a Caseworker,  
I want PIMS to automatically flag a proportion of all P3 ITAHC Import Records for random inspection,  
So that we consistently inspect a random proportion of imported commodities aligned to current Defra policy.

## Description

PIMS applies the 2% random inspection coverage rule to all P3 Import Records of type ITAHC with a Primary ITAHC linked. A single Auto Number counter (**Import Application Priority 3 Counter**, visible under Reference Data > Auto Numbers) is maintained, incremented on creation (not update) of qualifying Import Records. When the counter reaches the configured threshold (defined by the "2% All-Case-Random" Inspection Coverage Rule), the record is flagged for inspection and the counter resets to 0.

The counter increment happens **before** automated risk assessment rules are evaluated (BR-009).

## Acceptance Criteria

- [x] **AC-1 (Counter incremented on qualifying Import Record creation):**  
  When an Import Record of type ITAHC with a linked Primary ITAHC is **created** (not updated), PIMS increments the **Import Application Priority 3 Counter** (Auto Number) by 1. This increment occurs before other risk assessment rules are evaluated.

- [x] **AC-2 (Counter below threshold — no inspection flagged):**  
  If Risk Level = P3 AND the counter is less than the configured limit:
  - Post Import Checks Required? = **No**
  - Post Import Checks Required Reason = **No inspection required**

- [x] **AC-3 (Counter reaches or exceeds threshold — inspection flagged, counter reset):**  
  If Risk Level = P3 AND the counter equals or exceeds the configured limit:
  - Post Import Checks Required? = **Yes**
  - Post Import Checks Required Reason = **Random P3 Inspection**
  - Counter is reset to 0

- [x] **AC-4 (Rule only applies to P3):**  
  If Risk Level is not P3, the 2% rule does not apply and no Post Import Check is flagged under this rule.

## Business Rules

- [BR-008](../business-rules.md#br-008) — P3 random 2% inspection coverage (ITAHC records only)
- [BR-009](../business-rules.md#br-009) — Counter incremented before risk assessment evaluation

## Dependencies

- [US-013](US-013-Manage-Inspection-Coverage-Rules.md) (Inspection Coverage Rules define the P3 threshold)
- [US-011](US-011-Manage-Commodity-Risk-Levels.md) (P3 classification is a prerequisite)

## Traceability

### Source Jira Issues

- IMTA-5867
- IMTA-5872
- IMTA-5892
- IMTA-5933

### Original Links

- IMTA-5867
- IMTA-5872
- IMTA-5892
- IMTA-5933
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

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                 |
| -------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------ |
| AC-1                 | ✅ Implemented | 2% Random counter incremented on P3 ITAHC Import Record **creation** (not update) before risk assessment rules evaluated |
| AC-2                 | ✅ Implemented | P3, counter < threshold → Post Import Checks Required? = No, Reason = "No inspection required"                           |
| AC-3                 | ✅ Implemented | P3, counter ≥ threshold → Post Import Checks Required? = Yes, Reason = "Random P3 Inspection", counter reset to 0        |
| AC-4                 | ✅ Implemented | Risk Level ≠ P3 → 2% rule does not apply                                                                                 |
