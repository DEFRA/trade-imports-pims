# US-029: Document Attachment

## Summary

As an EU Imports Caseworker,  
I want to be able to attach multiple documents to an Import Record,  
So that I can track important communications such as PDF versions of ITAHCs and other documentation associated with the record.

## Description

Caseworkers can attach documents (e.g. PDF certificates, correspondence) to Import Records using the Azure Attachment Management solution (Microsoft Labs). Attached documents are visible within the Import Record but cannot be deleted once attached, ensuring an immutable document trail.

This baseline has evidence for generic attachment capability, including storing PDF versions of ITAHCs as documents. It does not currently have confirmed evidence of a distinct automated ITAHC PDF-generation workflow on receipt; that scope remains an open confirmation item. See [assumptions-and-constraints.md](../assumptions-and-constraints.md) DEP-007.

## Acceptance Criteria

- **AC-1 (Attach documents):**  
  An EU Imports Caseworker can attach multiple documents to an Import Record using an intuitive file attachment interface.

- **AC-2 (View attached documents):**  
  An EU Imports Caseworker can see a list of all documents attached to an Import Record within the record itself.

- **AC-3 (Prevent document deletion):**  
  An EU Imports Caseworker cannot delete documents once they have been attached to an Import Record.

- **AC-4 (Document types on Importer Notification):**  
  The Document Type field on documents attached to an Importer Notification offers (in addition to Health Certificate): Latest Health Certificate, Air waybill, Import permit, Letter of authority (Directive 2008/61/EC), Sea waybill, Rail waybill, Customs declaration, Bill of lading, Laboratory Sampling results for Aflatoxin (Reg 2019/1793).

- **AC-5 (Latest Health Certificate behaviour):**  
  A document with Document Type = Latest Health Certificate behaves the same as Health Certificate (sets Health Certificate Attached = Y; updates Owner and Status per [BR-042](../business-rules.md#br-042) where the Importer Notification is already completed — see [BR-056](../business-rules.md#br-056)). Where a Latest Health Certificate is superseded by a newer one, the superseded document's Document Type is re-classified to Health Certificate.

- **AC-6 (Attachments to Place of Destination):**  
  An EU Imports Caseworker can attach documents to a Place of Destination record and view previously-attached documents there, supported by an "Active Place of Destinations" view showing Organisation Name and Address fields.

- **AC-7 (View IPAFFS-held attachments):**  
  Where an Importer Notification or Import Record has documents held in IPAFFS, an EU Imports Caseworker can view a read-only link to each document together with its Document Type, Document Reference and Document Issue Date, opening the document in a separate window without edit access. These links and their metadata are removed if the Importer Notification is deleted.

- **AC-8 (Documents attached to an Importer Notification remain editable):**  
  Documents attached to an Importer Notification (as distinct from an Import Record) can be viewed and edited by an EU Imports Caseworker. This is an exception to the non-deletion rule in AC-3, which applies to Import Record attachments; Importer Notification attachments may still not be deleted.

## Business Rules

- [BR-031](../business-rules.md#br-031) — Documents attached to Import Records cannot be deleted
- [BR-056](../business-rules.md#br-056) — Latest Health Certificate document type mirrors Health Certificate behaviour

## Dependencies

- Azure Attachment Management solution (Microsoft Labs) deployed (ASM-009, DEP-005)
- [US-001](US-001-Manage-Import-Record.md) (Import Record)
- [US-003](US-003-Manage-Importer-Notification.md) (document types and Latest Health Certificate behaviour on Importer Notification)

## Traceability

### Source Jira Issues

- IMTA-5913
- IMTA-7779
- IMTA-7782
- IMTA-10471
- EDA-648
- EDA-681
- EDA-728
