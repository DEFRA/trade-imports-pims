# US-002: Manage ITAHC

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update, view and search for ITAHC records,  
So that I can record key health certificate information and link it to an Import Record for risk assessment.

## Description

An ITAHC (International Transport of Animals Health Certificate) is the primary health certificate for live animal consignments. Caseworkers create and manage ITAHC records in PIMS. ITAHC records can also be auto-created by the TRACES Classic integration ([US-007](US-007-Receive-ITAHC-From-TRACES.md)).

An ITAHC has a replacement chain (Replaced By / Replaces) mirroring the TRACES chain, and replacement links and cross-references are maintained. Explicit prevention of primary-certificate selection in every selection context remains to be confirmed.

## Acceptance Criteria

- **AC-1:** An EU Imports Caseworker can create or update an ITAHC record. The D365 ITAHC form ("ITAHC Details" tab) is organised into the following sections with confirmed fields:

  **General**
  - Reference Number* (mandatory — the ITAHC local reference number, e.g. INTRA.PL.2019.0008620 - V1; maps to field I.2.a on the physical certificate)
  - Health Certificate Number (the formal I.2 certificate reference number)
  - Certified For (Animal Certified As — option set from TRACES)
  - TRACES Notification Received Date
  - Official Veterinarian or Official Inspector (OV Name)
  - Local Veterinary Unit (LVU No.)
  - Replaced By (Lookup to another ITAHC)
  - Replaces (Lookup to another ITAHC)

  **Consignment Details**
  - Country of Origin (Lookup)
  - Commodity Type
  - Animal Species / Product (Commodity Complements Text)
  - Quantity / Weight
  - Unit
  - Number of Packages
  - Date / Time of Departure
  - Estimated Journey Time (Days)
  - Estimated Journey Time (Hours)
  - Identification of Animals (JSON text from TRACES)

  **Place of Destination**
  - Place of Destination Type
  - Approval Number
  - Address (Line 1, Line 2, Line 3, City, County, Country, Postcode)

- **AC-2:** An EU Imports Caseworker can view a list of all ITAHC records ("Active ITAHCs" view) sorted by Created On (newest first). The list view columns are **Local Reference Number** and **Created On**. Full record details are accessible by opening an individual record.

- **AC-3:** An EU Imports Caseworker can perform a free text search for an ITAHC by Local Reference or Certificate Reference Number.

- **AC-4:** The replacement chain (Replaced By / Replaces) is maintained on ITAHC records and replaced certificates are identifiable to caseworkers during Import Record processing.

## Business Rules

- [BR-027](../business-rules.md#br-027) — ITAHC replacement chain must be tracked; full primary-selection prevention remains subject to confirmation

## Dependencies

- [US-007](US-007-Receive-ITAHC-From-TRACES.md) (Auto-receipt from TRACES Classic)
- [US-001](US-001-Manage-Import-Record.md) (Import Record links to ITAHC via Primary ITAHC lookup)

## Traceability

### Source Jira Issues

- IMTA-5868
- IMTA-5984
