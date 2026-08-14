# Epic: Post Import Check Management

## Purpose

Enable caseworkers to manage the lifecycle of Post Import Checks from flagging through to outcome recording, including manual override capabilities.

## Business Value

- Ensures the correct proportion of consignments are inspected per Defra policy.
- Provides caseworkers with the ability to respond to risk manually where the automated rules do not capture the full picture.
- Provides a complete audit trail of inspection decisions and outcomes.

## Capability Description

PIMS automatically flags Import Records for Post Import Checks based on risk assessment rules. Caseworkers can then:

- Create and update one or more Post Import Check records linked to the same Import Record over time.
- Override the system-determined Post Import Check requirement (skip or manually schedule).
- Record the outcome of a Post Import Check.
- Decide whether to revoke a Place of Origin's Gold Trust Level following an unsatisfactory outcome.

## Functional Scope

- Create and update Post Import Check records, including multiple sequenced checks against the same Import Record where required
- View and filter Post Import Check records (due today, this week, this month)
- Record Post Import Check outcomes (Satisfactory, Not Visited, Unsatisfactory, Non-Compliant, Resolved Not Required, Cancelled, Quarantined, Additional Inspection Required)
- Manually skip a system-required Post Import Check (with declined reason)
- Manually schedule a Post Import Check for a record not flagged by the system
- Decision gate for revoking Gold Trust Level on unsatisfactory outcome (BR-013)

## Associated User Stories

| Story                                                                 | Title                                                |
| --------------------------------------------------------------------- | ---------------------------------------------------- |
| [US-023](../user-stories/US-023-Post-Import-Check-Management.md)      | Post Import Check Management                         |
| [US-024](../user-stories/US-024-Manual-Post-Import-Check-Override.md) | Manual Post Import Check Override                    |
| [US-021](../user-stories/US-021-Revoke-Gold-Trust-Level.md)           | Revoke Gold Trust Level After Unsatisfactory Outcome |

## Source Jira Issues

IMTA-6253, IMTA-6128, IMTA-6669, IMTA-6699, IMTA-5866
