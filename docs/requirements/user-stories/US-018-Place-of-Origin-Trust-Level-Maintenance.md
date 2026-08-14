# US-018: Place of Origin Trust Level Maintenance

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically maintain the Trust Level of a Place of Origin based on the outcomes of Post Import Checks,  
So that when I create an Import Record and associate it with a Place of Origin, PIMS can automatically determine whether an inspection is required.

## Description

PIMS automatically manages the Trust Level (Gold/Bronze) of each Place of Origin based on post-import check outcomes. Three counters are maintained per Place of Origin:

1. **Number of Consecutive Satisfactory Import Records** — drives Gold promotion.
2. **Number of Import Records** — tracks how many Import Records with a Primary ITAHC are linked.
3. **Number of Import Records Since Last Post Import Check** — used by the Gold 1-in-10 inspection rule.

## Acceptance Criteria

- [x] **AC-1:** When a Place of Origin is created, its Trust Level defaults to Bronze.

- [x] **AC-2 (Consecutive satisfactory count — increment):**  
  When an Import Record with a Gold/Bronze Commodity and a Place of Origin is completed with Post Import Check Outcome = Satisfactory or Not Visited, increment the Number of Consecutive Satisfactory Import Records for the Place of Origin by 1.

- [x] **AC-3 (Consecutive satisfactory count — reset):**  
  When an Import Record with a Gold/Bronze Commodity and a Place of Origin is completed with Post Import Check Outcome = Unsatisfactory, reset the Number of Consecutive Satisfactory Import Records for the Place of Origin to 0.

- [x] **AC-4 (Gold promotion after 3 consecutive satisfactory outcomes):**  
  After 3 consecutive Satisfactory or Not Visited outcomes, if the Place of Origin Trust Level is Bronze and Lock to Bronze = No, PIMS promotes the Trust Level to Gold.

- [x] **AC-5 (Bronze demotion after unsatisfactory + Gold revocation):**  
  When a Post Import Check Outcome is Unsatisfactory AND the user sets Reset Gold Trust Level to Bronze? = Yes on the Import Record, PIMS sets the Trust Level to Bronze.

- [x] **AC-6 (Number of Import Records — increment):**  
  When an Import Record with a Primary ITAHC is linked to a Place of Origin, increment the Number of Import Records counter on the Place of Origin by 1.

- [x] **AC-7 (Number of Import Records — decrement):**  
  When a Place of Origin is unlinked from an Import Record, or the Primary ITAHC is cleared, decrement the Number of Import Records counter by 1.

- [x] **AC-8 (Number of Import Records Since Last Check — increment):**  
  When an Import Record with a Gold/Bronze Commodity and a Gold Trust Level Place of Origin is completed, increment the Number of Import Records Since Last Post Import Check on the Place of Origin by 1.

## Business Rules

- [BR-010](../business-rules.md#br-010) — Bronze default on creation
- [BR-011](../business-rules.md#br-011) — Gold promotion after 3 consecutive satisfactory outcomes
- [BR-012](../business-rules.md#br-012) — Consecutive count resets on Unsatisfactory
- [BR-013](../business-rules.md#br-013) — Trust Level revoked to Bronze on Unsatisfactory + user confirmation
- [BR-018](../business-rules.md#br-018) — Number of Import Records counter maintained
- [BR-019](../business-rules.md#br-019) — Number Since Last Check counter incremented for Gold Places of Origin

## Dependencies

- [US-017](US-017-Manage-Place-of-Origin.md) (Place of Origin entity)
- [US-021](US-021-Revoke-Gold-Trust-Level.md) (User decision to revoke Gold Trust Level)
- [US-023](US-023-Post-Import-Check-Management.md) (Post Import Check outcome recording)

## Traceability

### Source Jira Issues

- IMTA-5886

### Original Links

- IMTA-5886
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

| Acceptance Criterion | Status        | Evidence                                                                                                                                    |
| -------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | New Place of Origin defaults to Bronze Trust Level (BR-010)                                                                                 |
| AC-2                 | ✅ Implemented | Consecutive Satisfactory counter incremented on Post Import Check Outcome = Satisfactory or Not Visited with Gold/Bronze Commodity          |
| AC-3                 | ✅ Implemented | Consecutive Satisfactory counter reset to 0 on Outcome = Unsatisfactory                                                                     |
| AC-4                 | ✅ Implemented | After 3 consecutive Satisfactory outcomes (Lock to Bronze = No) → promote to Gold                                                           |
| AC-5                 | ✅ Implemented | Unsatisfactory + Reset Gold = Yes → set Trust Level to Bronze                                                                               |
| AC-6                 | ✅ Implemented | Primary ITAHC linked → increment Number of Import Records counter on Place of Origin                                                        |
| AC-7                 | ✅ Implemented | Place of Origin unlinked or Primary ITAHC cleared → decrement Number of Import Records counter                                              |
| AC-8                 | ✅ Implemented | Import Record with Gold/Bronze Commodity + Gold Place of Origin completed → increment Number of Import Records Since Last Post Import Check |
