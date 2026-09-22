# US-017: Manage Place of Origin

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update and search for Places of Origin,  
So that I can later link an Import Record to a pre-defined Place of Origin and have PIMS automatically determine the post-import check requirement.

## Description

The Place of Origin entity stores registered farms and origin premises. Caseworkers can create and manage Place of Origin records and search for them when linking to an Import Record. A Place of Origin record maintains a Trust Level (Gold or Bronze) and related counters that drive the inspection logic ([US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md)).

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can create or update a Place of Origin record with the following fields:
  - Organisation Name (Free text — Mandatory)
  - Address: Line 1, Line 2, Line 3, City, County, Postcode (all optional)
  - Country (Lookup to Country reference data — optional)

- **AC-2:** An EU Imports Caseworker can search for a Place of Origin from within the EU Imports application by Organisation Name or Postcode.

- **AC-3:** A caseworker can search for a pre-defined Place of Origin from within an Import Record (by Organisation Name or Postcode) and link the matched Place of Origin to the Import Record.

- **AC-4:** When a Place of Origin with a populated Country field is selected on an Import Record, the Country of Origin field on the Import Record is updated automatically.

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
