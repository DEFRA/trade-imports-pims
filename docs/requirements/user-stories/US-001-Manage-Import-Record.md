# US-001: Manage Import Record

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update, view and search for Import Records,  
So that I can collate and record all information relating to the risk assessment of a consignment.

## Description

The Import Record is the primary case record in PIMS. It consolidates information from health certificates (ITAHC, DOCOM, CVED), Importer Notifications and place of origin data into a single record that supports the full case lifecycle: Triage → Risk Assessment → Post Import Check → Completion.

A user can either manually create an Import Record or one may be created automatically at the point of confirming a match ([US-053](US-053-Create-Import-Record-at-Matching.md)). A quick-create form is also available for rapid triage ([US-033](US-033-Quick-Create-Import-Record.md)).

The record supports "No ITAHC Received" as a valid option in the Primary HC field, allowing the record to be saved without a linked certificate where one has not been presented. Source stories may refer to this option as "No ITAHC Provided".

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can create or update an Import Record with the following fields (all optional unless stated):

    - Import Record Type (see AC-7 for the value list — PLNT-4536)
    - Primary HC (Lookup; includes "No ITAHC Received" option; field label renamed from "Primary ITAHC" — carries no business process logic, see AC-6)
    - Primary Importer Notification (Lookup)
    - GB Import Health Certificate, Traces Export Health Certificate, ITAHC Reference, DOCOM Reference (single line of text; non-mandatory; manually populated — PLNT-4535, see AC-5)
    - Devolved Office (see [US-059](US-059-Devolved-Office-Assignment.md) — inherited from the source ITAHC at creation)
    - Region / Area Allocated to (includes Transit and North 6 options)
    - Importer Name, Address (Line 1-3, City, Postcode), Telephone, Email
    - Country of Origin, Countries of Transit, Date of Import
    - Place of Destination (Contact Name, Address, Telephone, Email)
    - Permanent Destination / Final Destination (Contact Name, Address, City, Postcode, Telephone, Email)
    - Place of Origin (Contact Name, Address, Country)
    - Transporter (Organisation, Address, Telephone, Email)
    - Transport to PoE (Means of Transport, ID of Transport, Document, Estimated Arrival Date, Estimated Arrival Time)
    - Transport after PoE (Means of Transport, ID of Transport, Document, Departure Date, Departure Time, Estimated Journey Time (mins)) — the Person Responsible for Transport field remains on the entity for historic/backdated records but is not shown on the form
    - Commodity Type (auto-set — see [US-061](US-061-Automatically-Determine-Commodity-Type.md)), Commodity Code, Commodity Notes, Quantity, Unit, Intended Use of Commodity, Weight (KG) (hidden for CHEDA — see [BR-046](../business-rules.md#br-046))
    - Purpose of Consignment, Internal Market Purpose, Certified For, Purpose for Movement, Number of Packages (journey-conditional visibility — see [BR-045](../business-rules.md#br-045))
    - Port of Entry, Commodity Identifiers (including Horse Name, shown only where the commodity is Horse), Commodity Permanent Address Information (CHEDA only — per-animal permanent address subgrid)
    - MRN Number (sourced from IPAFFS)
    - Cloned (Yes/No — identifies an Import Notification created via IPAFFS Clone Journey)
    - Import Risk Level, Consignment Risk / Impact, Inspection Required (Required / Not Required)
    - Warble Fly Treatment Declaration Required, Received Date
    - General Comments
    - IV65 Sent, IV65 Sent Date, IV65 Response Received Date, IV65 Response Due Date
    - Date Importer Notification Received, Importer Notification Received within Timescales (previously labelled "Date IV66 Received" and "IV66 received in required timescales" — PLNT-4541; see AC-9, AC-10)
    - Moved to Completion?, Moved to Completion Date (read-only)

- **AC-2:** An EU Imports Caseworker can view a list of all Import Records ordered by creation date (newest first), showing: Primary HC, Commodity Type, Country of Origin, Import Risk Level, Place of Origin Organisation, Place of Destination, Created On Date.

- **AC-3:** An EU Imports Caseworker can perform a free text search for an Import Record using: Importer Name, Date of Import, Premises of Origin Name (Place of Origin Organisation), ITAHC Certificate Reference Number, Importer Notification Local Reference Number.

- **AC-4:** The user can select "No ITAHC Received" in the Primary HC field and save the Import Record without a linked ITAHC.

- **AC-5 (Reference number fields):** GB Import Health Certificate, Traces Export Health Certificate, ITAHC Reference and DOCOM Reference are displayed as single-line text fields immediately below the Primary Importer Notification field, in the Commodity section of the Summary tab. All four fields are non-mandatory and are populated manually; none are auto-populated by PIMS.

- **AC-6 (Triage step no longer clears Commodity Code):** Entering a value in the Primary HC field during the Triage stage of the Import Record business process flow no longer removes or updates the Commodity Code value on the Import Record on save. Primary HC carries no business process logic. This resolves the defect reported as DEFRA incident INC0838632.

- **AC-7 (Import Record Type value list):** The Import Record Type field offers exactly the following values, with no default selected: Importer Notification, Health Certificate, ITAHC - Landbridge, CHEDA, CHEDP, DOCOM, ITAHC. The legacy values CED, CVEDA and CVEDP are removed. Selecting "Create Import Record" on an Importer Notification sets the new Import Record's Import Record Type to Importer Notification.

- **AC-8 (DOCOM tab fields):** A DOCOM tab is displayed on the Import Record form immediately to the right of the Post Import Checks tab, with the section heading "DOCOM". The tab contains the following optional fields (no BR — CIT's off-system Proof of Delivery tracking process, see PLNT-4539):
    - DOCOM Category (Option Set — Cat1, Cat2, Cat3 - PAP, Cat3 – PAP Fish, Cat3 - Other)
    - Requested POD (Option Set — Blank, Y, N)
    - Date POD Requested (Date/Time)
    - Reply Received (Option Set — Blank, Y, N)

- **AC-9 (Auto-populate Date Importer Notification Received):** When an EU Imports Caseworker selects "Create Import Record" on an Importer Notification, PIMS auto-populates the new Import Record's Date Importer Notification Received field with the Submission Date of the Importer Notification ([BR-040](../business-rules.md#br-040)).

- **AC-10 (IV66 section relabelled to Importer Notification):** For records with Import Record Type = Importer Notification, the Import Record form's "IV66" section is labelled "Importer Notification", the "Date IV66 Received" field is labelled "Date Importer Notification Received", and the "IV66 received in required timescales" field is labelled "Importer Notification Received within Timescales".

- **AC-11 (Notes retain the original creator after reassignment):** Where a caseworker adds a note to an Import Record, the note displays the name of the user who created it (Created By), not the current Owner of the Import Record. If the Import Record is subsequently reassigned to another user, previously-created notes continue to display their original creator.

- **AC-12 (Manual Import Record Type picklist excludes DOCOM):** When manually creating an Import Record, the Import Record Type picklist offers ITAHC, CHEDA and CHEDP only. DOCOM is not offered as a manually-selectable type, as a DOCOM-linked Import Record is created via the matching process ([US-053](US-053-Create-Import-Record-at-Matching.md)).

- **AC-13 (Imp Type field):** The Import Record form displays an Imp Type field positioned between Import Record Type and Commodity Type, offering Live Animals, POAO or HRFNAO, populated from a reference Imp Type lookup table (Imp Type Code, Imp Type Name). When the Import Record is created, Imp Type is set from the Imp Type value on the source Importer Notification or ITAHC. The field is hidden for the CHED journey (see [US-044](US-044-View-Importer-Notification.md) AC-7).

- **AC-14 (Transporter Type and extended Purpose options):** The Transporter section includes a Type field (Commercial Transporter, Private Transporter, Commercial Transporter – User Added) positioned after the Transporter Email field, set from the Type value on the source Importer Notification when the Import Record is created. For POAO and HRFNAO Imp Types, the Purpose field additionally accepts For Internal Market, For Transhipment, For Re-entry and Personally Owned Pets Not for Rehoming.

- **AC-15 (Transit and rejected consignments):** The Purpose / Certified For option set additionally includes Rejected or Returned Consignment and Transit. Port of Exit (text) and Port of Exit Date (date) fields are shown on the Import Record; both are optional and are only populated where Purpose is set to Transit.

- **AC-16 (Importer Notification Received within Timescales defaults to No):** The Importer Notification Received within Timescales field (see AC-10) defaults to No and remains visible on the Import Record form at all times; it is not conditionally hidden.

- **AC-17 (Field mapping when creating an Import Record from an Importer Notification):** In addition to the Date Importer Notification Received mapping in AC-9, creating an Import Record from an Importer Notification also copies: Devolved Office, Import Record Type, Commodity Type, Commodity Code, Species ID, Purpose, Quantity, Country of Origin, a link back to the source Importer Notification, Arrival Date, Port of Entry, Countries of Transit (from the Transit Details section), IV65 Sent, Warble Fly Treatment Declaration Required, Commodity Identifiers (Species Name and Identifiers), and any Watch Flags present on the Importer Notification.

- **AC-18 (Manual importer contact details and structured transporter address):** Where Importer contact details are not present or are incorrect on the source certificate or notification, an EU Imports Caseworker can manually record CPH (optional), Phone (mandatory) and Email (mandatory) against the Importer's Contact Name. The Transporter's Address field (see AC-1) is recorded as structured properties (Line 1, Line 2, Line 3, City, Postcode, Country) rather than free text.

## Business Rules

- [BR-002](../business-rules.md#br-002), [BR-003](../business-rules.md#br-003), [BR-004](../business-rules.md#br-004), [BR-005](../business-rules.md#br-005), [BR-006](../business-rules.md#br-006) — Post Import Check flagging rules applied on create/update
- [BR-022](../business-rules.md#br-022) — Unique reference number generated on creation
- [BR-024](../business-rules.md#br-024) — IV65 Response Due Date calculated when IV65 Sent Date is set
- [BR-025](../business-rules.md#br-025) — Warble Fly Treatment Declaration Received Date only enabled when Required = Yes
- [BR-026](../business-rules.md#br-026) — Moved to Completion Date journalled automatically
- [BR-028](../business-rules.md#br-028) — "No ITAHC Received" option available on Primary HC field
- [BR-036](../business-rules.md#br-036) — Certificate/notification reference fields are optional and manually entered
- [BR-037](../business-rules.md#br-037) — Triage step must not clear or update Commodity Code
- [BR-038](../business-rules.md#br-038) — Import Record Type value list
- [BR-039](../business-rules.md#br-039) — Import Record Type set to Importer Notification when created from an Importer Notification
- [BR-040](../business-rules.md#br-040) — Date Importer Notification Received auto-populated from Submission Date
- [BR-045](../business-rules.md#br-045) — Commodity field visibility depends on journey type
- [BR-046](../business-rules.md#br-046) — Weight (KG) hidden for CHEDA
- [BR-050](../business-rules.md#br-050) — Country code mapping for devolved nations
- [BR-051](../business-rules.md#br-051) — Commodity Type auto-set from Commodity Type Mapping
- [BR-052](../business-rules.md#br-052) — Devolved Office auto-set and inheritance

## Dependencies

- [US-002](US-002-Manage-ITAHC.md) (ITAHC lookup), [US-003](US-003-Manage-Importer-Notification.md) (Importer Notification lookup), [US-017](US-017-Manage-Place-of-Origin.md) (Place of Origin lookup)
- [US-011](US-011-Manage-Commodity-Risk-Levels.md) (Commodity Risk Level rules applied on create/update)
- [US-028](US-028-Generate-Unique-Reference-Number.md) (Unique reference number)
- [US-004](US-004-Manage-DOCOM.md) (DOCOM record — the DOCOM tab on the Import Record is additional triage context, not a replacement for the DOCOM entity)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md), [US-044](US-044-View-Importer-Notification.md) (Importer Notification — source of the "Create Import Record" button and Submission Date)
- [US-059](US-059-Devolved-Office-Assignment.md) (Devolved Office inheritance), [US-061](US-061-Automatically-Determine-Commodity-Type.md) (Commodity Type auto-set), [US-053](US-053-Create-Import-Record-at-Matching.md) (Import Record creation at matching time), [US-056](US-056-Compare-Commodity-Details.md) (Commodity Identifiers)

## Traceability

### Source Jira Issues

- IMTA-5870
- IMTA-6119
- IMTA-6121
- IMTA-7379
- IMTA-7466
- IMTA-7468
- IMTA-7469
- IMTA-8219
- IMTA-8589
- IMTA-8738
- IMTA-9134
- PLNT-4535
- PLNT-4536
- PLNT-4539
- PLNT-4540
- PLNT-4541
- EDA-235
- EDA-303
- EDA-304
- EDA-307
- EDA-308
- EDA-322
- EDA-337
- EDA-338
- EDA-353
- EDA-399
- EDA-400
- EDA-432
- EDA-620
- EDA-621
- EDA-642
- EDA-644
- EDA-680
- EDA-696
- EDA-705
- EDA-708
- EDA-725
- EDA-737
- EDA-794
- EDA-798
