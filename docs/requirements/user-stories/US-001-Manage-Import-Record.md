# US-001: Manage Import Record

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update, view and search for Import Records,  
So that I can collate and record all information relating to the risk assessment of a consignment.

## Description

The Import Record is the primary case record in PIMS. It consolidates information from health certificates (ITAHC, DOCOM, CVED), import notifications and place of origin data into a single record that supports the full case lifecycle: Triage → Risk Assessment → Post Import Check → Completion.

A user can either manually create an Import Record or one may be auto-created by the TRACES integration ([US-009](US-009-Auto-Create-Import-Record-From-ITAHC.md), [US-010](US-010-Auto-Create-Import-Record-From-DOCOM.md)). A quick-create form is also available for rapid triage ([US-033](US-033-Quick-Create-Import-Record.md)).

The record supports "No ITAHC Received" as a valid option in the Primary ITAHC field, allowing the record to be saved without a linked certificate where one has not been presented. Source stories may refer to this option as "No ITAHC Provided".

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can create or update an Import Record with the following fields (all optional unless stated):

    - Import Record Type (see AC-7 for the value list — PLNT-4536)
    - Primary ITAHC (Lookup; includes "No ITAHC Received" option)
    - Primary Import Notification (Lookup)
    - GB Import Health Certificate, Traces Export Health Certificate, ITAHC Reference, DOCOM Reference (single line of text; non-mandatory; manually populated — PLNT-4535, see AC-5)
    - Devolved Office
    - Importer Name, Address (Line 1-3, City, Postcode), Telephone, Email
    - Country of Origin, Countries of Transit, Date of Import
    - Place of Destination (Contact Name, Address, Telephone, Email)
    - Permanent Destination (Contact Name, Address, City, Postcode, Telephone, Email)
    - Place of Origin (Contact Name, Address, Country)
    - Transporter (Organisation, Address, Telephone, Email)
    - Commodity Type, Commodity Notes, Quantity, Unit, Intended Use of Commodity
    - Port of Entry, Commodity Identifiers
    - Import Risk Level, Consignment Risk / Impact
    - Warble Fly Treatment Declaration Required, Received Date
    - General Comments
    - IV65 Sent, IV65 Sent Date, IV65 Response Received Date, IV65 Response Due Date
    - Date Importer Notification Received, Importer Notification Received within Timescales (previously labelled "Date IV66 Received" and "IV66 received in required timescales" — PLNT-4541; see AC-10)
    - Region / Area Allocated to
    - Moved to Completion?, Moved to Completion Date (read-only)

- **AC-2:** An EU Imports Caseworker can view a list of all Import Records ordered by creation date (newest first), showing: Primary ITAHC, Commodity Type, Country of Origin, Import Risk Level, Place of Origin Organisation, Place of Destination, Created On Date.

- **AC-3:** An EU Imports Caseworker can perform a free text search for an Import Record using: Importer Name, Date of Import, Premises of Origin Name (Place of Origin Organisation), ITAHC Certificate Reference Number, Import Notification Local Reference Number.

- **AC-4:** The user can select "No ITAHC Received" in the Primary ITAHC field and save the Import Record without a linked ITAHC.

- **AC-5 (Reference number fields):** GB Import Health Certificate, Traces Export Health Certificate, ITAHC Reference and DOCOM Reference are displayed as single-line text fields immediately below the Primary Import Notification field, in the Commodity section of the Summary tab. All four fields are non-mandatory and are populated manually; none are auto-populated by PIMS.

- **AC-6 (Triage step no longer clears Commodity Code):** Entering a value in the Primary ITAHC field during the Triage stage of the Import Record business process flow no longer removes or updates the Commodity Code value on the Import Record on save. Primary ITAHC carries no business process logic. This resolves the defect reported as DEFRA incident INC0838632.

- **AC-7 (Import Record Type value list):** The Import Record Type field offers exactly the following values, with no default selected: Importer Notification, Health Certificate, ITAHC - Landbridge, CHEDA, CHEDP, DOCOM, ITAHC. The legacy values CED, CVEDA and CVEDP are removed. Selecting "Create Import Record" on an Importer Notification sets the new Import Record's Import Record Type to Importer Notification.

- **AC-8 (DOCOM tab fields):** A DOCOM tab is displayed on the Import Record form immediately to the right of the Post Import Checks tab, with the section heading "DOCOM". The tab contains the following optional fields (no BR — CIT's off-system Proof of Delivery tracking process, see PLNT-4539):
    - DOCOM Category (Option Set — Cat1, Cat2, Cat3 - PAP, Cat3 – PAP Fish, Cat3 - Other)
    - Requested POD (Option Set — Blank, Y, N)
    - Date POD Requested (Date/Time)
    - Reply Received (Option Set — Blank, Y, N)

- **AC-9 (Auto-populate Date Importer Notification Received):** When an EU Imports Caseworker selects "Create Import Record" on an Importer Notification, PIMS auto-populates the new Import Record's Date Importer Notification Received field with the value of the associated Importer Notification's Submission Date field.

- **AC-10 (IV66 section relabelled to Importer Notification):** For records with Import Record Type = Importer Notification, the Import Record form's "IV66" section is labelled "Importer Notification", the "Date IV66 Received" field is labelled "Date Importer Notification Received", and the "IV66 received in required timescales" field is labelled "Importer Notification Received within Timescales".

## Business Rules

- [BR-002](../business-rules.md#br-002), [BR-003](../business-rules.md#br-003), [BR-004](../business-rules.md#br-004), [BR-005](../business-rules.md#br-005), [BR-006](../business-rules.md#br-006) — Post Import Check flagging rules applied on create/update
- [BR-022](../business-rules.md#br-022) — Unique reference number generated on creation
- [BR-024](../business-rules.md#br-024) — IV65 Response Due Date calculated when IV65 Sent Date is set
- [BR-025](../business-rules.md#br-025) — Warble Fly Treatment Declaration Received Date only enabled when Required = Yes
- [BR-026](../business-rules.md#br-026) — Moved to Completion Date journalled automatically
- [BR-028](../business-rules.md#br-028) — "No ITAHC Received" option available on Primary ITAHC field
- [BR-036](../business-rules.md#br-036) — Certificate/notification reference fields are optional and manually entered
- [BR-037](../business-rules.md#br-037) — Triage step must not clear or update Commodity Code
- [BR-038](../business-rules.md#br-038) — Import Record Type value list
- [BR-039](../business-rules.md#br-039) — Import Record Type set to Importer Notification when created from an Importer Notification
- [BR-040](../business-rules.md#br-040) — Date Importer Notification Received auto-populated from Submission Date

## Dependencies

- [US-002](US-002-Manage-ITAHC.md) (ITAHC lookup), [US-003](US-003-Manage-Import-Notification.md) (Import Notification lookup), [US-017](US-017-Manage-Place-of-Origin.md) (Place of Origin lookup)
- [US-011](US-011-Manage-Commodity-Risk-Levels.md) (Commodity Risk Level rules applied on create/update)
- [US-028](US-028-Generate-Unique-Reference-Number.md) (Unique reference number)
- [US-004](US-004-Manage-DOCOM.md) (DOCOM record — the DOCOM tab on the Import Record is additional triage context, not a replacement for the DOCOM entity)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md), [US-044](US-044-View-Importer-Notification.md) (Importer Notification — source of the "Create Import Record" button and Submission Date)

## Traceability

### Source Jira Issues

- IMTA-5870
- IMTA-5985
- PLNT-4535
- PLNT-4536
- PLNT-4539
- PLNT-4540
- PLNT-4541
