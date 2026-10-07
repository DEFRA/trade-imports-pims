# US-004: Manage DOCOM

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update, view and search for DOCOM records,  
So that I can record information relating to a Document of Commercial Movement consignment.

## Description

A DOCOM (Document of Commercial Movement) is a health certificate used for commercial movements of certain animals. DOCOM records can be manually created by caseworkers or auto-created by the TRACES Classic integration ([US-008](US-008-Receive-DOCOM-From-TRACES.md)).

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can create or update a DOCOM record with the following fields:
  - Certificate Reference Number (Mandatory)
  - Local Reference Number
  - Receiving Category (Option Set)
  - Purpose (Option Set)
  - Seal Number
  - Container Number
  - APHA ABP Approval / Registration Number
  - Date of Decision
  - Devolved Office (auto-set from Place of Destination postcode, including Non-GB; manually editable — see [US-059](US-059-Devolved-Office-Assignment.md))

- **AC-2:** An EU Imports Caseworker can view a list of all DOCOM records ordered by creation date (newest first), showing: Certificate Reference Number, Local Reference Number, Receiving Category, Purpose, Seal Number, Container Number, Created On.

  *Note: no sort order is specified for this view in the available source Jira text. The order above (Created On, newest first) reflects the deployed PIMS sort order, used here as a fallback rather than a sourced requirement.*

- **AC-3:** An EU Imports Caseworker can perform a free text search for a DOCOM using: Certificate Reference Number, Local Reference Number, Receiving Category, Purpose, Seal Number, Container Number.

- **AC-4 (DOCOM Controls):** An EU Imports Caseworker can record the following dates under a "DOCOM Controls" heading on the Summary tab: Date Importer Contacted, Date Consignment Received, Date Control Added to TRACES.

- **AC-5 (Monthly reporting view):** An EU Imports Caseworker can select a view of all DOCOMs received in the previous calendar month (by TRACES Received Date), showing: Consignor Name, Consignor Address Country, Certificate Reference Number, Local Reference Number, Category, Consignee Name, Place of Origin Name, Place of Destination Name/Address City/Country/Approval Number, Transporter Name, Receiving Category, Quantity/Weight, Number of Packages, Container Number, Seal Number, Animal Certified As, Commodity Code, Commodity Type, Date of Decision.

## Business Rules

- [BR-052](../business-rules.md#br-052) — Devolved Office auto-set and inheritance

## Dependencies

- [US-008](US-008-Receive-DOCOM-From-TRACES.md) (Auto-receipt from TRACES Classic)
- [US-001](US-001-Manage-Import-Record.md) (Import Record may link to a DOCOM)
- [US-059](US-059-Devolved-Office-Assignment.md) (Devolved Office auto-set)

## Traceability

### Source Jira Issues

- IMTA-6252
- IMTA-7471
- IMTA-7472
- IMTA-7474
- IMTA-8103
