# US-011: Manage Commodity Risk Levels

## Summary

As a Caseworker Admin,  
I want to be able to create and update Import Country Commodity Risk Level rules,  
So that these rules can be applied to Import Records as they are created to determine their Risk Level.

## Description

Commodity Risk Level records define the mapping of Country + Commodity Type → Risk Level (P1, P2, P3). These rules are evaluated when an Import Record is created or updated to determine its base risk classification. The rules are managed by EU Imports Business Rules Admins.

## Acceptance Criteria

- **AC-1:** An EU Imports Business Rules Admin can create or update an Import Country Commodity Risk Level record with the following mandatory fields:
  - Country (Lookup)
  - Commodity Type (Lookup)
  - Risk Level (Lookup: P1, P2, P3)

- **AC-2:** All three fields (Country, Commodity Type, Risk Level) are mandatory on create and update.

- **AC-3:** When an Import Record is created or updated, PIMS evaluates the Commodity Risk Level rules to set the Import Risk Level field on the Import Record.

## Business Rules

- [BR-001](../business-rules.md#br-001) — Commodity Risk Level determines base Risk Level on Import Record

## Dependencies

- [US-014](US-014-Automated-Risk-Assessment-P1.md), [US-015](US-015-Automated-Risk-Assessment-P2.md), [US-016](US-016-Automated-Risk-Assessment-P3-Random.md) (Risk assessment rules consume commodity risk levels)
- [US-034](US-034-Manage-APHA-Region.md) (Country reference data from Exports solution — ASM-004)
- Commodity Type reference data from Exports solution (ASM-005)

## Traceability

### Source Jira Issues

- IMTA-5865
- IMTA-5914
- IMTA-5915
- IMTA-5916
