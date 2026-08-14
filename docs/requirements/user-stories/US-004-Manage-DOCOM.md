# US-004: Manage DOCOM

## Summary

As an EU Imports Caseworker,  
I want to be able to create, update, view and search for DOCOM records,  
So that I can record information relating to a Document of Commercial Movement consignment.

## Description

A DOCOM (Document of Commercial Movement) is a health certificate used for commercial movements of certain animals. DOCOM records can be manually created by caseworkers or auto-created by the TRACES Classic integration ([US-008](US-008-Receive-DOCOM-From-TRACES.md)).

## Acceptance Criteria

- [x] **AC-1:** An EU Imports Caseworker can create or update a DOCOM record with the following fields:
  - Certificate Reference Number (Mandatory)
  - Local Reference Number
  - Receiving Category (Option Set)
  - Purpose (Option Set)
  - Seal Number
  - Container Number

- [x] **AC-2:** An EU Imports Caseworker can view a list of all DOCOM records ordered by creation date (newest first), showing: Certificate Reference Number, Local Reference Number, Receiving Category, Purpose, Seal Number, Container Number, Created On.

- [x] **AC-3:** An EU Imports Caseworker can perform a free text search for a DOCOM using: Certificate Reference Number, Local Reference Number, Receiving Category, Purpose, Seal Number, Container Number.

## Business Rules

None specific to this entity beyond standard record ownership.

## Dependencies

- [US-008](US-008-Receive-DOCOM-From-TRACES.md) (Auto-receipt from TRACES Classic)
- [US-001](US-001-Manage-Import-Record.md) (Import Record may link to a DOCOM)

## Traceability

### Source Jira Issues

- IMTA-6252

### Original Links

- IMTA-6252
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Entities/defraimp_docom/Entity.xml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                          |
| -------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | src/solutions/defra_Imports/src/Entities/defraimp_docom/Entity.xml - Certificate Reference Number (mandatory), Local Reference Number, Receiving Category, Purpose, Seal Number, Container Number |
| AC-2                 | ✅ Implemented | SavedQueries for DOCOM list view with all fields visible                                                                                                                                          |
| AC-3                 | ✅ Implemented | Free-text search views on all AC-3 fields                                                                                                                                                         |
