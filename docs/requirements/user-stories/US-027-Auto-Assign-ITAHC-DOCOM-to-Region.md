# US-027: Auto-Assign ITAHC/DOCOM to Region

## Summary

As an EU Imports operations team,  
We need ITAHC and DOCOM records to be automatically assigned to the correct regional team (Carlisle / Scotland / Wales) when they are created by the TRACES Classic integration,  
So that records are immediately routable without manual assignment.

## Description

When ITAHC and DOCOM records are auto-created by the TRACES Classic integration ([US-007](US-007-Receive-ITAHC-From-TRACES.md), [US-008](US-008-Receive-DOCOM-From-TRACES.md)), they must be automatically assigned to the correct regional team based on routing logic. This avoids a manual step immediately after auto-creation.

**Note:** The routing logic and the criteria for determining which regional team an ITAHC/DOCOM should be assigned to are not specified in the source record. The acceptance criteria below are inferred and remain subject to business confirmation.

## Acceptance Criteria

- **AC-1:** ITAHC and DOCOM records created by the TRACES Classic integration are automatically assigned to the regional team that corresponds to the region indicated in the certificate data. The routing criteria are not specified in this baseline; the assignment resolves the devolved office from the destination postcode.

- **AC-2:** The regional team assignment must be set at the point of auto-creation, not as a subsequent manual step.

## Business Rules

None additional (routing criteria to be defined).

## Dependencies

- [US-007](US-007-Receive-ITAHC-From-TRACES.md) (ITAHC receipt from TRACES)
- [US-008](US-008-Receive-DOCOM-From-TRACES.md) (DOCOM receipt from TRACES)
- [US-026](US-026-Geographic-Team-Assignment.md) (Geographic team assignment — team records must exist)

## Traceability

### Source Jira Issues

- IMTA-6661
