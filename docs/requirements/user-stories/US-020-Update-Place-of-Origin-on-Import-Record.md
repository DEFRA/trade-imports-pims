# US-020: Update Place of Origin on Import Record

## Summary

As an EU Imports Caseworker,  
I want to be able to change the Verified Place of Origin on an Import Record,  
So that I can get the correct Post Import Check requirement based on the new Place of Origin.

## Description

When the Place of Origin on an Import Record is changed after initial assignment, PIMS must adjust counters on the previous Place of Origin, re-evaluate the post-import check requirement based on the new Place of Origin, and manage any inspection quota obligations arising from the change.

## Acceptance Criteria

- **AC-1 (Re-evaluate Post Import Check requirement):**  
  When a user changes the Verified Place of Origin and saves the Import Record, if the Risk Level is P1 and the record is subject to Gold/Bronze rules, PIMS re-runs the Post Import Check determination logic and updates Post Import Checks Required? and Post Import Checks Required Reason.

- **AC-2 (Quota adjustment for previous Gold Place of Origin):**  
  If the previous Place of Origin was Gold AND the Import Record was flagged for a Gold inspection:
  - Add 1 to the previous Place of Origin's Inspection Quota counter.

- **AC-3 (Counter adjustment on previous Place of Origin):**  
  Decrement the Number of Import Records counter on the previous Place of Origin by 1.

## Business Rules

- [BR-020](../business-rules.md#br-020) — Changing Place of Origin updates previous Place of Origin counters

## Dependencies

- [US-017](US-017-Manage-Place-of-Origin.md) (Place of Origin entity)
- [US-014](US-014-Automated-Risk-Assessment-P1.md) (Risk assessment re-evaluation)

## Traceability

### Source Jira Issues

- IMTA-6666
