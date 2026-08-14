# US-046: Match Inbound Records to Import Records

## Summary

As an EU Imports Caseworker,  
I want PIMS to identify and present candidate Import Records for inbound certificates and notifications,  
So that I can review likely matches without repeating manual searches.

## Description

PIMS includes a matching capability centred on Match Record processing. When relevant inbound data is created or updated, matching logic searches for potentially related Import Records and presents them for review.

This story captures the implemented matching behaviour evidenced by the Match Record entity, matching workflow and supporting Match View. It also captures the Work Schedule Number column confirmed on the "8. Match View" saved query.

## Acceptance Criteria

- [x] **AC-1:** When relevant inbound certificate or notification data is created or updated, PIMS runs matching logic to identify candidate related Import Records.

- [x] **AC-2:** Candidate related Import Records are presented in a match-oriented view or equivalent review surface for caseworker assessment.

- [x] **AC-3:** The Match View includes Work Schedule Number where that value is available on the related Import Record.

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md) (Importer Notification receipt)

## Traceability

### Source Jira Issues

- IMTA-5872
- IMTA-6720

### Original Links

- IMTA-5872
- IMTA-6720
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Entities/defraimp_matchrecord/Entity.xml
- src/solutions/defra_Imports/src/Workflows/MatchRecord-FindRelatedImportRecords-8842B32A-5C26-4CD1-B710-04DFE5FCB73C.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                                                                                   |
| -------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| AC-1                 | ✅ Implemented | src/solutions/defra_Imports/src/Entities/defraimp_matchrecord/Entity.xml entity and workflow src/solutions/defra_Imports/src/Workflows/MatchRecord-FindRelatedImportRecords-8842B32A-5C26-4CD1-B710-04DFE5FCB73C.xaml run matching logic on inbound certificate/notification create/update |
| AC-2                 | ✅ Implemented | Match View surfaces candidate Import Records for caseworker review                                                                                                                                                                                                                         |
| AC-3                 | ✅ Implemented | Work Schedule Number column on Match View (from related Import Record)                                                                                                                                                                                                                     |
