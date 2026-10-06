# US-051: Automated Matching of Certificates to Importer Notifications

## Summary

As an EU Imports Caseworker,
I want PIMS to automatically identify candidate matches between ITAHCs/DOCOMs and Importer Notifications,
So that I don't have to manually search for likely matches.

## Description

This story elaborates the high-level matching capability summarised in [US-046](US-046-Match-Inbound-Records-to-Import-Records.md) with the concrete matching algorithm. PIMS periodically searches unmatched ITAHCs/DOCOMs and Importer Notifications created within a recent, configurable window and produces a weighted percentage match score for each candidate pair, creating a Match Record for review ([US-052](US-052-Review-Resolve-Candidate-Matches.md)) wherever the score meets a configurable confidence threshold.

## Acceptance Criteria

- **AC-1 (Search for a matching Importer Notification for an unmatched ITAHC):** PIMS periodically (default every 30 minutes, configurable) searches Importer Notifications created within a configurable window (default 14 days) for candidates matching an unmatched ITAHC created within the same window, excluding any pairing previously marked "No Match".

- **AC-2 (Search for a matching ITAHC for an unmatched Importer Notification):** PIMS performs the equivalent search in the other direction for an unmatched Importer Notification.

- **AC-3 (Match scoring):** For each candidate pair, PIMS calculates a percentage match:
  - If the ITAHC/DOCOM certificate number matches the Importer Notification exactly, the pair scores 100%.
  - Otherwise, if the first part of the Destination Postcode matches, PIMS calculates a weighted mean score across the configured fields (per [US-050](US-050-Configure-Matching-Algorithm.md)), which may include: second part of postcode, Destination Name, Commodity Code, Quantity, Country of Origin, Place of Origin Name and Place of Origin Postcode.
  - Otherwise, the pair is not a candidate match.

- **AC-4 (Candidate Match Record created above threshold):** Where the calculated score meets or exceeds a configurable confidence threshold (default 70%), PIMS creates a Match Record linking the candidate pair with a status of Unmatched and the calculated probability of match, for review under [US-052](US-052-Review-Resolve-Candidate-Matches.md). If a Match Record already exists for the same candidate pair (from a previous run), PIMS updates its calculated probability of match rather than creating a duplicate.

## Business Rules

- [BR-053](../business-rules.md#br-053) — Matching algorithm weighted mean calculation

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-002](US-002-Manage-ITAHC.md), [US-003](US-003-Manage-Importer-Notification.md), [US-004](US-004-Manage-DOCOM.md)
- [US-050](US-050-Configure-Matching-Algorithm.md) (field/weighting configuration)
- [US-046](US-046-Match-Inbound-Records-to-Import-Records.md) (summary-level matching story this elaborates)

## Traceability

### Source Jira Issues

- IMTA-7394
