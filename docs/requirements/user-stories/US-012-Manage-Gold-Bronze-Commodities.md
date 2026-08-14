# US-012: Manage Gold/Bronze Commodities

## Summary

As an EU Imports Business Rules Admin,  
I want to be able to create and update Gold/Bronze Commodity rules,  
So that PIMS can determine which Import Records should be flagged for a Post Import Check based on the Trust Level of the Place of Origin.

## Description

Gold/Bronze Commodity rules define which Commodity Types (and optionally, which Countries) are subject to the Place of Origin trust level-based post-import check logic. These rules are maintained by EU Imports Business Rules Admins and are evaluated when an Import Record is created or updated.

## Acceptance Criteria

- [x] **AC-1:** An EU Imports Business Rules Admin can create or update a Gold/Bronze Commodity record with the following fields:
  - Name (Free text — Mandatory)
  - Commodity Type (Lookup — Mandatory)

- [x] **AC-2:** An EU Imports Business Rules Admin can link multiple Countries to a Gold/Bronze Commodity record via a "Countries This Rule Applies To" related list.

- [x] **AC-3:** Only users with the EU Imports Business Rules Admin security role can create or update Gold/Bronze Commodity records.

## Business Rules

- [BR-002](../business-rules.md#br-002) through [BR-006](../business-rules.md#br-006) — Gold/Bronze Commodity rules drive post-import check flagging logic

## Dependencies

- [US-014](US-014-Automated-Risk-Assessment-P1.md) (Post-import check flagging for P1 consignments consumes Gold/Bronze rules)
- Commodity Type and Country reference data (ASM-004, ASM-005)

## Traceability

### Source Jira Issues

- IMTA-5888

### Original Links

- IMTA-5888
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

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                |
| -------------------- | ------------- | --------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | [defraimp_goldbronzecommodity] entity with Name (mandatory), Commodity Type (mandatory) |
| AC-2                 | ✅ Implemented | Related list "Countries This Rule Applies To" via N:N relationship                      |
| AC-3                 | ✅ Implemented | Security role configuration enforces role-based CRUD                                    |
