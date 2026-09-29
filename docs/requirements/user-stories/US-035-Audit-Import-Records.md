# US-035: Audit Import Records

## Summary

As an EU Imports Team Leader,  
I want to ensure that all changes to Import Records are audited,  
So that all historic changes can be tracked along with the identity of the user that made them.

## Description

D365 organisation-level auditing must be enabled on the Import Record entity, capturing all changes to updatable fields via the user interface.

## Acceptance Criteria

- **AC-1 (Field-level audit on Import Records):**  
  Dynamics 365 must capture changes to all updatable fields on an Import Record, including the user identity, date, time and old/new values. This must be visible in the record's audit history.

## Business Rules

None additional.

## Dependencies

- D365 organisation-level auditing enabled (CON-004)
- [US-001](US-001-Manage-Import-Record.md) (Import Record entity)

## Traceability

### Source Jira Issues

- IMTA-5986
