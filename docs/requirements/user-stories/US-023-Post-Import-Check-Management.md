# US-023: Post Import Check Management

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update and view Post Import Check records,  
So that I can record and track inspection activities required for Import Records.

## Description

Post Import Checks are physical or document-based inspections of consignments. PIMS flags Import Records for Post Import Checks automatically ([US-014](US-014-Automated-Risk-Assessment-P1.md), [US-015](US-015-Automated-Risk-Assessment-P2.md), [US-016](US-016-Automated-Risk-Assessment-P3-Random.md)) or via manual override ([US-024](US-024-Manual-Post-Import-Check-Override.md)). This story covers the management of the Post Import Check record itself.

One Import Record may be linked to multiple Post Import Check records over time, including sequenced or follow-up checks where operationally required.

**Note:** The full acceptance criteria for the Post Import Check entity were not specified in the source records (IMTA-6253 was incomplete). The criteria below are inferred from related stories and remain subject to business confirmation.

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can create a Post Import Check record linked to an Import Record.

- **AC-1a:** A single Import Record can be linked to multiple Post Import Check records over time, and each check remains individually viewable and auditable.

- **AC-2:** An EU Imports Caseworker can update a Post Import Check record, including recording the outcome. The Outcome field offers Satisfactory, Unsatisfactory or Not Visited (defaulting to Awaiting Outcome until set). Where Outcome = Unsatisfactory, a Reason for Unsatisfactory Visit field must be completed (Non-Compliant welfare, Non-Compliant documentary check, Quarantined). Where Outcome = Not Visited, a Reason for Not Visiting field must be completed (Additional Inspection Required, Cancelled, Resolved Not Required, Lack of field resource).

- **AC-3:** An EU Imports Caseworker can view a list of Post Import Check records filtered by outcome (Not Started, In Progress, Completed).

- **AC-4:** An EU Imports Caseworker can view Post Import Checks due today, this week and this month (for use in dashboards).

- **AC-5:** The IV17 Received Date field is available on a Post Import Check record. The IV17 Chase Date and IV17 Received Date fields are unlocked and editable whenever the Date of Visit field is blank.

- **AC-6:** PIMS updates the Place of Origin Trust Level counters when a Post Import Check outcome is recorded on a completed Import Record (per [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md)): Satisfactory increments the consecutive satisfactory count, Unsatisfactory resets it to zero, Not Visited does not change it.

- **AC-7 (TB default):** When an Import Record's Risk Level is set to TB, Post Import Checks Required? defaults to No (Reason: "No Inspection Required"), except where the Import Record's Devolved Office is IRMS - Scotland, in which case it defaults to Yes (Reason: "TB"). The caseworker may still override the default ([US-024](US-024-Manual-Post-Import-Check-Override.md)).

- **AC-8 (Cancellation on case closure):** When an Import Record is closed, any linked Post Import Check with Outcome = Awaiting Outcome and no work started is set to Outcome = Not Visited with Reason for Not Visiting = Cancelled automatically; where work has started but no outcome is recorded, the caseworker is prompted to confirm before it is cancelled.

## Business Rules

- [BR-011](../business-rules.md#br-011), [BR-012](../business-rules.md#br-012), [BR-013](../business-rules.md#br-013) — Trust level counters updated on outcome recording
- [BR-047](../business-rules.md#br-047) — TB risk level default Post Import Check decision
- [BR-048](../business-rules.md#br-048) — Post Import Check outcome values and consecutive count impact
- [BR-049](../business-rules.md#br-049) — Open Post Import Checks cancelled on case closure

## Dependencies

- [US-014](US-014-Automated-Risk-Assessment-P1.md), [US-015](US-015-Automated-Risk-Assessment-P2.md), [US-016](US-016-Automated-Risk-Assessment-P3-Random.md) (Risk assessment rules flag records for Post Import Check)
- [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md) (Trust Level maintenance triggered by Post Import Check outcome)
- [US-021](US-021-Revoke-Gold-Trust-Level.md) (Gold Trust Level revocation decision)
- [US-059](US-059-Devolved-Office-Assignment.md) (Scotland exception to the TB default)

## Traceability

### Source Jira Issues

- IMTA-6253
- IMTA-6128
- IMTA-5866
- IMTA-6669
- IMTA-6015
- IMTA-6034
- IMTA-6031
- IMTA-7467
- EDA-231
- EDA-682
- EDA-750
