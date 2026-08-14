# US-044: View Importer Notification in PIMS

## Summary

As an EU Imports Caseworker,  
I want to be able to view an Importer Notification record within PIMS,  
So that I can use the information contained within an Importer Notification to support the risk assessment process.

## Description

Caseworkers can view Importer Notification records in PIMS (auto-received from IPAFFS via [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md)). Caseworkers have read-only access to these records and cannot edit them. The implemented entity is Importer Notification, although some legacy labels and views may still use the term Import Notification.

## Acceptance Criteria

- [x] **AC-1 (View Importer Notification fields on PIMS system form):**  
  An EU Imports Caseworker can view the Importer Notification fields on the PIMS system form as specified in the agreed D365 Importer Notification schema (reference: EU Imports — CIT 3.0 — Importer Notification Schemas.xlsx, worksheet "3-I.N. D365 Schema IMTA-7201").

- [x] **AC-2 (Security role permissions):**  
  The EU Imports Caseworker security role has Read, Append and Append To permissions on the Importer Notification entity. The role does not have Create or Update permissions.

- [x] **AC-3 (System views):**  
  System view names, fields and sort orders for Importer Notifications align to the implemented views.

## Business Rules

None additional.

## Dependencies

- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md) (Importer Notifications auto-received from IPAFFS)
- Importer Notification schema agreed (DEP-002)

## Traceability

### Source Jira Issues

- IMTA-7201
- IMTA-7240

### Original Links

- IMTA-7201
- IMTA-7240
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

High Medium

## Conformance Snapshot (2026-07-22)

- Status: ⚠️ Partially Implemented
- Conflicts/Gaps: The source record (IMTA-7240) did not specify the view field list. System views are implemented and accepted as correct, but attribute-level traceability back to a specification is not available.

## Acceptance Criteria Conformance

| Acceptance Criterion | Status                  | Evidence                                                                                                          |
| -------------------- | ----------------------- | ----------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented           | Importer Notification form configured with fields per D365 Importer Notification schema                           |
| AC-2                 | ✅ Implemented           | EU Imports Caseworker role: Read, Append, Append To permissions on Importer Notification entity; no Create/Update |
| AC-3                 | ⚠️ Partially Implemented | System views for Importer Notifications configured; source attribute detail not specified                         |
