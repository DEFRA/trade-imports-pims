# US-033: Quick Create Import Record

## Summary

As an EU Imports Caseworker,  
I want to be able to quickly create a bare-bones Import Record using a Quick Create form,  
So that records can be placed in an unassigned work queue immediately without completing all fields.

## Description

A Quick Create form allows caseworkers to create Import Records rapidly at triage with only the essential fields. The Owner field is cleared on form load to force the caseworker to explicitly assign the record to a team or user.

## Acceptance Criteria

- [x] **AC-1 (Quick Create form with essential fields):**  
  An EU Imports Caseworker can use a Quick Create form named "Import Record — Quick Create" to create an Import Record with the following fields:
  - Import Record Type
  - Devolved Office
  - Primary ITAHC
  - Commodity Type
  - Country of Origin
  - Region / Area Allocated to
  - Owner

- [x] **AC-2 (Owner field cleared on form load):**  
  The Owner field is automatically cleared when the Quick Create form loads, ensuring the caseworker must explicitly select an owning team or user.

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Full Import Record — Quick Create records are the same entity)

## Traceability

### Source Jira Issues

- IMTA-6411

### Original Links

- IMTA-6411
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Entities/defraimp_importapplication/FormXml/quickCreate/
- src/solutions/defra_Imports/src/Workflows/LockFieldsafterCreatedOnispopulated-EBC2455F-0DCC-EA11-A812-000D3AD82CAC.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                                      |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Quick Create form src/solutions/defra_Imports/src/Entities/defraimp_importapplication/FormXml/quickCreate/ includes: Import Record Type, Devolved Office, Primary ITAHC, Commodity Type, Country of Origin, Region / Area Allocated to, Owner |
| AC-2                 | ✅ Implemented | Owner field cleared on form load via workflow src/solutions/defra_Imports/src/Workflows/LockFieldsafterCreatedOnispopulated-EBC2455F-0DCC-EA11-A812-000D3AD82CAC.xaml forces caseworker selection                                             |
