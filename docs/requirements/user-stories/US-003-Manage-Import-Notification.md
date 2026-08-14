# US-003: Manage Import Notification

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update, view and search for Import Notification records,  
So that I can record information relating to a pre-notification of an import consignment and link it to an Import Record.

## Description

An Import Notification is the legacy manual notification concept used in source stories for pre-notifications submitted by importers (for example IV66 forms). It is distinct from the Importer Notification entity ([US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md), [US-044](US-044-View-Importer-Notification.md)), which is received automatically from IPAFFS.

Implementation evidence currently shows legacy Import Notification wording persisting in selected labels, options and dashboards, while the dedicated notification entity present in solution metadata is Importer Notification.

## Acceptance Criteria

- [~] **AC-1:** An EU Imports Caseworker can create or update an Import Notification record with the following fields (all optional):
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

- [~] **AC-2:** An EU Imports Caseworker can view a list of all Import Notifications ordered by creation date (newest first), showing: Date of Import, Premises of Origin Country, Species / Product (Common Name), Reference Number, Importer Name, Importer Telephone, Importer Email, Port / Airport of Entry.

- [~] **AC-3:** An EU Imports Caseworker can perform a free text search for an Import Notification by: Importer Name, Charity Name, Premises of Origin Name, Permanent Destination Name, Animal / Product ID. Results show the list view fields from AC-2.

## Business Rules

None specific to this entity.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record links to Import Notification via Primary Import Notification lookup)

## Traceability

### Source Jira Issues

- IMTA-5869

### Original Links

- IMTA-5869
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- None evidenced in this review.

## Implementation Confidence

Medium

## Conformance Snapshot (2026-07-22)

- Status: ⚠️ Partially Implemented
- Conflicts/Gaps: Terminology — "Import Notification" and "Importer Notification" are both used in the source records. The implementation uses Importer Notification as the primary entity; see the [Glossary](../glossary.md).

## Acceptance Criteria Conformance

| Acceptance Criterion | Status                  | Evidence                                                                                                                                                   |
| -------------------- | ----------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ⚠️ Partially Implemented | Entity definition exists with legacy "Import Notification" naming, but primary implementation is "Importer Notification" (`defraimp_ImporterNotification`) |
| AC-2                 | ⚠️ Partially Implemented | Views for Importer Notification exist but legacy "Import Notification" wording remains in some option labels                                               |
| AC-3                 | ⚠️ Partially Implemented | Search capability on Importer fields exists                                                                                                                |
