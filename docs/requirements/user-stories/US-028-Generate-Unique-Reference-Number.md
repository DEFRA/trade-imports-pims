# US-028: Generate Unique Reference Number

## Summary

As an EU Imports Caseworker,  
I want Dynamics 365 to generate a unique reference number for each Import Record I create,  
So that the reference number can be used on a UK Health Certificate in the event of no access to TRACES.

## Description

Each Import Record is assigned a globally unique reference number at creation in the format `GB.{YEAR}.{9-digit-sequential-number}`. The prefix (default `GB.`) is configurable by an EU Imports Business Rules Admin. The number must be generated synchronously on save of the Import Record.

## Acceptance Criteria

- **AC-1 (Unique ID generated on creation):**  
  When an Import Record is created, Dynamics 365 generates and assigns a unique reference number with the following format:
  - Prefix: `GB.` (configurable by EU Imports Business Rules Admin)
  - Year: current 4-digit year
  - Sequence: 9-digit zero-padded sequential number
  - Example: `GB.2019.000000009`

- **AC-2 (Prefix configurable):**  
  The prefix (default `GB.`) can be changed by an EU Imports Business Rules Admin without a code change.

- **AC-3 (Generated on save):**  
  The unique reference number is generated synchronously when the Import Record is first saved.

## Business Rules

- [BR-022](../business-rules.md#br-022) — Unique reference number format

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record — unique reference is set on the Import Record)

## Traceability

### Source Jira Issues

- IMTA-6132
