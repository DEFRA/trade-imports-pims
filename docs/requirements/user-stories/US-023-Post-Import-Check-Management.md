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

- **AC-2:** An EU Imports Caseworker can update a Post Import Check record, including recording the outcome (Satisfactory, Not Visited, Unsatisfactory, Non-Compliant, Resolved Not Required, Cancelled, Quarantined, Additional Inspection Required).

- **AC-3:** An EU Imports Caseworker can view a list of Post Import Check records filtered by outcome (Not Started, In Progress, Completed).

- **AC-4:** An EU Imports Caseworker can view Post Import Checks due today, this week and this month (for use in dashboards).

- **AC-5:** The IV17 Received Date field is available on a Post Import Check record.

- **AC-6:** PIMS updates the Place of Origin Trust Level counters when a Post Import Check outcome is recorded on a completed Import Record (per [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md)).

## Business Rules

- [BR-011](../business-rules.md#br-011), [BR-012](../business-rules.md#br-012), [BR-013](../business-rules.md#br-013) — Trust level counters updated on outcome recording

## Dependencies

- [US-014](US-014-Automated-Risk-Assessment-P1.md), [US-015](US-015-Automated-Risk-Assessment-P2.md), [US-016](US-016-Automated-Risk-Assessment-P3-Random.md) (Risk assessment rules flag records for Post Import Check)
- [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md) (Trust Level maintenance triggered by Post Import Check outcome)
- [US-021](US-021-Revoke-Gold-Trust-Level.md) (Gold Trust Level revocation decision)

## Traceability

### Source Jira Issues

- IMTA-6253
- IMTA-6128
- IMTA-5866
- IMTA-6669
- IMTA-6015
