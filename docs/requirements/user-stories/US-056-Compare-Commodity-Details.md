# US-056: Compare Commodity Details Across Certificates and Import Records

## Summary

As an EU Imports Caseworker,
I want to view and compare commodity identifiers across the ITAHC, Importer Notification, Import Record and Match Record,
So that I can verify commodities align when matching or reviewing a case.

## Description

Commodity-level identifying detail (species, commodity code and type, and per-animal identifiers such as microchip, tattoo or passport number) is displayed consistently across the ITAHC, Importer Notification and Import Record, and compared side-by-side on the Match Record, supporting records with multiple commodity lines.

## Acceptance Criteria

- **AC-1 (Commodities tab on ITAHC):** An EU Imports Caseworker can view a Commodities tab on the ITAHC, positioned between ITAHC Details and Consignor.

- **AC-2 (ITAHC commodity fields):** The Commodities tab on the ITAHC shows, for each commodity line: Commodity Code, Commodity Type, Species Name, Commodity ID Type (e.g. Microchip, Tattoo, Passport — a commodity may have more than one), Commodity ID and Passport Number, sourced from the TRACES data.

- **AC-3 (Commodities tab on Importer Notification):** An EU Imports Caseworker can view an equivalent Commodities tab on the Importer Notification, positioned between Importer Notification Details and Person Responsible, showing the equivalent fields sourced from the IPAFFS data.

- **AC-4 (Commodity Identifiers on Import Record):** The Import Record Summary tab shows, for each commodity line, the Commodity ID Type and Commodity ID.

- **AC-5 (Commodities tab on Match Record):** An EU Imports Caseworker can view a Commodities tab on the Match Record, positioned between Match Details and Related Import Records, showing two side-by-side sections: "ITAHC Commodities" and "Importer Notification Commodities".

- **AC-6 (Comparable ordering):** Commodity lines are presented in the same order in both sections of the Match Record's Commodities tab, so corresponding lines can be compared directly.

## Business Rules

- None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-002](US-002-Manage-ITAHC.md), [US-003](US-003-Manage-Importer-Notification.md)
- [US-052](US-052-Review-Resolve-Candidate-Matches.md) (Commodities tab is part of the side-by-side match comparison)

## Traceability

### Source Jira Issues

- IMTA-7591
