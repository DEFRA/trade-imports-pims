# US-019: Lock Place of Origin to Bronze

## Summary

As an EU Imports Caseworker,  
I want to be able to lock the Trust Level of a Place of Origin to Bronze,  
So that I can prevent PIMS from automatically promoting the Trust Level to Gold even after 3 consecutive satisfactory outcomes.

## Description

Provides caseworkers with a manual override to prevent a Place of Origin from being automatically promoted to Gold Trust Level. Locking requires a mandatory reason. Unlocking also requires a mandatory reason and restores the previous Trust Level. Both actions are audited.

## Acceptance Criteria

- [x] **AC-1:** An EU Imports Caseworker can set the Lock to Bronze field on a Place of Origin to Yes or No.

- [x] **AC-2:** While Lock to Bronze = Yes, PIMS does not promote the Trust Level to Gold regardless of the number of consecutive satisfactory outcomes.

- [x] **AC-3:** PIMS audits the user identity, date and time each time the Lock to Bronze field is changed.

- [x] **AC-4:** PIMS records the date when the Lock to Bronze field was last set to Yes in a field named Locked to Bronze Date (read-only, displayed next to the field).

- [x] **AC-5:** When the Lock to Bronze field is set to Yes, the user must provide a non-empty value in the Locked to Bronze Reason field before saving. PIMS prevents saving if this field is empty.

- [x] **AC-6:** PIMS records the date when the Lock to Bronze field was last changed from Yes to No in a field named Unlocked from Bronze Date.

- [x] **AC-7:** When the Lock to Bronze field is changed from Yes to No, the user must provide a non-empty value in the Unlocked from Lock to Bronze Reason field before saving.

- [x] **AC-8:** When the Lock to Bronze field is changed from Yes to No, PIMS restores the Trust Level to the value held in the Previous Trust Level field (see [US-020](US-020-Update-Place-of-Origin-on-Import-Record.md) / BR-015).

- [x] **AC-9:** Before setting Lock to Bronze = Yes, PIMS records the current Trust Level in a read-only field named Previous Trust Level.

## Business Rules

- [BR-014](../business-rules.md#br-014) — Lock to Bronze prevents Gold promotion
- [BR-015](../business-rules.md#br-015) — Unlock from Bronze restores previous Trust Level
- [BR-016](../business-rules.md#br-016) — Locking requires a mandatory reason
- [BR-017](../business-rules.md#br-017) — Unlocking requires a mandatory reason

## Dependencies

- [US-017](US-017-Manage-Place-of-Origin.md) (Place of Origin entity)
- [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md) (Trust Level maintenance)

## Traceability

### Source Jira Issues

- IMTA-5890
- IMTA-6668

### Original Links

- IMTA-5890
- IMTA-6668
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

| Acceptance Criterion | Status        | Evidence                                                                            |
| -------------------- | ------------- | ----------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Lock to Bronze field (Yes/No) on Place of Origin entity                             |
| AC-2                 | ✅ Implemented | Lock to Bronze = Yes prevents Gold promotion regardless of counter                  |
| AC-3                 | ✅ Implemented | Audit trail on Lock to Bronze field changes                                         |
| AC-4                 | ✅ Implemented | Locked to Bronze Date field (read-only) records lock timestamp                      |
| AC-5                 | ✅ Implemented | Lock to Bronze = Yes requires non-empty Locked to Bronze Reason before save         |
| AC-6                 | ✅ Implemented | Unlocked from Bronze Date field records unlock timestamp                            |
| AC-7                 | ✅ Implemented | Lock to Bronze change from Yes to No requires non-empty Unlocked from Bronze Reason |
| AC-8                 | ✅ Implemented | Unlock restores Trust Level to Previous Trust Level field value                     |
| AC-9                 | ✅ Implemented | Previous Trust Level field records current level before lock set                    |
