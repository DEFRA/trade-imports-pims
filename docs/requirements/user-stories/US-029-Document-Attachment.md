# US-029: Document Attachment

## Summary

As an EU Imports Caseworker,  
I want to be able to attach multiple documents to an Import Record,  
So that I can track important communications such as PDF versions of ITAHCs and other documentation associated with the record.

## Description

Caseworkers can attach documents (e.g. PDF certificates, correspondence) to Import Records using the Azure Attachment Management solution (Microsoft Labs). Attached documents are visible within the Import Record but cannot be deleted once attached, ensuring an immutable document trail.

This baseline has evidence for generic attachment capability, including storing PDF versions of ITAHCs as documents. It does not currently have confirmed evidence of a distinct automated ITAHC PDF-generation workflow on receipt; that scope remains an open confirmation item. See [assumptions-and-constraints.md](../assumptions-and-constraints.md) DEP-007.

## Acceptance Criteria

- [x] **AC-1 (Attach documents):**  
  An EU Imports Caseworker can attach multiple documents to an Import Record using an intuitive file attachment interface.

- [x] **AC-2 (View attached documents):**  
  An EU Imports Caseworker can see a list of all documents attached to an Import Record within the record itself.

- [x] **AC-3 (Prevent document deletion):**  
  An EU Imports Caseworker cannot delete documents once they have been attached to an Import Record.

## Business Rules

- [BR-031](../business-rules.md#br-031) — Documents attached to Import Records cannot be deleted

## Dependencies

- Azure Attachment Management solution (Microsoft Labs) deployed (ASM-009, DEP-005)
- [US-001](US-001-Manage-Import-Record.md) (Import Record)

## Traceability

### Source Jira Issues

- IMTA-5913

### Original Links

- IMTA-5913
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

- Status: ✅ Fully Implemented
- Conflicts/Gaps: **DEP-007:** Automated ITAHC PDF-generation workflow scope remains unconfirmed

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                  |
| -------------------- | ------------- | ----------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Azure Attachment Management solution deployed allows document attachment on Import Record |
| AC-2                 | ✅ Implemented | List of attached documents visible within Import Record                                   |
| AC-3                 | ✅ Implemented | Documents cannot be deleted after attachment (immutable document trail)                   |
