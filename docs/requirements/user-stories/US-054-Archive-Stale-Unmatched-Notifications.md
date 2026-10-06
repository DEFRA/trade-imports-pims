# US-054: Archive Stale Unmatched Importer Notifications

## Summary

As an EU Imports Caseworker,
I want unmatched Importer Notifications older than a configurable threshold to be automatically archived,
So that active matching views only show notifications that still need attention.

## Description

Importer Notifications that have not been confirmed as matched to an ITAHC within a configurable period are automatically archived, keeping the active caseload manageable while remaining retrievable.

## Acceptance Criteria

- **AC-1 (Archiving rule):** An Importer Notification that has not been confirmed as a match to an ITAHC and was created more than 30 days ago (configurable) is set to Inactive.

- **AC-2 (Archived view):** An EU Imports Caseworker can select a view showing archived (Inactive, unmatched) Importer Notifications.

## Business Rules

- None additional; the 30-day threshold is defined as [NFR-SCA-003](../non-functional-requirements.md).

## Dependencies

- [US-051](US-051-Automated-Matching.md), [US-052](US-052-Review-Resolve-Candidate-Matches.md)

## Traceability

### Source Jira Issues

- IMTA-7589
