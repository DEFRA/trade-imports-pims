# US-005: Manage CVED

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update, view and search for CVED records,  
So that I can record information relating to a Common Veterinary Entry Document consignment.

## Description

A CVED (Common Veterinary Entry Document) is a certificate used for goods imported from third countries at a Border Inspection Post. CVED records are managed manually by caseworkers in PIMS.

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can create or update a CVED record with the following fields:
  - Certificate Reference Number (Mandatory)

- **AC-2:** An EU Imports Caseworker can view a list of all CVED records ordered by creation date (newest first), showing: Certificate Reference Number, Created On.

- **AC-3:** An EU Imports Caseworker can perform a free text search for a CVED by Certificate Reference Number.

## Business Rules

None specific to this entity.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record may reference a CVED)

## Traceability

### Source Jira Issues

- IMTA-6357
