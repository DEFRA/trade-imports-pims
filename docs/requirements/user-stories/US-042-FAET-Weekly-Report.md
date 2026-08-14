# US-042: Farming Analysis and Evidence Team Weekly Report

## Summary

As a Data Team Member,  
I want to be able to view and export the Farming Analysis and Evidence Team (FAET) Weekly Report,  
So that information can be shared with other departments.

## Description

A report exportable by users with the Excel Export security role, covering Import Records for a given date range with the specified field set.

## Acceptance Criteria

- [x] **AC-1 (FAET Weekly Report):**  
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

- IMTA-6354

### Original Links

- IMTA-6354
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

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified (user role "Excel Export" must be defined)

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                                                          |
| -------------------- | ------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Excel report available for date range with 24 specified fields (Devolved Office, Importer Name/Address/Postcode/CPH, Countries, Date of Import, Destination, Premises of Origin, Transporter, Commodity, Purpose, Port, Place of Origin History, LVU, LVU Number) |
