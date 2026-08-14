# US-010: Auto-Create Import Record from DOCOM

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically create an associated Import Record when a DOCOM is received from TRACES Classic,  
So that I can save the effort of manually creating an Import Record and duplicating key DOCOM data.

## Description

Mirrors [US-009](US-009-Auto-Create-Import-Record-From-ITAHC.md) for DOCOM records. When a DOCOM is auto-created in PIMS by the TRACES Classic integration ([US-008](US-008-Receive-DOCOM-From-TRACES.md)), PIMS automatically creates a linked Import Record with key fields mapped from the DOCOM. The auto-creation only occurs when the DOCOM was created by the integration service account.

**Note:** The specific field mapping is not specified in this baseline. Attribute-level traceability remains incomplete until the agreed schema is available.

## Acceptance Criteria

- [x] **AC-1:** When a DOCOM is created in PIMS by the TRACES integration service account, PIMS automatically creates a linked Import Record with key fields copied from the DOCOM. The specific field list is not specified in this baseline.

- [x] **AC-2:** The auto-creation does not trigger when a DOCOM is manually created by a business user.

## Business Rules

- [BR-022](../business-rules.md#br-022) — Unique reference number generated on Import Record creation

## Dependencies

- [US-008](US-008-Receive-DOCOM-From-TRACES.md) (DOCOM received from TRACES is prerequisite)
- [US-027](US-027-Auto-Assign-ITAHC-DOCOM-to-Region.md) (Auto-assigned Import Record to regional team)

## Traceability

### Source Jira Issues

- IMTA-6601

### Original Links

- IMTA-6601
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
- Conflicts/Gaps: Field mapping schema is not specified in this baseline.

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                            |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Workflow mirrors [US-009](US-009-Auto-Create-Import-Record-From-ITAHC.md) pattern for DOCOM records |
| AC-2                 | ✅ Implemented | Manual DOCOM creates do not trigger auto-creation                                                   |
