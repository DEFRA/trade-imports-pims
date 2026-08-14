# US-036: Audit Post Import Checks

## Summary

As an EU Imports Team Leader,  
I want to ensure that all changes to Post Import Check records are audited,  
So that all historic changes can be tracked along with the identity of the user that made them.

## Description

Mirrors [US-035](US-035-Audit-Import-Records.md) for the Post Import Check entity. D365 auditing must be enabled on the Post Import Check entity and all its custom fields.

## Acceptance Criteria

- [x] **AC-1 (Field-level audit on Post Import Check records):**  
  Dynamics 365 must capture changes to all updatable fields on a Post Import Check record, including the user identity, date, time and old/new values. This must be visible in the record's audit history.

## Business Rules

None additional.

## Dependencies

- D365 organisation-level auditing enabled (CON-004)
- [US-023](US-023-Post-Import-Check-Management.md) (Post Import Check entity)

## Traceability

### Source Jira Issues

- IMTA-6128

### Original Links

- IMTA-6128
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

| Acceptance Criterion | Status        | Evidence                                                            |
| -------------------- | ------------- | ------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | D365 auditing enabled on Post Import Check entity and custom fields |
