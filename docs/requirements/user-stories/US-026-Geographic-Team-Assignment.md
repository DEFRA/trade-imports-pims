# US-026: Geographic Team Assignment

## Summary

As an EU Imports Caseworker,  
I want to be able to assign an Import Record to a specific geographic team,  
So that risk assessment of consignments can be carried out by the appropriate local team.

## Description

Import Records are assigned to one of three geographic teams: IRMS Scotland, IRMS Wales, IRMS England. In the event of a regional disaster, all teams can view records from other teams to maintain continuity.

## Acceptance Criteria

- **AC-1 (Assign Import Record to geographic team):**  
  An EU Imports Caseworker can assign an Import Record to one of the following teams:
  - IRMS — Scotland
  - IRMS — Wales
  - IRMS — England

- **AC-2 (View other teams' records in a disaster scenario):**  
  In the event of a regional disaster, all geographic teams can view a list of Import Records owned by another team.

- **AC-3 (Region/Area Allocated to options):**  
  The Region / Area Allocated to field on the Import Record offers Transit and North 6 as additional selectable values, alongside the existing regional options.

## Business Rules

None additional to standard D365 team ownership.

## Dependencies

- D365 Team records for each geographic team must be provisioned and replicated to all environments (ASM-008, DEP-006)
- [US-059](US-059-Devolved-Office-Assignment.md) (related but distinct Devolved Office concept), [US-060](US-060-APHA-Owner-Team-Assignment.md) (additional APHA owner Teams)

## Traceability

### Source Jira Issues

- IMTA-5863
- EDA-619
