# US-040: EU Imports — Border Control Metrics Dashboard

## Summary

As an EU Imports Caseworker,  
I want to be able to view the "EU Imports — Border Control Metrics" dashboard,  
So that I can view border control analytics and monitor 2% scanning check compliance.

## Description

A Dynamics 365 dashboard providing monthly import volumes, 2% P3 scanning check compliance metrics and Post Import Check scheduling views.

## Acceptance Criteria

- [x] **AC-1 (Imports arriving in England this month):**  
  Chart showing count by month of Import Records where Date of Import = This Month.

- [x] **AC-2 (2% P3 scanning checks required this month — excluding cats and dogs):**  
  Chart showing count by month of Import Records where:
  - Created On is within the last 1 month
  - Import Risk Level = P3
  - Commodity Type ≠ Cat or Dog
  - Post Import Checks Required? = Yes

- [x] **AC-3 (2% P3 scanning Post Import Checks created this month — excluding cats and dogs):**  
  Chart showing count by month of Post Import Checks where:
  - Related Import Record Import Risk Level = P3
  - Related Import Record Commodity Type ≠ Cat or Dog
  - Related Import Record Created On is within the last 1 month
  - Related Import Record Post Import Checks Required? = Yes

- [x] **AC-4 (Post Import Checks due today, this week, this month):**  
  Three list views showing Post Import Checks due: (a) today, (b) this week, (c) this month (next 30 days).

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-023](US-023-Post-Import-Check-Management.md) (Import Record and Post Import Check entities)

## Traceability

### Source Jira Issues

- IMTA-6344

### Original Links

- IMTA-6344
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

| Acceptance Criterion | Status        | Evidence                                                                                                                                            |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Dashboard chart: imports arriving in England this month (count by month of Date of Import = This Month)                                             |
| AC-2                 | ✅ Implemented | Dashboard chart: 2% P3 scanning checks required this month (P3 + Commodity ≠ Cat/Dog + Post Import Checks Required? = Yes, Created On = This Month) |
| AC-3                 | ✅ Implemented | Dashboard chart: 2% P3 scanning Post Import Checks created this month                                                                               |
| AC-4                 | ✅ Implemented | Dashboard list views: Post Import Checks due today, this week, this month                                                                           |
