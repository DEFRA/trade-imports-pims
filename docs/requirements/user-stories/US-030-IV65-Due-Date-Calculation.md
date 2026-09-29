# US-030: IV65 Response Due Date Calculation

## Summary

As an EU Imports Caseworker,  
I want Dynamics 365 to calculate the date a response to an IV65 is due,  
So that I can follow up on overdue responses without manually calculating dates.

## Description

When a caseworker sets the IV65 Sent Date on an Import Record, PIMS automatically calculates the IV65 Response Due Date as IV65 Sent Date + 14 calendar days. The calculated date remains editable by caseworkers to support exceptional circumstances.

## Acceptance Criteria

- **AC-1 (Auto-calculate IV65 Response Due Date):**  
  When a user changes the IV65 Sent Date field on an Import Record, PIMS calculates the IV65 Response Due Date = IV65 Sent Date + 14 calendar days.

- **AC-2 (Response Due Date remains editable):**  
  The IV65 Response Due Date field remains editable by EU Imports Caseworkers after it has been auto-calculated.

## Business Rules

- [BR-024](../business-rules.md#br-024) — IV65 Response Due Date = IV65 Sent Date + 14 calendar days

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record — IV65 fields reside on the Import Record)

## Traceability

### Source Jira Issues

- IMTA-6166
