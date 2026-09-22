# US-041: EU Imports — APHA Border Control Metrics Dashboard

## Summary

As a Caseworker,  
I want to be able to view the "EU Imports — APHA Border Control Metrics" dashboard,  
So that I can view APHA border control analytics for Dogs, Cats and Ferrets checks.

## Description

A Dynamics 365 dashboard providing compliance metrics for the 10% low risk and high risk commercial Dogs/Cats/Ferrets inspection policies.

## Acceptance Criteria

- **AC-1 (10% checks on Low Risk Commercial Dogs/Cats/Ferrets required — last month):**  
  Chart: count by month of Import Records where Created On is within the last 1 month AND Import Risk Level = P2 AND Commodity Type ∈ {Cat, Dog, Ferret} AND Post Import Checks Required? = Yes.

- **AC-2 (10% checks on Low Risk Commercial Dogs/Cats/Ferrets created — last month):**  
  Chart: count by month of Post Import Checks where related Import Record matches the AC-1 filter.

- **AC-3 (10% checks on High Risk Gold Dogs/Cats/Ferrets required — last month):**  
  Chart: count by month of Import Records where Created On is within the last 1 month AND Import Risk Level = P1 AND Commodity Type ∈ {Cat, Dog, Ferret} AND Post Import Checks Required? = Yes AND Verified Place of Origin = Gold.

- **AC-4 (10% checks on High Risk Gold Dogs/Cats/Ferrets created — last month):**  
  Chart: count by month of Post Import Checks where related Import Record matches the AC-3 filter.

- **AC-5 (100% checks on High Risk Bronze Dogs/Cats/Ferrets required — last month):**  
  Chart: count by month of Import Records where Created On is within the last 1 month AND Import Risk Level = P1 AND Commodity Type ∈ {Cat, Dog, Ferret} AND Post Import Checks Required? = Yes AND Verified Place of Origin = Bronze OR was manually input.

- **AC-6 (100% checks on High Risk Bronze Dogs/Cats/Ferrets created — last month):**  
  Chart: count by month of Post Import Checks where related Import Record matches the AC-5 filter.

## Business Rules

None additional.

## Dependencies

- [US-001](US-001-Manage-Import-Record.md), [US-023](US-023-Post-Import-Check-Management.md) (Import Record and Post Import Check entities)

## Traceability

### Source Jira Issues

- IMTA-6372
