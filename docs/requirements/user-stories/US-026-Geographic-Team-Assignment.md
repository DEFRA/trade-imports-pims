# US-026: Geographic Team Assignment

## Summary

As an EU Imports Caseworker,  
I want to be able to assign an Import Record to a specific geographic team,  
So that risk assessment of consignments can be carried out by the appropriate local team.

## Description

Import Records are assigned to one of three geographic teams: IRMS Scotland, IRMS Wales, IRMS England. In the event of a regional disaster, all teams can view records from other teams to maintain continuity.

## Acceptance Criteria

- [x] **AC-1 (Assign Import Record to geographic team):**  
  An EU Imports Caseworker can assign an Import Record to one of the following teams:
  - IRMS — Scotland
  - IRMS — Wales
  - IRMS — England

- [x] **AC-2 (View other teams' records in a disaster scenario):**  
  In the event of a regional disaster, all geographic teams can view a list of Import Records owned by another team.

## Business Rules

None additional to standard D365 team ownership.

## Dependencies

- D365 Team records for each geographic team must be provisioned and replicated to all environments (ASM-008, DEP-006)

## Traceability

### Source Jira Issues

- IMTA-5863

### Original Links

- IMTA-5863
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

| Acceptance Criterion | Status        | Evidence                                                                                                   |
| -------------------- | ------------- | ---------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Team lookup on Import Record with teams: IRMS Scotland, IRMS Wales, IRMS England                           |
| AC-2                 | ✅ Implemented | Disaster continuity: all teams can view other teams' records (standard D365 team visibility configuration) |
