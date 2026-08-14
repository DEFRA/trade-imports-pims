# US-017: Manage Place of Origin

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update and search for Places of Origin,  
So that I can later link an Import Record to a pre-defined Place of Origin and have PIMS automatically determine the post-import check requirement.

## Description

The Place of Origin entity stores registered farms and origin premises. Caseworkers can create and manage Place of Origin records and search for them when linking to an Import Record. A Place of Origin record maintains a Trust Level (Gold or Bronze) and related counters that drive the inspection logic ([US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md)).

## Acceptance Criteria

- [x] **AC-1:** An EU Imports Caseworker can create or update a Place of Origin record with the following fields:
  - Organisation Name (Free text — Mandatory)
  - Address: Line 1, Line 2, Line 3, City, County, Postcode (all optional)
  - Country (Lookup to Country reference data — optional)

- [x] **AC-2:** An EU Imports Caseworker can search for a Place of Origin from within the EU Imports application by Organisation Name or Postcode.

- [x] **AC-3:** A caseworker can search for a pre-defined Place of Origin from within an Import Record (by Organisation Name or Postcode) and link the matched Place of Origin to the Import Record.

- [x] **AC-4:** When a Place of Origin with a populated Country field is selected on an Import Record, the Country of Origin field on the Import Record is updated automatically.

## Business Rules

- [BR-010](../business-rules.md#br-010) — New Place of Origin defaults to Bronze Trust Level
- [BR-018](../business-rules.md#br-018) — Number of Import Records counter maintained on Place of Origin

## Dependencies

- [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md) (Trust Level maintenance on Place of Origin)
- [US-014](US-014-Automated-Risk-Assessment-P1.md) (Import Record risk assessment uses Place of Origin Trust Level)

## Traceability

### Source Jira Issues

- IMTA-5885
- IMTA-5887

### Original Links

- IMTA-5885
- IMTA-5887
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Entities/defraimp_placeoforigin/Entity.xml
- src/solutions/defra_Imports/src/Workflows/LockCountryofOriginwhenPlaceofOriginisPopulated-9F4ACF99-BBF1-E911-A812-000D3AB5D511.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                |
| -------------------- | ------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | src/solutions/defra_Imports/src/Entities/defraimp_placeoforigin/Entity.xml entity with Organisation Name (mandatory), Address fields (all optional), Country (optional Lookup)                                          |
| AC-2                 | ✅ Implemented | Search by Organisation Name or Postcode available in UI                                                                                                                                                                 |
| AC-3                 | ✅ Implemented | Place of Origin lookup on Import Record with search by Organisation Name/Postcode                                                                                                                                       |
| AC-4                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/LockCountryofOriginwhenPlaceofOriginisPopulated-9F4ACF99-BBF1-E911-A812-000D3AB5D511.xaml auto-populates Country of Origin when Place of Origin Country is populated |
