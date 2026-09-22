# US-009: Auto-Create Import Record from ITAHC

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically create an associated Import Record when an ITAHC is received from TRACES Classic,  
So that I can save the effort of manually creating an Import Record and duplicating key ITAHC data.

## Description

When an ITAHC is auto-created in PIMS by the TRACES Classic integration ([US-007](US-007-Receive-ITAHC-From-TRACES.md)), PIMS should automatically create a linked Import Record with key ITAHC fields mapped across. The Import Record is only auto-created when the ITAHC was created by the integration service account, not when manually created by a caseworker.

**Note:** The specific field mapping from ITAHC to Import Record is not specified in this baseline. Attribute-level traceability remains incomplete until the agreed schema is available.

## Acceptance Criteria

- **AC-1:** When an ITAHC is created in PIMS by the TRACES integration service account, PIMS automatically creates a linked Import Record with key fields copied from the ITAHC. The specific field list is not specified in this baseline.

- **AC-2:** The auto-created Import Record has the ITAHC set as the Primary ITAHC.

- **AC-3:** The auto-creation does not trigger when an ITAHC is manually created by a business user.

## Business Rules

- [BR-008](../business-rules.md#br-008) — P3 random inspection counter incremented on Import Record creation
- [BR-022](../business-rules.md#br-022) — Unique reference number generated on Import Record creation

## Dependencies

- [US-007](US-007-Receive-ITAHC-From-TRACES.md) (ITAHC received from TRACES is prerequisite)
- [US-027](US-027-Auto-Assign-ITAHC-DOCOM-to-Region.md) (Auto-assigned Import Record to regional team)

## Traceability

### Source Jira Issues

- IMTA-6600
