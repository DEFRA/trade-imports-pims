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
- **Importer Notification** — the implemented IPAFFS-sourced notification entity (auto-received)
- **Match Record** — matching support artefact used to identify candidate related Import Records (summarised here via [US-046](../user-stories/US-046-Match-Inbound-Records-to-Import-Records.md); the detailed matching algorithm, review process and related capabilities are specified in [EPIC-Matching-Process](./EPIC-Matching-Process.md))

Supporting case management capabilities include document attachment, IV65 response tracking, warble fly declaration tracking, completion date recording, matching support and quick-create for triage. Business-facing requirements use **Import Record** as the canonical term, while some technical artefacts still use `importapplication` / "Import Application".

## Functional Scope

- Create, update, view, list and search for Import Records, ITAHCs, DOCOMs and CVEDs; view, list, search and perform limited updates on Importer Notifications
- Link health certificates to Import Records
- Attach documents to Import Records
- Calculate IV65 response due dates
- Record warble fly treatment declaration dates
- Record Import Record completion dates
- Quick-create Import Records for the unassigned work queue
- View and search Importer Notifications received from IPAFFS
- Select "No ITAHC Received" on an Import Record where applicable
- Review Match View candidates and Work Schedule Number context when matching inbound records
- Record DOCOM-specific triage details (category, Proof of Delivery request/reply) on the Import Record
- Record and track non-compliance queries against Importer Notifications and Import Records
- Auto-populate the Date Importer Notification Received field from the linked Importer Notification's Submission Date

## Associated User Stories

| Story                                                                       | Title                                   |
| --------------------------------------------------------------------------- | --------------------------------------- |
| [US-001](../user-stories/US-001-Manage-Import-Record.md)                    | Manage Import Record                    |
| [US-002](../user-stories/US-002-Manage-ITAHC.md)                            | Manage ITAHC                            |
| [US-003](../user-stories/US-003-Manage-Importer-Notification.md)              | Manage Importer Notification            |
| [US-004](../user-stories/US-004-Manage-DOCOM.md)                            | Manage DOCOM                            |
| [US-005](../user-stories/US-005-Manage-CVED.md)                             | Manage CVED                             |
| [US-029](../user-stories/US-029-Document-Attachment.md)                     | Document Attachment                     |
| [US-030](../user-stories/US-030-IV65-Due-Date-Calculation.md)               | IV65 Response Due Date Calculation      |
| [US-031](../user-stories/US-031-Completion-Date-Recording.md)               | Completion Date Recording               |
| [US-032](../user-stories/US-032-Warble-Fly-Declaration-Date.md)             | Warble Fly Treatment Declaration Date   |
| [US-033](../user-stories/US-033-Quick-Create-Import-Record.md)              | Quick Create Import Record              |
| [US-044](../user-stories/US-044-View-Importer-Notification.md)              | View Importer Notification in PIMS      |
| [US-046](../user-stories/US-046-Match-Inbound-Records-to-Import-Records.md) | Match Inbound Records to Import Records |
| [US-049](../user-stories/US-049-Non-Compliance-Management.md)              | Non-Compliance Management               |

## Source Jira Issues

IMTA-5868, IMTA-5869, IMTA-5870, IMTA-5913, IMTA-6119, IMTA-6121, IMTA-6132, IMTA-6158, IMTA-6166, IMTA-6180, IMTA-6252, IMTA-6357, IMTA-6411, IMTA-7201, IMTA-7379, IMTA-7466, IMTA-7468, IMTA-7469, IMTA-7556, IMTA-7566, IMTA-7779, IMTA-7782, IMTA-8219, IMTA-8589, IMTA-8738, IMTA-9133, IMTA-9134, IMTA-9144, PLNT-4535, PLNT-4536, PLNT-4537, PLNT-4538, PLNT-4539, PLNT-4540, PLNT-4541, PLNT-4542, PLNT-4543, EDA-194, EDA-234, EDA-235, EDA-280, EDA-303, EDA-304, EDA-307, EDA-308, EDA-322, EDA-337, EDA-338, EDA-353, EDA-399, EDA-400, EDA-432, EDA-620, EDA-621, EDA-642, EDA-643, EDA-644, EDA-648, EDA-649, EDA-656, EDA-680, EDA-681, EDA-696, EDA-705, EDA-708, EDA-725, EDA-728, EDA-737, EDA-739, EDA-794, EDA-798
