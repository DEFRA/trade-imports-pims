# US-044: View Importer Notification in PIMS

## Summary

As an EU Imports Caseworker,  
I want to be able to view an Importer Notification record within PIMS,  
So that I can use the information contained within an Importer Notification to support the risk assessment process.

## Description

Caseworkers can view Importer Notification records in PIMS (auto-received from IPAFFS via [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md)). Caseworkers have read-only access to these records and cannot edit them. The Importer Notification entity is the current record type, although some legacy labels and views may still use the term Import Notification.

## Acceptance Criteria

- **AC-1 (View Importer Notification fields on PIMS system form):**  
  An EU Imports Caseworker can view the Importer Notification fields on the PIMS system form as specified in the agreed D365 Importer Notification schema (reference: EU Imports — CIT 3.0 — Importer Notification Schemas.xlsx, worksheet "3-I.N. D365 Schema IMTA-7201").

- **AC-2 (Security role permissions):**  
  The EU Imports Caseworker security role has Read, Append, Append To, Assign, Share and **Write (Update)** permissions on the Importer Notification entity, all at Global level. The role does **not** have Create permission — an Importer Notification can only be received from IPAFFS ([US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md)), not manually created by a caseworker.

- **AC-3 (System views):**  
  System view names, fields and sort orders for Importer Notifications align to the implemented views.

- **AC-4 (Health Certificate Attached field):**  
  The Importer Notification Details tab displays a Health Certificate Attached field immediately beneath the "Imp Type" field. The field is auto-populated with Y if a document with Document Type = Health Certificate and a populated URL is attached to the Importer Notification, otherwise N. The field remains editable by an EU Imports Caseworker.

- **AC-5 (Shared "IMP" option label displays as "Importer Notification"):**  
  The Importer Notification Details form displays the option label previously shown as "IMP" as "Importer Notification" (see [BR-038](../business-rules.md#br-038)). This is the same option label used by the Import Record Type field on the Import Record entity — a different entity from Importer Notification. Only the "IMP" label rename is shared between the two entities; the removal of CED, CVEDA and CVEDP is scoped to Import Record Type only and does not apply to the Importer Notification's own notification type classification.

- **AC-6 (System view — Amended Notifications where Health Certificate Attached and Owner changed to EU Imports Dynamics):**  
  An EU Imports Caseworker can select a system view named "Amended Notifications where Health Certificate Attached and Owner changed to EU Imports Dynamics" on the Importer Notification entity, filtered by Status = Amend, Owner = EU Imports Dynamics Application User, Health Certificate Attached = Yes. Column widths are adjusted so the whole column title is visible.

## Business Rules

- [BR-038](../business-rules.md#br-038) — Import Record Type value list ("IMP" displays as "Importer Notification")
- [BR-041](../business-rules.md#br-041) — Health Certificate Attached auto-flag
- [BR-042](../business-rules.md#br-042) — Health Certificate Attached after completion triggers amendment and reassignment

## Dependencies

- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md) (Importer Notifications auto-received from IPAFFS; Health Certificate Attached-triggered amendment)
- Importer Notification schema agreed (DEP-002)

## Traceability

### Source Jira Issues

- IMTA-7201
- IMTA-7240
- PLNT-4536
- PLNT-4538
- PLNT-4542
