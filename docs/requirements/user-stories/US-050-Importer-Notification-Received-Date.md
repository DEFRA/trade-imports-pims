# US-050: Importer Notification Received Date and Timescale

## Summary

As a CIT Caseworker,  
I want the Import Record's "IV66" section relabelled to "Importer Notification" and the received date auto-populated from the linked Importer Notification's Submission Date,  
So that I no longer need to switch between the Import Record and Importer Notification entities, and the correct current terminology is used.

## Description

CIT currently flip between the Importer Notification and Import Record to populate the "Date IV66 Received" field on the Import Record. The field and its section are relabelled to reflect the current terminology (the paper IV66 form has been superseded by digital Importer Notifications — see the [Glossary](../glossary.md)), and the date is auto-populated from the associated Importer Notification to save caseworkers time.

This story consolidates two closely related Jira issues, PLNT-4540 (auto-population) and PLNT-4541 (relabelling), which were missing from the original corpus compilation.

## Acceptance Criteria

- [ ] **AC-1 (Auto-populate "Date Importer Notification Received" with "Submission Date"):**  
  When an EU Imports Caseworker selects "Create Import Record" on an Importer Notification, PIMS creates an Import Record with Import Record Type = Importer Notification (see [BR-039](../business-rules.md#br-039)) and auto-populates the new Import Record's "Date Importer Notification Received" field with the value of the associated Importer Notification's "Submission Date" field.

- [ ] **AC-2 (Section and field labels updated):**  
  On the Import Record form, for records with Import Record Type = Importer Notification, the section and field labels are updated as follows:
  - Section header: "IV66" → "Importer Notification"
  - Field: "Date IV66 Received" → "Date Importer Notification Received"
  - Field: "IV66 received in required timescales" → "Importer Notification Received within timescales"

## Business Rules

- [BR-040](../business-rules.md#br-040) — Date Importer Notification Received auto-populated from Submission Date

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record — the fields and section being relabelled and auto-populated reside here)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md), [US-044](US-044-View-Importer-Notification.md) (Importer Notification — source of the Submission Date and the "Create Import Record" button)

## Traceability

### Source Jira Issues

- PLNT-4540
- PLNT-4541

### Original Links

- PLNT-4540
- PLNT-4541

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
- Conflicts/Gaps: Newly identified requirement (PLNT-4540, PLNT-4541). Not yet built or verified. Note: [US-030](US-030-IV65-Due-Date-Calculation.md) covers the separate IV65 (formal written query) fields and is not affected by this story.

## Acceptance Criteria Conformance

| Acceptance Criterion | Status              | Evidence                                                        |
| --------------------- | ------------------- | ----------------------------------------------------------------- |
| AC-1                  | ⬜ No Evidence Found | Newly identified requirement; not yet checked against solution metadata |
| AC-2                  | ⬜ No Evidence Found | Newly identified requirement; not yet checked against solution metadata |
