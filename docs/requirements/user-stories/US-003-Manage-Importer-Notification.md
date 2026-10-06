# US-003: Manage Importer Notification

## Summary

As an EU Imports Caseworker,  
I want to be able to view and search for Importer Notification records,  
So that I can use information relating to a pre-notification of an import consignment and link it to an Import Record.

## Description

The Importer Notification entity (`defraimp_importernotification`) is the IPAFFS-originated notification record used in PIMS. It is only ever created by the IPAFFS integration ([US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md)); an EU Imports Caseworker cannot create one manually. Caseworkers can only edit the Non-Compliance tab fields ([US-049](US-049-Non-Compliance-Management.md)), the Health Certificate Attached field ([US-044](US-044-View-Importer-Notification.md) AC-4) and the Devolved Office field ([US-059](US-059-Devolved-Office-Assignment.md) AC-1) — all other fields are populated from IPAFFS and are not intended to be manually edited. The EU Imports Caseworker role holds org-level WRITE privilege on this entity (NFR-AUT-004) to support this; a caseworker can also attach supporting documents (including non-IPAFFS material such as emails extracted to a supported file format) to an Importer Notification using the same mechanism as [US-029](US-029-Document-Attachment.md).

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can view an Importer Notification record with the following fields (all optional):
  - Importer Name, Address, Postcode, Telephone, Email
  - CPH Number
  - Charity Name, Address, Postcode, Telephone, Email
  - Devolved Office (manual, no default — see [US-059](US-059-Devolved-Office-Assignment.md))
  - Consignment Country of Origin, Countries of Transit
  - Date of Import
  - Place of Destination (Contact Name, Address, Postcode, Telephone, Email)
  - Permanent Destination (Contact Name, Address, Postcode, Telephone, Email)
  - Premises of Origin (Name, Address, Postcode, Country)
  - Transporter (Name, Address, Postcode, Telephone, Email)
  - Transport to PoE / Transport after PoE sections (mirrors the Import Record — see [US-001](US-001-Manage-Import-Record.md))
  - Species / Product (Common Name), Quantity, Units, Weight (KG) (hidden for CHEDA — [BR-046](../business-rules.md#br-046))
  - Intended Use of Commodity
  - Purpose of Consignment, Internal Market Purpose, Certified For, Purpose for Movement, Number of Packages (journey-conditional visibility — [BR-045](../business-rules.md#br-045))
  - Port / Airport of Entry
  - Animal / Product IDs, Commodity Code, Horse Name (shown only where the commodity is Horse), Commodity Permanent Address Information (CHEDA only)
  - MRN Number (sourced from IPAFFS)
  - Cloned (Yes/No — identifies a notification created via IPAFFS Clone Journey)
  - Imp Type (Live Animals / POAO / HFRNAO; hidden for the CHED journey)
  - Inspection Required (Required / Not Required, sourced from IPAFFS risk decision)

- **AC-2:** An EU Imports Caseworker can view a list of all Importer Notification records ordered by creation date (newest first), showing: Date of Import, Premises of Origin Country, Species / Product (Common Name), Reference Number, Importer Name, Importer Telephone, Importer Email, Port / Airport of Entry, Imp Type.

- **AC-3:** An EU Imports Caseworker can perform a free text search for an Importer Notification by: Importer Name, Charity Name, Premises of Origin Name, Permanent Destination Name, Animal / Product ID. Results show the list view fields from AC-2.

- **AC-4 (Process Status removed):** The Process Status field is not shown on the Importer Notification form; it is unused and holds no historical data.

- **AC-5 (All POAO/HRFNAO view):** An EU Imports Caseworker can select an "All POAO/HRFNAO Importer Notifications" view, equivalent to the Active Importer Notifications view with Status (Active/Inactive), Imp Type, Owner, Commodity Description and Commodity Code columns added.

- **AC-6 (Multiple commodities flagged):** An Importer Notification received with more than one commodity code is flagged for caseworker review (see [US-055](US-055-Flag-Multiple-Commodity-Certificates.md)).

## Business Rules

- [BR-045](../business-rules.md#br-045) — Commodity field visibility depends on journey type
- [BR-046](../business-rules.md#br-046) — Weight (KG) hidden for CHEDA
- [BR-052](../business-rules.md#br-052) — Devolved Office auto-set and inheritance
- [BR-055](../business-rules.md#br-055) — Owner-change workflow excludes in-flight IPAFFS statuses
- [BR-056](../business-rules.md#br-056) — Latest Health Certificate document type mirrors Health Certificate behaviour

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record links to Importer Notification via Primary Importer Notification lookup)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md) (the only route by which an Importer Notification record is created)
- [US-049](US-049-Non-Compliance-Management.md), [US-044](US-044-View-Importer-Notification.md) AC-4 (the only fields a caseworker can edit on this entity)
- [US-059](US-059-Devolved-Office-Assignment.md), [US-029](US-029-Document-Attachment.md) (document types, Latest Health Certificate), [US-055](US-055-Flag-Multiple-Commodity-Certificates.md), [US-056](US-056-Compare-Commodity-Details.md)

## Traceability

### Source Jira Issues

- IMTA-5869
- IMTA-9144
- EDA-236
- EDA-249
- EDA-621
- EDA-642
- EDA-643
- EDA-644
- EDA-649
- EDA-680
- EDA-696
- EDA-705
- EDA-708
- EDA-725
- EDA-739
- EDA-794
- EDA-798
