# US-046: Match Inbound Records to Import Records

## Summary

As an EU Imports Caseworker,  
I want PIMS to identify and present candidate Import Records for inbound certificates and notifications,  
So that I can review likely matches without repeating manual searches.

## Description

PIMS includes a matching capability centred on Match Record processing. When relevant inbound data is created or updated, matching logic searches for potentially related Import Records and presents them for review.

This story captures the matching behaviour supported by Match Record processing and the Match View. It also captures the Work Schedule Number column confirmed on the "8. Match View" saved query.

## Acceptance Criteria

- **AC-1:** When relevant inbound certificate or notification data is created or updated, PIMS runs matching logic to identify candidate related Import Records.

- **AC-2:** Candidate related Import Records are presented in a match-oriented view or equivalent review surface for caseworker assessment.

- **AC-3:** The Match View includes Work Schedule Number where that value is available on the related Import Record.

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md) (Importer Notification receipt)

## Traceability

### Source Jira Issues

- IMTA-5872
- IMTA-6720
