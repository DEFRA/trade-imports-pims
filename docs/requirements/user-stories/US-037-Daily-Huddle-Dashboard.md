# US-037: EU Imports — Daily Huddle Stats Dashboard

## Summary

As an EU Imports Caseworker,  
I want to be able to view the "EU Imports — Daily Huddle Stats" dashboard,  
So that I can view live operational analytics of the system for daily team huddles.

## Description

A Dynamics 365 dashboard providing regional and risk-level breakdowns of live Import Records, completion counts and Post Import Check status.

## Acceptance Criteria

- [x] **AC-1 (Live Northern Import Records by Risk Level):**  
  Chart showing count of live Import Records by Risk Level for the Northern Region.  
  Filter: Import Record Type = ITAHC AND Region/Area Allocated to = North AND Status Reason ∈ {Triage, Risk Assessment, Post Import Check}.

- [x] **AC-2 (Live Southern Import Records by Risk Level):**  
  Chart showing count of live Import Records by Risk Level for the Southern Region.  
  Filter: Import Record Type = ITAHC AND Region/Area Allocated to = South AND Status Reason ∈ {Triage, Risk Assessment, Post Import Check}.

- [x] **AC-3 (Live Western Import Records by Risk Level):**  
  Chart showing count of live Import Records by Risk Level for the Western Region.  
  Filter: Import Record Type = ITAHC AND Region/Area Allocated to = West AND Status Reason ∈ {Triage, Risk Assessment, Post Import Check}.

- [x] **AC-4 (Count of live Import Records by Risk Level — all regions):**  
  Chart showing count of live Import Records for each risk level.  
  Filter: Status Reason ∈ {Triage, Risk Assessment, Post Import Check}.

- [x] **AC-5 (Count of ITAHCs completed yesterday):**  
  Chart showing count of Import Records completed yesterday.  
  Filter: Import Record Type = ITAHC AND Moved to Completion Date = Yesterday AND Moved to Completion? = Yes.

- [x] **AC-6 (Count of live Import Notifications by status):**  
  Chart showing count of live Import Records with a Primary Import Notification set, by Status Reason.  
  Filter: Status Reason ∈ {Triage, Risk Assessment, Post Import Check} AND Primary Import Notification contains data.

- [x] **AC-7 (Post Import Checks started vs not started):**  
  Chart showing count of live Post Import Checks that have been started and those that have not.  
  Filter: Post Import Check Outcome ∉ {Satisfactory, Resolved Not Required, Non-Compliant, Cancelled, Quarantined, Additional Inspection Required}.

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-023](US-023-Post-Import-Check-Management.md) (Import Record and Post Import Check data)

## Traceability

### Source Jira Issues

- IMTA-6340

### Original Links

- IMTA-6340
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
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                   |
| -------------------- | ------------- | -------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Dashboard chart: live ITAHC Import Records by Risk Level (Northern Region) |
| AC-2                 | ✅ Implemented | Dashboard chart: live ITAHC Import Records by Risk Level (Southern Region) |
| AC-3                 | ✅ Implemented | Dashboard chart: live ITAHC Import Records by Risk Level (Western Region)  |
| AC-4                 | ✅ Implemented | Dashboard chart: live Import Records by Risk Level (all regions)           |
| AC-5                 | ✅ Implemented | Dashboard chart: count of ITAHC completed yesterday                        |
| AC-6                 | ✅ Implemented | Dashboard chart: count of live Import Notifications by status              |
| AC-7                 | ✅ Implemented | Dashboard chart: Post Import Checks started vs not started                 |
