# US-042: Farming Analysis and Evidence Team Weekly Report

## Summary

As a Data Team Member,  
I want to be able to view and export the Farming Analysis and Evidence Team (FAET) Weekly Report,  
So that information can be shared with other departments.

## Description

A report exportable by users with the Excel Export security role, covering Import Records for a given date range with the specified field set.

**Note (traceability):** The sole source ticket for this story, IMTA-6354, has been removed from the Jira export following an export correction. This story is retained based on implementation evidence — an "EU Imports Export to Excel" security role granting `prvExportToExcel` exists, and the Import Record "6. Export to Excel View" saved query exposes almost the entire field set below.

## Acceptance Criteria

- **AC-1 (FAET Weekly Report):**  
  A user with the Excel Export security role can run a report for a given date range that outputs Import Records with the following fields:

  - Devolved Office
  - Importer Name
  - Importer Address
  - Importer Postcode
  - Importer CPH
  - Country of Origin (EU)
  - Country of Origin (Non-EU)
  - Transiting Countries
  - Date of Import
  - Destination Name
  - Destination Address
  - Destination Postcode
  - Consignment Final (Permanent) Name
  - Consignment Final (Permanent) Address
  - Consignment Final (Permanent) Postcode
  - Premises of Origin Name
  - Premises of Origin Address
  - Premises of Origin Postcode
  - Transporter Name
  - Commodity
  - Commodity Notes
  - Quantity
  - Quantity (Units)
  - Purpose
  - Port of Entry
  - Place of Origin History
  - Local Veterinary Unit
  - LVU Number

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record data source)
- Excel Export security role must be defined and assigned to appropriate users

## Traceability

### Source Jira Issues

- _None — IMTA-6354 removed from the source export; see traceability note above._
