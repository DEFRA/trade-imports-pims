# US-011: Manage Commodity Risk Levels

## Summary

As a Caseworker Admin,  
I want to be able to create and update Import Country Commodity Risk Level rules,  
So that these rules can be applied to Import Records as they are created to determine their Risk Level.

## Description

Commodity Risk Level records define the mapping of Country + Commodity Type → Risk Level (P1, P2, P3). These rules are evaluated when an Import Record is created or updated to determine its base risk classification. The rules are managed by EU Imports Business Rules Admins.

## Acceptance Criteria

- [x] **AC-1:** An EU Imports Business Rules Admin can create or update an Import Country Commodity Risk Level record with the following mandatory fields:
  - Country (Lookup)
  - Commodity Type (Lookup)
  - Risk Level (Lookup: P1, P2, P3)

- [x] **AC-2:** All three fields (Country, Commodity Type, Risk Level) are mandatory on create and update.

- [x] **AC-3:** When an Import Record is created or updated, PIMS evaluates the Commodity Risk Level rules to set the Import Risk Level field on the Import Record.

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

### Original Links

- IMTA-5865
- IMTA-5914
- IMTA-5915
- IMTA-5916
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/ImportApplication-AutoRiskAssessmentandInspection-8C09794A-1388-4113-B508-46DDE8A56422.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                                    |
| -------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | [defraimp_ImportCountryCommodityRiskLevel] entity with Country (Lookup), Commodity Type (Lookup), Risk Level (Lookup P1/P2/P3) all mandatory                                                                                                |
| AC-2                 | ✅ Implemented | All three fields mandatory on create/update                                                                                                                                                                                                 |
| AC-3                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/ImportApplication-AutoRiskAssessmentandInspection-8C09794A-1388-4113-B508-46DDE8A56422.xaml evaluates Commodity Risk Level rules on Import Record create/update to set Import Risk Level |
