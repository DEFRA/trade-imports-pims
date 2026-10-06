# US-003: Manage Importer Notification

## Summary

As an EU Imports Caseworker,  
I want to be able to view and search for Importer Notification records,  
So that I can use information relating to a pre-notification of an import consignment and link it to an Import Record.

## Description

The Importer Notification entity (`defraimp_importernotification`) is the IPAFFS-originated notification record used in PIMS. It is only ever created by the IPAFFS integration ([US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md)); an EU Imports Caseworker cannot create one manually. Caseworkers can only edit the Non-Compliance tab fields ([US-049](US-049-Non-Compliance-Management.md)) and the Health Certificate Attached field ([US-044](US-044-View-Importer-Notification.md) AC-4) — all other fields are populated from IPAFFS and are not intended to be manually edited.

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can view an Importer Notification record with the following fields (all optional):
  - Importer Name, Address, Postcode, Telephone, Email
  - CPH Number
  - Charity Name, Address, Postcode, Telephone, Email
  - Consignment Country of Origin, Countries of Transit
  - Date of Import
  - Place of Destination (Contact Name, Address, Postcode, Telephone, Email)
  - Permanent Destination (Contact Name, Address, Postcode, Telephone, Email)
  - Premises of Origin (Name, Address, Postcode, Country)
  - Transporter (Name, Address, Postcode, Telephone, Email)
  - Species / Product (Common Name), Quantity, Units
  - Intended Use of Commodity
  - Port / Airport of Entry
  - Animal / Product IDs

- **AC-2:** An EU Imports Caseworker can view a list of all Importer Notification records ordered by creation date (newest first), showing: Date of Import, Premises of Origin Country, Species / Product (Common Name), Reference Number, Importer Name, Importer Telephone, Importer Email, Port / Airport of Entry.

- **AC-3:** An EU Imports Caseworker can perform a free text search for an Importer Notification by: Importer Name, Charity Name, Premises of Origin Name, Permanent Destination Name, Animal / Product ID. Results show the list view fields from AC-2.

## Business Rules

None specific to this entity.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record links to Importer Notification via Primary Importer Notification lookup)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md) (the only route by which an Importer Notification record is created)
- [US-049](US-049-Non-Compliance-Management.md), [US-044](US-044-View-Importer-Notification.md) AC-4 (the only fields a caseworker can edit on this entity)

## Traceability

### Source Jira Issues

- IMTA-5869
