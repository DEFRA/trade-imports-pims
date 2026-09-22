# US-040: EU Imports — Border Control Metrics Dashboard

## Summary

As an EU Imports Caseworker,  
I want to be able to view the "EU Imports — Border Control Metrics" dashboard,  
So that I can view border control analytics and monitor 2% scanning check compliance.

## Description

A Dynamics 365 dashboard providing monthly import volumes, 2% P3 scanning check compliance metrics and Post Import Check scheduling views.

## Acceptance Criteria

- **AC-1 (Imports arriving in England this month):**  
  Chart showing count by month of Import Records where Date of Import = This Month.

- **AC-2 (2% P3 scanning checks required this month — excluding cats and dogs):**  
  Chart showing count by month of Import Records where:
  - Created On is within the last 1 month
  - Import Risk Level = P3
  - Commodity Type ≠ Cat or Dog
  - Post Import Checks Required? = Yes

- **AC-3 (2% P3 scanning Post Import Checks created this month — excluding cats and dogs):**  
  Chart showing count by month of Post Import Checks where:
  - Related Import Record Import Risk Level = P3
  - Related Import Record Commodity Type ≠ Cat or Dog
  - Related Import Record Created On is within the last 1 month
  - Related Import Record Post Import Checks Required? = Yes

- **AC-4 (Post Import Checks due today, this week, this month):**  
  Three list views showing Post Import Checks due: (a) today, (b) this week, (c) this month (next 30 days).

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-023](US-023-Post-Import-Check-Management.md) (Import Record and Post Import Check entities)

## Traceability

### Source Jira Issues

- IMTA-6344
