# US-060: APHA Owner Team Assignment

## Summary

As an APHA caseworker,
I want to manually assign Import Records and Importer Notifications to dedicated APHA owner teams,
So that approved/rejected travel cases are tracked separately from standard regional casework.

## Description

Four additional owner Teams support APHA-specific casework: AIS Approved Travel, AIS Reject Travel, AIS Approved Enter, AIS Reject Enter. These are assigned manually by APHA; there is no automated routing rule for them, unlike the geographic teams in [US-026](US-026-Geographic-Team-Assignment.md) and [US-027](US-027-Auto-Assign-ITAHC-DOCOM-to-Region.md).

**Note for Solution Architect:** these are environment-provisioned D365 Team records with an assigned security role, similar in nature to the geographic teams referenced in [DEP-006](../assumptions-and-constraints.md). Provisioning and promotion approach across environments should be reviewed alongside that existing pattern.

## Acceptance Criteria

- **AC-1 (Teams available as owners):** An EU Imports Caseworker can assign an Import Record or Importer Notification to one of the following Teams: AIS Approved Travel, AIS Reject Travel, AIS Approved Enter, AIS Reject Enter.

- **AC-2 (Manual assignment only):** Assignment to these Teams is always performed manually by a caseworker; no automated rule assigns records to them.

- **AC-3 (Security role):** Each of the four Teams is granted the EU Imports Caseworker security role.

## Business Rules

- None additional to standard D365 team ownership.

## Dependencies

- [US-026](US-026-Geographic-Team-Assignment.md) (equivalent D365 Team provisioning pattern)

## Traceability

### Source Jira Issues

- EDA-618
