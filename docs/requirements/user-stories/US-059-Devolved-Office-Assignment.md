# US-059: Devolved Office Assignment

## Summary

As an EU Imports Caseworker,
I want the Devolved Office responsible for a consignment to be recorded and, where possible, set automatically,
So that casework is routed to the correct regional office without a manual postcode lookup.

## Description

A Devolved Office field identifies the regional Defra office responsible for an Importer Notification, ITAHC or DOCOM (see also the existing [glossary](../glossary.md) entry). It is set automatically on ITAHC and DOCOM records from the first part of the Place of Destination postcode, using a maintained postcode-to-office reference list, falling back to "Unknown" where the postcode is not recognised; it remains manually editable by caseworkers in all cases, and is always manual (no default) on the Importer Notification. The field is copied onto the Import Record from the source ITAHC at creation.

This story supersedes the earlier, ad-hoc approach of maintaining separate system views per country/office (England, Scotland, Wales) without a backing field; those views are replaced by the field-driven views in AC-5, which also cover the "Non-GB" and "Unknown" cases that the earlier views did not.

Devolved Office identifies the *responsible regional office* for a certificate or notification. It is distinct from [US-026](US-026-Geographic-Team-Assignment.md)'s Geographic Team, which governs D365 record ownership and visibility for an Import Record; the two may align operationally but are not the same field.

## Acceptance Criteria

- **AC-1 (Devolved Office field on Importer Notification):** An EU Imports Caseworker can view and select a Devolved Office field on the Importer Notification, with no default value, from: IRMS - CIT, IRMS - Scotland, IRMS - Wales.

- **AC-2 (Devolved Office field on ITAHC):** An EU Imports Caseworker can view and select a Devolved Office field on the ITAHC, with no default value, from: IRMS - CIT, IRMS - Scotland, IRMS - Wales, Non-GB, Unknown, or amend the value set automatically per AC-6.

- **AC-3 (Devolved Office field on DOCOM):** An EU Imports Caseworker can view and select a Devolved Office field on the DOCOM from: IRMS - CIT, IRMS - Scotland, IRMS - Wales, Non-GB, Unknown, or amend the value set automatically per AC-6.

- **AC-4 (Import Record inherits Devolved Office from ITAHC):** When an Import Record is created from an ITAHC, PIMS sets the Import Record's Devolved Office to the value held on the source ITAHC. Import Record creation is blocked, with a message prompting the caseworker to select a Devolved Office first, if the source ITAHC's Devolved Office is not set.

- **AC-5 (Devolved Office views):** An EU Imports Caseworker can select Active and Inactive ITAHC views for each Devolved Office value, including Non-GB and Unknown, each showing: Local Reference, Certificate Reference Number, Certificate Status, TRACES Notification Received Date, OV Name, Local Veterinary Unit, LVU No., Replaced By, Replaces, Created On, Owner.

- **AC-6 (Auto-set from postcode on ITAHC/DOCOM):** When PIMS receives an ITAHC or DOCOM, it automatically sets the Devolved Office using the first part of the Place of Destination postcode against a maintained reference list.

- **AC-7 (Unrecognised postcode):** Where the first part of the postcode is not on the reference list, Devolved Office is set to Unknown.

## Business Rules

- [BR-052](../business-rules.md#br-052) — Devolved Office auto-set and inheritance

## Dependencies

- [US-002](US-002-Manage-ITAHC.md), [US-003](US-003-Manage-Importer-Notification.md), [US-004](US-004-Manage-DOCOM.md), [US-001](US-001-Manage-Import-Record.md)
- [US-026](US-026-Geographic-Team-Assignment.md) (related but distinct Geographic Team concept)

## Traceability

### Source Jira Issues

- IMTA-7475
- IMTA-7780
- IMTA-7784
- IMTA-8103
- IMTA-8108
