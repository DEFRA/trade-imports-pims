# US-041: EU Imports — APHA Border Control Metrics Dashboard

## Summary

As a Caseworker,  
I want to be able to view the "EU Imports — APHA Border Control Metrics" dashboard,  
So that I can view APHA border control analytics for Dogs, Cats and Ferrets checks.

## Description

A Dynamics 365 dashboard providing compliance metrics for the 10% low risk and high risk commercial Dogs/Cats/Ferrets inspection policies.

## Acceptance Criteria

- [x] **AC-1 (10% checks on Low Risk Commercial Dogs/Cats/Ferrets required — last month):**  
  Chart: count by month of Import Records where Created On is within the last 1 month AND Import Risk Level = P2 AND Commodity Type ∈ {Cat, Dog, Ferret} AND Post Import Checks Required? = Yes.

- [x] **AC-2 (10% checks on Low Risk Commercial Dogs/Cats/Ferrets created — last month):**  
  Chart: count by month of Post Import Checks where related Import Record matches the AC-1 filter.

- [x] **AC-3 (10% checks on High Risk Gold Dogs/Cats/Ferrets required — last month):**  
  Chart: count by month of Import Records where Created On is within the last 1 month AND Import Risk Level = P1 AND Commodity Type ∈ {Cat, Dog, Ferret} AND Post Import Checks Required? = Yes AND Verified Place of Origin = Gold.

- [x] **AC-4 (10% checks on High Risk Gold Dogs/Cats/Ferrets created — last month):**  
  Chart: count by month of Post Import Checks where related Import Record matches the AC-3 filter.

- [x] **AC-5 (100% checks on High Risk Bronze Dogs/Cats/Ferrets required — last month):**  
  Chart: count by month of Import Records where Created On is within the last 1 month AND Import Risk Level = P1 AND Commodity Type ∈ {Cat, Dog, Ferret} AND Post Import Checks Required? = Yes AND Verified Place of Origin = Bronze OR was manually input.

- [x] **AC-6 (100% checks on High Risk Bronze Dogs/Cats/Ferrets created — last month):**  
  Chart: count by month of Post Import Checks where related Import Record matches the AC-5 filter.

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-023](US-023-Post-Import-Check-Management.md) (Import Record and Post Import Check entities)

## Traceability

### Source Jira Issues

- IMTA-6372

### Original Links

- IMTA-6372
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

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                          |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Dashboard chart: 10% checks on Low Risk Commercial Dogs/Cats/Ferrets required (P2 + Commodity ∈ {Cat, Dog, Ferret} + Post Import Checks Required? = Yes, Created On = This Month) |
| AC-2                 | ✅ Implemented | Dashboard chart: 10% checks created for low-risk commercial Dogs/Cats/Ferrets                                                                                                     |
| AC-3                 | ✅ Implemented | Dashboard chart: 10% checks on High Risk Gold Dogs/Cats/Ferrets required (P1 + Gold Place of Origin + Commodity ∈ {Cat, Dog, Ferret} + Post Import Checks Required? = Yes)        |
| AC-4                 | ✅ Implemented | Dashboard chart: 10% checks created for high-risk gold Dogs/Cats/Ferrets                                                                                                          |
| AC-5                 | ✅ Implemented | Dashboard chart: 100% checks on High Risk Bronze Dogs/Cats/Ferrets required (P1 + Bronze Place of Origin + Commodity ∈ {Cat, Dog, Ferret})                                        |
| AC-6                 | ✅ Implemented | Dashboard chart: 100% checks created for high-risk bronze Dogs/Cats/Ferrets                                                                                                       |
