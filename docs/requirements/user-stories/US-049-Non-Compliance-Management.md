# US-049: Non-Compliance Management

## Summary

As a CIT Case Worker,  
I want to see non-compliance fields displayed on the Importer Notification and Import Record in Dynamics, and pre-defined system views to find them,  
So that I can save time on the number of notifications allocated to me and easily navigate to non-compliant records.

## Description

CIT currently manage a manual off-system process for recording non-compliance queries against an import notification or Import Record. Capturing non-compliance directly in PIMS saves caseworkers time, increases the number of notifications that can be handled, and improves reporting visibility.

Non-compliance fields are new to both the Importer Notification and Import Record entities — they are not currently surfaced from one entity to the other via a sub-grid. The relationship between an Importer Notification and its Import Records is 1:N (the system allows many Import Records per notification, though in practice the team currently treats it as 1:1); the relationship between an Importer Notification and its non-compliance records is also 1:N.

This story was identified from Jira issues PLNT-4537 and PLNT-4538, which were missing from the original corpus compilation.

## Acceptance Criteria

- [ ] **AC-1 (Add Non-Compliance fields to Importer Notification):**  
  The Importer Notification form displays a "Non-Compliance" tab immediately to the right of the Queries tab, containing:
  - Contacted Due to Non-Compliance (Two Option, Y/N, default N)
  - Date Email Sent (Date only)
  - Date Telephone Call Made (Date only)
  - Type of Non-Compliance (Option Set — values: No Health Certificate, Other)
  - Other - Comments (single line of text; only displayed when Type of Non-Compliance = "Other", immediately beneath that field)
  - IRMS Person Responsible (Lookup to a user, in the style of an "assign" field)
  - Non-Compliance Status (Option Set — values: Blank, In Progress (default), Completed)
  - PIMS Status (Option Set — values: Open, Amended, Completed)
  - Date Completed (Date only)
  
  A caseworker can add a new non-compliance record.

- [ ] **AC-2 (Add Non-Compliance fields to Import Record):**  
  Where the non-compliance was flagged on the Importer Notification (Type of Non-Compliance is not null), the Import Record form displays a "Non-Compliance" tab immediately to the right of the Queries tab, containing:
  - Contacted Due to Non-Compliance (Two Option, Y/N, default N)
  - Date Email Sent (Date only)
  - Date Telephone Call Made (Date only)
  - Type of Non-Compliance (Multi-select Option Set — values: No Importer Notification, No UNN on Health Certificate, ITAHC but no GB Certificate, Health Certificate not attached to IN, No Fit to Travel Declaration, Other; if Other is selected, the "Other - Comments" box displays immediately beneath)
  - Other - Comments (single line of text; only displayed when Type of Non-Compliance includes "Other")
  - IRMS Person Responsible (Lookup to a user, in the style of an "assign" field)
  - Non-Compliance Status (Option Set — values: Blank, In Progress (default), Completed)
  - PIMS Status (Option Set — values: Open, Amended, Completed)
  - Date Completed (Date only)
  
  A caseworker can add a new non-compliance record.

- [ ] **AC-3 (Non-compliance record from Importer Notification is displayed on Import Record):**  
  Where an Importer Notification with non-compliance recorded (per AC-1) has an associated Import Record, the Import Record's Non-Compliance tab displays a read-only quick view form below its own Non-Compliance fields (per AC-2), showing the Importer Notification's Non-Compliance tab fields (per AC-1).

- [ ] **AC-4 (System views — Importer Notifications with active/closed non-compliance queries):**  
  An EU Imports Caseworker can select system views on the Importer Notification entity filtered by Non-Compliance Status = In Progress ("active") and Non-Compliance Status = Completed ("closed"). Column widths are adjusted so the whole column title is visible.

- [ ] **AC-5 (System views — Import Records with active/closed non-compliance queries):**  
  An EU Imports Caseworker can select system views on the Import Record entity filtered by Non-Compliance Status = In Progress ("active") and Non-Compliance Status = Completed ("closed"). Column widths are adjusted so the whole column title is visible.

## Business Rules

- [BR-043](../business-rules.md#br-043) — Non-compliance fields default state
- [BR-044](../business-rules.md#br-044) — Non-compliance details mirrored onto Import Record via quick view

## Dependencies

- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md), [US-044](US-044-View-Importer-Notification.md) (Importer Notification — the entity the non-compliance tab is added to)
- [US-001](US-001-Manage-Import-Record.md) (Import Record — the entity the non-compliance tab and quick view are added to)
- [US-025](US-025-Import-Query-Management.md) (Import Query — the existing Queries tab that the Non-Compliance tab is positioned next to; non-compliance tracking is distinct from the Import Query entity)

## Traceability

### Source Jira Issues

- PLNT-4537
- PLNT-4538

### Original Links

- PLNT-4537
- PLNT-4538

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

Low — newly identified requirement, not yet checked against solution metadata.

## Conformance Snapshot (2026-09-22)

- Status: ⬜ No Evidence Found
- Conflicts/Gaps: Newly identified requirement (PLNT-4537, PLNT-4538). Not yet built or verified. PLNT-4538's fifth acceptance criterion (a system view for Importer Notifications amended after a Health Certificate is attached) is tracked instead against [US-044](US-044-View-Importer-Notification.md) AC-6, as it depends on the Health Certificate Attached behaviour (PLNT-4542) rather than non-compliance.

## Acceptance Criteria Conformance

| Acceptance Criterion | Status              | Evidence                                                        |
| --------------------- | ------------------- | ----------------------------------------------------------------- |
| AC-1                  | ⬜ No Evidence Found | Newly identified requirement; not yet checked against solution metadata |
| AC-2                  | ⬜ No Evidence Found | Newly identified requirement; not yet checked against solution metadata |
| AC-3                  | ⬜ No Evidence Found | Newly identified requirement; not yet checked against solution metadata |
| AC-4                  | ⬜ No Evidence Found | Newly identified requirement; not yet checked against solution metadata |
| AC-5                  | ⬜ No Evidence Found | Newly identified requirement; not yet checked against solution metadata |
