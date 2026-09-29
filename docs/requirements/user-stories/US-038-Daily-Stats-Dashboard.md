# US-038: EU Imports — Daily Stats Dashboard

## Summary

As an EU Imports Caseworker,  
I want to be able to view the "EU Imports — Daily Stats" dashboard,  
So that I can view analytics of records received today.

## Description

A Dynamics 365 dashboard providing counts of all records received today, across ITAHCs, Import Notifications, Post Import Checks (IV17s) and Import Queries.

## Acceptance Criteria

- **AC-1 (Count of ITAHCs received today):**  
  Count of ITAHC records where TRACES Notification Received Date = Today.

- **AC-2 (Count of Import Notifications received today):**  
  Count of Import Notification records where Date Notification Received = Today.

- **AC-3 (Count of IV17s received today):**  
  Count of Post Import Check records where IV17 Received Date = Today.

- **AC-4 (Count of Import Queries received today):**  
  Count of Import Query records where Date Raised = Today.

## Business Rules

None additional.

## Dependencies

- [US-002](US-002-Manage-ITAHC.md), [US-003](US-003-Manage-Import-Notification.md), [US-023](US-023-Post-Import-Check-Management.md), [US-025](US-025-Import-Query-Management.md) (ITAHC, Import Notification, Post Import Check, Import Query entities)

## Traceability

### Source Jira Issues

- IMTA-6341
