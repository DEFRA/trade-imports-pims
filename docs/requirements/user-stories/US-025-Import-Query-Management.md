# US-025: Import Query Management

## Summary

As an EU Imports Caseworker,  
I want to be able to create, track, assign and resolve Import Queries against Import Records,  
So that I can record formal queries to importers or third parties and monitor their resolution.

## Description

Import Queries are formal queries raised by caseworkers against Import Records, directed to importers or other relevant parties. They are managed as a custom activity entity in Dynamics 365, leveraging OOB activity features for Raised By, Raised Date, assignment and Close as Resolved.

Each query receives an auto-generated sequential query number in the format `RMQ{YY}-{SEQNUM:4}`.

**Note:** IMTA-6185 (using "Import Record") and IMTA-6255 (using "Import Application") are duplicate source records describing the same functionality. They are consolidated into this single story, using the canonical term "Import Record".

## Acceptance Criteria

- [x] **AC-1 (Create/update Import Query):**  
  An EU Imports Caseworker can create or update an Import Query with the following fields:
  - Query Sent To (email address)
  - Summary (Mandatory)
  - Related Import Record (Mandatory)
  - Date Due to Be Resolved
  - Detailed Description of the Query

- [x] **AC-2 (View related Import Record context):**  
  Within the Import Query form, the caseworker can view the following fields from the related Import Record:
  - The relevant Health Certificate (ITAHC/DOCOM)
  - Commodity Type
  - Place of Origin Country
  - Any additional Health Certificates

- [x] **AC-3 (Auto-populated fields on creation):**  
  The following fields are auto-populated when an Import Query is created:
  - Date Raised = today's date
  - Who Raised the Query = the creating user
  - Query Number = auto-generated in format `RMQ{YY}-{SEQNUM:4}`, unique across all Import Queries

- [x] **AC-4 (Global query views with filters):**  
  An EU Imports Caseworker can view all queries in the system filtered by:
  - All Active Queries
  - All Completed Queries
  - All Overdue Queries
  - All Queries (all statuses)
  - My Active Queries
  - My Completed Queries
  - My Overdue Queries
  - All My Queries
  
  Queries are sorted by Query Number. Each view shows: Query Number, Related Import Record, Query Sent To, Who Raised the Query, Summary, Date Raised, Date Due, Completion Status, Resolution Date (if resolved).

- [x] **AC-5 (Related query views within an Import Record):**  
  An EU Imports Caseworker can view all queries related to a specific Import Record within the Import Record, filtered by:
  - All Related Queries
  - Overdue Related Queries
  
  Sorted by Query Number. Same fields as AC-4.

- [x] **AC-6 (Search for queries):**  
  An EU Imports Caseworker can search for queries by Query Number or Import Record name.

- [x] **AC-7 (Attach notes and files):**  
  An EU Imports Caseworker can attach notes and files to any Import Query, including queries they do not own. Notes and files record the date added and who added them.

- [x] **AC-8 (Close as resolved):**  
  An EU Imports Caseworker can close an Import Query as resolved. PIMS records the resolution date automatically.

- [x] **AC-9 (Assign to another caseworker):**  
  An EU Imports Caseworker can assign an Import Query to another EU Imports Caseworker.

## Business Rules

- [BR-023](../business-rules.md#br-023) — Query number format: `RMQ{YY}-{SEQNUM:4}`
- [BR-032](../business-rules.md#br-032) — Notes can be added by non-owners

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record — queries are linked to Import Records)

## Traceability

### Source Jira Issues

- IMTA-6185
- IMTA-6255

### Original Links

- IMTA-6185
- IMTA-6255
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Entities/defraimp_importquery/Entity.xml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                  |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | src/solutions/defra_Imports/src/Entities/defraimp_importquery/Entity.xml entity with Query Sent To, Summary (mandatory), Related Import Record (mandatory), Date Due to Be Resolved, Detailed Description |
| AC-2                 | ✅ Implemented | Import Query form includes Read-Only section showing related Import Record context (Health Certificate, Commodity Type, Place of Origin, etc.)                                                            |
| AC-3                 | ✅ Implemented | Auto-populated fields: Date Raised, Who Raised Query, Query Number (format RMQ{YY}-{SEQNUM:4})                                                                                                            |
| AC-4                 | ✅ Implemented | Global query views with filters (All Active, All Completed, All Overdue, My Active, My Completed, My Overdue, All My Queries)                                                                             |
| AC-5                 | ✅ Implemented | Related queries view within Import Record with filters (All Related, Overdue Related)                                                                                                                     |
| AC-6                 | ✅ Implemented | Search by Query Number or Import Record name                                                                                                                                                              |
| AC-7                 | ✅ Implemented | Attachment capability on Import Query (notes and files) via Activity association                                                                                                                          |
| AC-8                 | ✅ Implemented | Close as Resolved workflow updates Resolution Date                                                                                                                                                        |
| AC-9                 | ✅ Implemented | Query can be assigned to another caseworker                                                                                                                                                               |
