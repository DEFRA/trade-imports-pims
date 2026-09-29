# US-010: Auto-Create Import Record from DOCOM

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically create an associated Import Record when a DOCOM is received from TRACES Classic,  
So that I can save the effort of manually creating an Import Record and duplicating key DOCOM data.

## Description

Mirrors [US-009](US-009-Auto-Create-Import-Record-From-ITAHC.md) for DOCOM records. When a DOCOM is auto-created in PIMS by the TRACES Classic integration ([US-008](US-008-Receive-DOCOM-From-TRACES.md)), PIMS automatically creates a linked Import Record with key fields mapped from the DOCOM. The auto-creation only occurs when the DOCOM was created by the integration service account.

**Note:** The specific field mapping is not specified in this baseline. Attribute-level traceability remains incomplete until the agreed schema is available.

## Acceptance Criteria

- **AC-1:** When a DOCOM is created in PIMS by the TRACES integration service account, PIMS automatically creates a linked Import Record with key fields copied from the DOCOM. The specific field list is not specified in this baseline.

- **AC-2:** The auto-creation does not trigger when a DOCOM is manually created by a business user.

## Business Rules

- [BR-022](../business-rules.md#br-022) — Unique reference number generated on Import Record creation

## Dependencies

- [US-008](US-008-Receive-DOCOM-From-TRACES.md) (DOCOM received from TRACES is prerequisite)
- [US-027](US-027-Auto-Assign-ITAHC-DOCOM-to-Region.md) (Auto-assigned Import Record to regional team)

## Traceability

### Source Jira Issues

- IMTA-6601
