# US-032: Warble Fly Treatment Declaration Date

## Summary

As an EU Imports Caseworker,  
I want Dynamics 365 to only allow entry of the Warble Fly Treatment Declaration Received Date when a Warble Fly Treatment Declaration is Required,  
So that I can ensure the received date is only recorded when it is applicable.

## Description

The Warble Fly Treatment Declaration Received Date field on an Import Record must only be enabled when Warble Fly Treatment Declaration Required? = Yes. If the user sets the field back to No, the received date is cleared and the field disabled.

## Acceptance Criteria

- **AC-1 (Field only enabled when required):**  
  The Warble Fly Treatment Declaration Received Date field is only enabled when Warble Fly Treatment Declaration Required? = Yes. The field is disabled (and not editable) in all other states.

- **AC-2 (Clear date when no longer required):**  
  If a user changes Warble Fly Treatment Declaration Required? from Yes to No, PIMS clears the Warble Fly Treatment Declaration Received Date field and disables it.

## Business Rules

- [BR-025](../business-rules.md#br-025) — Warble Fly Treatment Declaration Received Date only enabled when Required = Yes

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record — Warble Fly fields reside on the Import Record)

## Traceability

### Source Jira Issues

- IMTA-6158
