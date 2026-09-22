# US-049: Non-Compliance Management

## Summary

As a CIT Case Worker,  
I want to see non-compliance fields displayed on the Importer Notification and Import Record in Dynamics, pre-defined system views to find them, and a chronological comments timeline on the Importer Notification,  
So that I can save time on the number of notifications allocated to me, easily navigate to non-compliant records, and keep a record of all comments relating to import notifications with no associated health certificate.

## Description

CIT currently manage a manual off-system process for recording non-compliance queries against an import notification or Import Record. Capturing non-compliance directly in PIMS saves caseworkers time, increases the number of notifications that can be handled, and improves reporting visibility.

Non-compliance fields are new to both the Importer Notification and Import Record entities — they are not currently surfaced from one entity to the other via a sub-grid. The relationship between an Importer Notification and its Import Records is 1:N, implemented as a lookup from Import Record to Importer Notification.

Where an Importer Notification is being chased with the importer (for example, no health certificate attached) or a caseworker needs to record contact with International Trade Vets (ITV), a chronological comments timeline on the Importer Notification lets caseworkers keep a record of that contact.

This story was identified from Jira issues PLNT-4537, PLNT-4538 and PLNT-4543, which were missing from the original corpus compilation.

## Acceptance Criteria

- **AC-1 (Add Non-Compliance fields to Importer Notification):**  
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
  
  A caseworker can populate these non-compliance fields.

- **AC-2 (Add Non-Compliance fields to Import Record):**  
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
  
  A caseworker can populate these non-compliance fields.

- **AC-3 (Non-compliance fields from Importer Notification are displayed on Import Record):**  
  Where an Importer Notification with non-compliance fields populated (per AC-1) has an associated Import Record, the Import Record's Non-Compliance tab displays a read-only quick view form below its own Non-Compliance fields (per AC-2), showing the Importer Notification's Non-Compliance tab fields (per AC-1).

- **AC-4 (System views — Importer Notifications with active/closed non-compliance queries):**  
  An EU Imports Caseworker can select system views on the Importer Notification entity filtered by Non-Compliance Status = In Progress ("active") and Non-Compliance Status = Completed ("closed"). Column widths are adjusted so the whole column title is visible.

- **AC-5 (System views — Import Records with active/closed non-compliance queries):**  
  An EU Imports Caseworker can select system views on the Import Record entity filtered by Non-Compliance Status = In Progress ("active") and Non-Compliance Status = Completed ("closed"). Column widths are adjusted so the whole column title is visible.

- **AC-6 (Add chronological comments to Importer Notification):**  
  The Importer Notification form displays a timeline beneath the Document section, where a CIT Case Worker can add free-text, chronological notes (for example, recording contact made with the importer, or with International Trade Vets (ITV)) for notifications that do not have a health certificate attached and are being chased.

## Business Rules

- [BR-043](../business-rules.md#br-043) — Non-compliance fields default state
- [BR-044](../business-rules.md#br-044) — Non-compliance details mirrored onto Import Record via quick view

## Dependencies

- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md), [US-044](US-044-View-Importer-Notification.md) (Importer Notification — the entity the non-compliance tab and comments timeline are added to)
- [US-001](US-001-Manage-Import-Record.md) (Import Record — the entity the non-compliance tab and quick view are added to)
- [US-025](US-025-Import-Query-Management.md) (Import Query — the existing Queries tab that the Non-Compliance tab is positioned next to; non-compliance tracking is distinct from the Import Query entity)

## Traceability

### Source Jira Issues

- PLNT-4537
- PLNT-4538
- PLNT-4543
