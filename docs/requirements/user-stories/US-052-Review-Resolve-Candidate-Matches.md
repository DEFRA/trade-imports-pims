# US-052: Review and Resolve Candidate Matches

## Summary

As an EU Imports Caseworker,
I want to review candidate matches side-by-side and confirm, reject or reactivate them,
So that I can accurately link certificates to Importer Notifications before an Import Record is created.

## Description

Candidate matches produced by [US-051](US-051-Automated-Matching.md) are reviewed and resolved through a dedicated set of views, a side-by-side comparison screen and a guided business process. Match Records naming follows the "Matching Process" terminology; some source material referred to renamed system views and workflow labels without specifying the exact final wording, so the generic naming used in this story should be confirmed against the live system before implementation.

## Acceptance Criteria

- **AC-1 (Match Record views):** An EU Imports Caseworker can view Match Records filtered as Unmatched, Matched, Rejected or All, each showing Certificate Reference Number, Importer Notification Reference Number, Status Reason and Probability of Match, and searchable by ITAHC number, Importer Notification Reference Number or Destination Postcode. The Unmatched view only shows candidates at or above the confidence threshold in [US-051](US-051-Automated-Matching.md) AC-4.

- **AC-2 (Side-by-side comparison):** From a Match Record, an EU Imports Caseworker can view the candidate ITAHC/DOCOM and Importer Notification side-by-side, showing all fields populated on either record, with field order and naming aligned between the two sides.

- **AC-3 (Mark as a match):** An EU Imports Caseworker can mark a candidate pair as a match, setting the Match Record's status to Matched and associating it with the resulting Import Record (see [US-053](US-053-Create-Import-Record-at-Matching.md)).

- **AC-4 (Mark as not a match):** An EU Imports Caseworker can mark a candidate pair as not a match, setting the Match Record's status to Rejected; rejected pairs are excluded from future candidate searches ([US-051](US-051-Automated-Matching.md)).

- **AC-5 (Confirm Match business process):** Resolving a Match Record follows a guided business process:
  - A "Is this Record a Valid Match?" field defaults to Yes.
  - If Yes, the caseworker proceeds to a Complete step, confirming whether records were appended to the Import Record and optionally closing the Match Record as Completed (status Matched).
  - If No, the caseworker proceeds to a Reject step, which requires a mandatory Rejected Reason (status Rejected).
  - The caseworker can navigate back from either the Complete or Reject step to change the "Is this Record a Valid Match?" answer, which updates the Match Record's status and reason accordingly.

- **AC-6 (Unverified matches and reactivation):** An EU Imports Caseworker can view Importer Notifications that have never had a verified matched ITAHC, and can reactivate a previously-resolved Match Record to Unmatched status so it re-appears in the Unmatched view.

- **AC-7 (Match Rating colour coding):** On the side-by-side comparison screen (AC-2), any field in the Match Rating section scoring below 100% is highlighted in red.

- **AC-8 (Matching error log):** Errors occurring during the automated matching process ([US-051](US-051-Automated-Matching.md)) are recorded in an error log with Date and Time, Process, Error Code and Error Description. An EU Imports Caseworker can view the error log.

## Business Rules

- [BR-053](../business-rules.md#br-053) — Matching algorithm weighted mean calculation

## Dependencies

- [US-051](US-051-Automated-Matching.md) (produces the candidate Match Records this story resolves)
- [US-001](US-001-Manage-Import-Record.md)
- [US-056](US-056-Compare-Commodity-Details.md) (Commodity details shown as part of the side-by-side comparison)

## Traceability

### Source Jira Issues

- IMTA-7341
- IMTA-7368
- IMTA-7377
- IMTA-7386
- IMTA-7587
- IMTA-7588
- IMTA-7596
- IMTA-7649
- IMTA-7721
- IMTA-7975
