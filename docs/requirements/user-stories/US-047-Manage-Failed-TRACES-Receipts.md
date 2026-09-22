# US-047: Manage Failed TRACES Receipts

## Summary

As an operational user responsible for EU imports processing,  
I want failed inbound TRACES receipts to be visible, diagnosable and reprocessable,  
So that ITAHC and DOCOM integration failures do not silently block casework.

## Description

Inbound TRACES Classic processing can fail before an ITAHC or DOCOM record is created or updated in PIMS. This story captures the explicit failed-receipt handling requirement inferred from IMTA-6626.

**Note:** The source issue content was empty and this requirement is reconstructed from the issue summary. Operator workflow detail should be confirmed with the business if a more specific support process is required.

## Acceptance Criteria

- **AC-1:** If an inbound ITAHC or DOCOM receipt cannot be processed successfully, PIMS captures the failed receipt with the message context and error information needed for investigation.

- **AC-2:** Authorised operational users can see failed TRACES receipts and distinguish whether the failure relates to ITAHC or DOCOM processing.

- **AC-3:** Authorised operational users can initiate a controlled retry or reprocess action for a failed TRACES receipt after corrective action has been taken.

- **AC-4:** Retry and resolution activity for failed TRACES receipts is auditable.

## Business Rules

- [BR-035](../business-rules.md#br-035) — Failed TRACES receipts must be captured, visible and reprocessable

## Dependencies

- DEP-001 (TRACES schemas and message format)
- DEP-004 (Azure Integration Services infrastructure)
- [US-007](US-007-Receive-ITAHC-From-TRACES.md) (Receive ITAHC from TRACES Classic)
- [US-008](US-008-Receive-DOCOM-From-TRACES.md) (Receive DOCOM from TRACES Classic)

## Traceability

### Source Jira Issues

- IMTA-6626
