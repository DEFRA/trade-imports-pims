# US-031: Completion Date Recording

## Summary

As an EU Caseworker,  
I want Dynamics 365 to record the date an Import Record is moved to Completion,  
So that Defra can measure the end-to-end processing time from receipt to post-import check scheduling.

## Description

When a caseworker marks an Import Record as complete (Moved to Completion? = Yes), PIMS automatically records the current date and time in a read-only Moved to Completion Date field. If the caseworker reverses this (sets to No), the date is cleared.

## Acceptance Criteria

- **AC-1 (Journal completion date):**  
  When a user sets the Moved to Completion? field to Yes, PIMS records the current date and time in the Moved to Completion Date field. This field is read-only.

- **AC-2 (Clear completion date on reversal):**  
  When a user sets the Moved to Completion? field back to No, PIMS clears the Moved to Completion Date field.

## Business Rules

- [BR-026](../business-rules.md#br-026) — Moved to Completion Date journalled automatically

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record)

## Traceability

### Source Jira Issues

- IMTA-6180
