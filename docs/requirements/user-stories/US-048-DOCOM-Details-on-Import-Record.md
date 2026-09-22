# US-048: DOCOM Details on Import Record

## Summary

As a CIT Case Worker,  
I want to see new DOCOM-specific fields displayed on a separate tab on Import Records in Dynamics,  
So that I can save time on the number of records allocated to me.

## Description

CIT currently manage a manual off-system process for requesting Proof of Delivery (POD) for DOCOM-related consignments, which includes populating some information already held in PIMS. Capturing this in PIMS saves caseworkers time and increases the number of notifications that can be handled, by adding a suite of DOCOM-related fields directly to the Import Record.

This story was identified from Jira issue PLNT-4539, which was missing from the original corpus compilation.

## Acceptance Criteria

- [ ] **AC-1 (Add DOCOM section fields to Import Record):**  
  A DOCOM tab is displayed on the Import Record form immediately to the right of the Post Import Checks tab. The section heading on the tab is "DOCOM". The tab contains the following fields, all optional (not mandatory):
  - DOCOM Category (Option Set — values: Cat1, Cat2, Cat3 - PAP, Cat3 – PAP Fish, Cat3 - Other)
  - Requested POD (Option Set — values: Blank, Y, N)
  - Date POD Requested (Date/Time field)
  - Reply Received (Option Set — values: Blank, Y, N)

## Business Rules

None additional — all DOCOM tab fields are optional with no default value or conditional logic.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record — the DOCOM tab is added to this entity)
- [US-004](US-004-Manage-DOCOM.md) (DOCOM record — the DOCOM tab on the Import Record is additional context for DOCOM-related consignments, not a replacement for the DOCOM entity)

## Traceability

### Source Jira Issues

- PLNT-4539

### Original Links

- PLNT-4539

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
- Conflicts/Gaps: Newly identified requirement (PLNT-4539). Not yet built or verified.

## Acceptance Criteria Conformance

| Acceptance Criterion | Status              | Evidence                                                        |
| --------------------- | ------------------- | ----------------------------------------------------------------- |
| AC-1                  | ⬜ No Evidence Found | Newly identified requirement; not yet checked against solution metadata |
