# US-039: EU Imports — Daily IRMS Stats Dashboard

## Summary

As a Caseworker,  
I want to be able to view the "EU Imports — Daily IRMS Stats" dashboard,  
So that I can view IRMS-specific operational analytics.

## Description

A Dynamics 365 dashboard providing counts of ITAHCs and Import Notifications received today, plus live counts by status for ITAHC and Import Notification Import Records.

## Acceptance Criteria

- [x] **AC-1 (Count of ITAHCs received today):**  
  Count of ITAHC records where TRACES Notification Received Date = Today.

- [x] **AC-2 (Count of Import Notifications received today):**  
  Count of Import Notification records where Date Notification Received = Today.

- [x] **AC-3 (Count of live ITAHC Import Records by status):**  
  Count of Import Records with Primary ITAHC containing data, by Status Reason.  
  Filter: Primary ITAHC contains data AND Status Reason ∈ {Triage, Risk Assessment, Post Import Check}.

- [x] **AC-4 (Count of live Import Notification Import Records by status):**  
  Count of Import Records with Primary Import Notification containing data, by Status Reason.  
  Filter: Primary Import Notification contains data AND Status Reason ∈ {Triage, Risk Assessment, Post Import Check}.

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-002](US-002-Manage-ITAHC.md), [US-003](US-003-Manage-Import-Notification.md) (Import Record, ITAHC, Import Notification entities)

## Traceability

### Source Jira Issues

- IMTA-6343

### Original Links

- IMTA-6343
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

| Acceptance Criterion | Status        | Evidence                                                                           |
| -------------------- | ------------- | ---------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Dashboard count: ITAHCs received today (TRACES Notification Received Date = Today) |
| AC-2                 | ✅ Implemented | Dashboard count: Import Notifications received today                               |
| AC-3                 | ✅ Implemented | Dashboard chart: live ITAHC Import Records by status                               |
| AC-4                 | ✅ Implemented | Dashboard chart: live Import Notification Import Records by status                 |
