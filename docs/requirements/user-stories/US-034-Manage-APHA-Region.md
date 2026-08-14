# US-034: Manage APHA Region

## Summary

As an EU Imports Caseworker Admin,  
I want to be able to create and update APHA Region records,  
So that Import Records can later be linked to a pre-defined APHA Region to identify which APHA Region is responsible for a Post Import Check.

## Description

APHA Regions are reference data records used to indicate the Animal and Plant Health Agency regional responsibility for Post Import Checks. Caseworker Admins manage the list of APHA Regions; Caseworkers have read-only access to link them to Import Records.

## Acceptance Criteria

- [x] **AC-1 (Create/update APHA Region):**  
  An EU Imports Caseworker Admin can create or update an APHA Region record with the following field:
  - Name (Free text — Mandatory)

- [x] **AC-2 (Search for APHA Region):**  
  An EU Imports Caseworker Admin can navigate to APHA Regions from the EU Imports application and search for an APHA Region by Name.

- [x] **AC-3 (Security):**  
  EU Imports Caseworker Admin role has Create/Update/Assign/Append/Append To privileges on the APHA Region entity. EU Imports Caseworker role has Read/Append/Append To privileges only.

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record may reference an APHA Region)

## Traceability

### Source Jira Issues

- IMTA-6165

### Original Links

- IMTA-6165
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Entities/defraimp_apharegion/Entity.xml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                    |
| -------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | src/solutions/defra_Imports/src/Entities/defraimp_apharegion/Entity.xml entity with Name (mandatory)                                        |
| AC-2                 | ✅ Implemented | APHA Regions available in EU Imports app navigation with search by Name                                                                     |
| AC-3                 | ✅ Implemented | Security role configuration: Business Rules Admin role has Create/Update/Assign/Append/Append To; Caseworker role has Read/Append/Append To |
