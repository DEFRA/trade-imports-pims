# US-055: Flag Multiple-Commodity Certificates for Caseworker Review

## Summary

As an EU Imports Caseworker,
I want Importer Notifications and ITAHCs with more than one commodity code to be flagged,
So that I know manual review is needed before matching and processing continue.

## Description

Both IPAFFS-sourced Importer Notifications and TRACES-sourced ITAHCs can arrive with more than one commodity code on a single record. PIMS flags these so a caseworker can review and confirm handling before the record proceeds through automated matching and risk assessment.

## Acceptance Criteria

- **AC-1 (Flag Importer Notification with multiple commodity codes):** When PIMS receives an Importer Notification with more than one commodity code, it is flagged with a banner "More than 1 Commodity Code - No caseworker intervention", appears in a dedicated view, and its Caseworker Intervention field is set to No. While Caseworker Intervention is No, the Importer Notification is excluded from the automated matching search ([US-051](US-051-Automated-Matching.md)) and from risk assessment, and only becomes eligible for those processes once Caseworker Intervention is set to Yes (AC-2).

- **AC-2 (Caseworker intervenes):** When a caseworker has reviewed a flagged Importer Notification and declares that they have intervened, the banner changes to "More than 1 Commodity Code - caseworker has intervened", the Importer Notification no longer appears in the no-intervention view, and the Caseworker Intervention field is set to Yes.

- **AC-3 (Equivalent flagging for ITAHCs):** PIMS applies equivalent flagging to an ITAHC received from TRACES Classic with more than one commodity code. *This AC is carried over from a source ticket whose full acceptance criteria were not available when this story was written; confirm the exact banner/view/field behaviour against the live system before implementation.*

## Business Rules

- None additional.

## Dependencies

- [US-002](US-002-Manage-ITAHC.md), [US-003](US-003-Manage-Importer-Notification.md)

## Traceability

### Source Jira Issues

- IMTA-7688
- IMTA-7783
