# US-061: Automatically Determine Commodity Type

## Summary

As an EU Imports Caseworker,
I want the Commodity Type on an Import Record to be set automatically from the certificate or notification's species and commodity code,
So that I don't have to set it manually and risk assessment has a reliable classification to work from.

## Description

A Commodity Type Mapping reference entity maps a TRACES or IPAFFS Species ID and Commodity Code combination to a Commodity Type. When an ITAHC or Importer Notification is linked to an Import Record, the relevant Species ID and Commodity Code fields are copied onto the Import Record (future-proofing against loss of either source system), and PIMS looks up the Commodity Type Mapping to set the Import Record's Commodity Type automatically. This feeds [US-011](US-011-Manage-Commodity-Risk-Levels.md) and [US-012](US-012-Manage-Gold-Bronze-Commodities.md), which classify Import Records by Commodity Type.

## Acceptance Criteria

- **AC-1 (Commodity Type Mapping entity):** An EU Imports Business Rules Admin can create and update Commodity Type Mapping records with: Commodity Type (lookup), TRACES Species ID, TRACES Commodity Code, IPAFFS Species ID, IPAFFS Commodity Code.

- **AC-2 (Source fields copied onto Import Record):** When an ITAHC or Importer Notification is linked to (or unlinked from) an Import Record, PIMS copies (or clears) the relevant Species ID and Commodity Code fields from the source record onto the Import Record.

- **AC-3 (Automatic Commodity Type determination):** Whenever the Import Record's Species ID or Commodity Code fields change, PIMS looks up the Commodity Type Mapping records matching the Commodity Code and, where more than one matches, narrows by Species ID. If exactly one mapping matches, Commodity Type is set to the mapped value. If no mapping matches, Commodity Type is set to "Other".

- **AC-4 (Field locked):** The Commodity Type field on the Import Record is not manually editable; it can only be set by the automatic determination in AC-3.

## Business Rules

- [BR-051](../business-rules.md#br-051) — Commodity Type auto-set from Commodity Type Mapping

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-002](US-002-Manage-ITAHC.md), [US-003](US-003-Manage-Importer-Notification.md)
- [US-011](US-011-Manage-Commodity-Risk-Levels.md), [US-012](US-012-Manage-Gold-Bronze-Commodities.md) (consume Commodity Type)

## Traceability

### Source Jira Issues

- IMTA-7785
- IMTA-8483
