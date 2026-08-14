# Epic: Consignment Record Management

## Purpose

Enable EU Imports Caseworkers to create, manage and complete all types of consignment health certificate records and the Import Record that links them, throughout the full case lifecycle.

## Business Value

Caseworkers can manage the complete EU imports workflow within a single system, replacing fragmented paper and spreadsheet processes with a traceable, auditable digital record.

## Capability Description

PIMS provides caseworkers with the ability to create and manage the following record types:

- **Import Record** — the primary case record linking health certificates, place of origin, risk assessment and post-import check
- **ITAHC** — International Transport of Animals Health Certificate
- **DOCOM** — Document of Commercial Movement
- **CVED** — Common Veterinary Entry Document
- **Import Notification** — legacy pre-notification wording retained in some labels, views and dashboard text
- **Importer Notification** — the implemented IPAFFS-sourced notification entity (auto-received)
- **Match Record** — matching support artefact used to identify candidate related Import Records

Supporting case management capabilities include document attachment, IV65 response tracking, warble fly declaration tracking, completion date recording, matching support and quick-create for triage. Business-facing requirements use **Import Record** as the canonical term, while some technical artefacts still use `importapplication` / "Import Application".

## Functional Scope

- Create, update, view, list and search for Import Records, ITAHCs, DOCOMs, CVEDs and Import Notifications
- Link health certificates to Import Records
- Attach documents to Import Records
- Calculate IV65 response due dates
- Record warble fly treatment declaration dates
- Record Import Record completion dates
- Quick-create Import Records for the unassigned work queue
- View Importer Notifications received from IPAFFS
- Select "No ITAHC Received" on an Import Record where applicable
- Review Match View candidates and Work Schedule Number context when matching inbound records

## Associated User Stories

| Story                                                                       | Title                                   |
| --------------------------------------------------------------------------- | --------------------------------------- |
| [US-001](../user-stories/US-001-Manage-Import-Record.md)                    | Manage Import Record                    |
| [US-002](../user-stories/US-002-Manage-ITAHC.md)                            | Manage ITAHC                            |
| [US-003](../user-stories/US-003-Manage-Import-Notification.md)              | Manage Import Notification              |
| [US-004](../user-stories/US-004-Manage-DOCOM.md)                            | Manage DOCOM                            |
| [US-005](../user-stories/US-005-Manage-CVED.md)                             | Manage CVED                             |
| [US-029](../user-stories/US-029-Document-Attachment.md)                     | Document Attachment                     |
| [US-030](../user-stories/US-030-IV65-Due-Date-Calculation.md)               | IV65 Response Due Date Calculation      |
| [US-031](../user-stories/US-031-Completion-Date-Recording.md)               | Completion Date Recording               |
| [US-032](../user-stories/US-032-Warble-Fly-Declaration-Date.md)             | Warble Fly Treatment Declaration Date   |
| [US-033](../user-stories/US-033-Quick-Create-Import-Record.md)              | Quick Create Import Record              |
| [US-044](../user-stories/US-044-View-Importer-Notification.md)              | View Importer Notification in PIMS      |
| [US-046](../user-stories/US-046-Match-Inbound-Records-to-Import-Records.md) | Match Inbound Records to Import Records |

## Source Jira Issues

IMTA-5868, IMTA-5869, IMTA-5870, IMTA-5913, IMTA-5984, IMTA-5985, IMTA-6132, IMTA-6158, IMTA-6166, IMTA-6180, IMTA-6252, IMTA-6357, IMTA-6411, IMTA-7201, IMTA-7240
