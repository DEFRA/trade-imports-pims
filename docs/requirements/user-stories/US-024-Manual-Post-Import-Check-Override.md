# US-024: Manual Post Import Check Override

## Summary

As an EU Import Caseworker,  
I want to be able to manually skip or schedule a Post Import Check independently of what PIMS has determined,  
So that I can respond to import risks manually when the automated rules do not reflect the full picture.

## Description

Caseworkers can override PIMS's automated Post Import Check determination in both directions: skipping a system-required inspection (with a decline reason) or scheduling a check for a record not flagged by the system (with an explicit reason).

## Acceptance Criteria

- **AC-1 (Manually skip a system-required Post Import Check):**  
  An EU Imports Caseworker can activate a "Skip this Post Import Check" action on an Import Record that was flagged for a Post Import Check. This sets:
  - Post Import Checks Required? = **No**
  - Post Import Checks Declined Reason = **System Required Post Import Check Skipped**

- **AC-2 (Manually schedule a Post Import Check):**  
  An EU Imports Caseworker can activate a "Post Import Check this Record" action on an Import Record that was not flagged for a Post Import Check. This sets:
  - Post Import Checks Required? = **Yes**
  - Post Import Checks Required Reason = **Manually Requested Post Import Check**

## Business Rules

- [BR-021](../business-rules.md#br-021) — Manually scheduling an early Post Import Check resets the Gold counter ([US-022](US-022-Defer-Post-Import-Check-Counter.md))

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record)
- [US-022](US-022-Defer-Post-Import-Check-Counter.md) (Counter deferral for early manual inspection)

## Traceability

### Source Jira Issues

- IMTA-6699
