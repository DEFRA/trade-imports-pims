# US-003: Manage Import Notification

## Summary

As an EU Imports Caseworker,  
I want to be able to view and search for Import Notification records,  
So that I can use information relating to a pre-notification of an import consignment and link it to an Import Record.

## Description

"Import Notification" and "Importer Notification" are the same Dataverse entity (`defraimp_importernotification`) — "Import Notification" is legacy wording that persists in some labels, options and dashboards, not a separate record type. The entity is only ever created by the IPAFFS integration ([US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md)); an EU Imports Caseworker cannot create one manually. Caseworkers can only edit the Non-Compliance tab fields ([US-049](US-049-Non-Compliance-Management.md)) and the Health Certificate Attached field ([US-044](US-044-View-Importer-Notification.md) AC-4) — all other fields are populated from IPAFFS and are not intended to be manually edited.

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can view a list of all Import Notifications ordered by creation date (newest first), showing: Date of Import, Premises of Origin Country, Species / Product (Common Name), Reference Number, Importer Name, Importer Telephone, Importer Email, Port / Airport of Entry.

- **AC-2:** An EU Imports Caseworker can perform a free text search for an Import Notification by: Importer Name, Charity Name, Premises of Origin Name, Permanent Destination Name, Animal / Product ID. Results show the list view fields from AC-1.

## Business Rules

None specific to this entity.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record links to Import Notification via Primary Import Notification lookup)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md) (the only route by which an Import Notification record is created)
- [US-049](US-049-Non-Compliance-Management.md), [US-044](US-044-View-Importer-Notification.md) AC-4 (the only fields a caseworker can edit on this entity)

## Traceability

### Source Jira Issues

- IMTA-5869
