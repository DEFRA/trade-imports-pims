# US-014: Automated Risk Assessment — P1 Consignments

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically determine whether a Post Import Check is required for a P1 Import Record based on Gold/Bronze commodity and Place of Origin trust level rules,  
So that Post Import Checks can be consistently scheduled based on policy.

## Description

For Import Records classified as P1 (high risk), PIMS evaluates whether a Gold/Bronze Commodity rule applies and, if so, what the Trust Level of the linked Place of Origin is. Based on this evaluation, PIMS sets the Post Import Checks Required? field and the Post Import Checks Required Reason.

If the Gold/Bronze rule applies but no verified Place of Origin is linked, the requirement is set to Undetermined.

If no Gold/Bronze rule applies, the inspection decision is flagged as Discretionary.

Where a Gold inspection is triggered but the caseworker subsequently overrides it to No, the 10th-inspection obligation is reallocated to the next Import Record for the same Place of Origin.

## Acceptance Criteria

- [x] **AC-1 (P1, Gold/Bronze commodity, Bronze Place of Origin → Post Import Check required):**  
  When an Import Record has Risk Level = P1 AND matches a Gold/Bronze Commodity rule AND the linked Place of Origin Trust Level = Bronze:
  - Post Import Checks Required? = **Yes**
  - Post Import Checks Required Reason = **Bronze Place of Origin**

- [x] **AC-2 (P1, Gold/Bronze commodity, Gold Place of Origin, counter ≥ 10 → Post Import Check required):**  
  When an Import Record has Risk Level = P1 AND matches a Gold/Bronze Commodity rule AND the linked Place of Origin Trust Level = Gold AND Number of Import Records Since Last Post Import Check ≥ 10:
  - Post Import Checks Required? = **Yes**
  - Post Import Checks Required Reason = **Gold Place of Origin — Inspection Coverage**
  - Number of Import Records Since Last Post Import Check on the Place of Origin is reset to 0

- [x] **AC-3 (P1, Gold/Bronze commodity, Gold Place of Origin, counter < 10 → No inspection required):**  
  When an Import Record has Risk Level = P1 AND matches a Gold/Bronze Commodity rule AND the linked Place of Origin Trust Level = Gold AND the counter is < 10:
  - Post Import Checks Required? = **No**
  - Post Import Checks Required Reason = **No Inspection Required — Gold Place of Origin**

- [x] **AC-4 (P1, Gold/Bronze commodity, no Place of Origin → Undetermined):**  
  When an Import Record has Risk Level = P1 AND matches a Gold/Bronze Commodity rule AND no Place of Origin is linked:
  - Post Import Checks Required? = **Undetermined**
  - Post Import Checks Required Reason = **Verified Place of Origin Missing**

- [x] **AC-5 (P1, no Gold/Bronze rule applies → Discretionary):**  
  When Risk Level = P1 AND no Gold/Bronze Commodity rule applies:
  - Post Import Checks Required? = **Discretionary**
  - Post Import Checks Required Reason = **Decision to inspect is discretionary**

- [x] **AC-6 (Manual override of Gold inspection reallocates to next Import Record):**  
  When an Import Record was flagged under AC-2 and the user subsequently sets Post Import Checks Required? = No:
  - Number of Import Records Since Last Post Import Check on the Place of Origin is incremented by 1

## Business Rules

- [BR-002](../business-rules.md#br-002) — Bronze Place of Origin → check required
- [BR-003](../business-rules.md#br-003) — Gold Place of Origin, counter ≥ 10 → check required
- [BR-004](../business-rules.md#br-004) — No Place of Origin → Undetermined
- [BR-005](../business-rules.md#br-005) — Manual override reallocates 10th inspection
- [BR-006](../business-rules.md#br-006) — No Gold/Bronze rule → Discretionary

## Dependencies

- [US-011](US-011-Manage-Commodity-Risk-Levels.md) (Commodity Risk Levels — P1 classification is a prerequisite)
- [US-012](US-012-Manage-Gold-Bronze-Commodities.md) (Gold/Bronze Commodity rules)
- [US-017](US-017-Manage-Place-of-Origin.md) (Place of Origin and Trust Level)

## Traceability

### Source Jira Issues

- IMTA-5866
- IMTA-5894

### Original Links

- IMTA-5866
- IMTA-5894
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/ImportApplication-ManualPostImportCheckDecision-7436FAAE-9A22-4821-9A1B-2AA5A22BE272.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                                                                                     |
| -------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/ImportApplication-ManualPostImportCheckDecision-7436FAAE-9A22-4821-9A1B-2AA5A22BE272.xaml evaluates: P1 + Gold/Bronze Commodity + Bronze Place of Origin → sets Post Import Checks Required? = Yes with Reason = "Bronze Place of Origin" |
| AC-2                 | ✅ Implemented | P1 + Gold/Bronze + Gold Place of Origin + counter ≥ 10 → Post Import Checks Required? = Yes, Reason = "Gold Place of Origin — Inspection Coverage", counter reset to 0                                                                                                                       |
| AC-3                 | ✅ Implemented | P1 + Gold/Bronze + Gold Place of Origin + counter < 10 → Post Import Checks Required? = No, Reason = "No Inspection Required — Gold Place of Origin"                                                                                                                                         |
| AC-4                 | ✅ Implemented | P1 + Gold/Bronze + no Place of Origin → Post Import Checks Required? = Undetermined, Reason = "Verified Place of Origin Missing"                                                                                                                                                             |
| AC-5                 | ✅ Implemented | P1 + no Gold/Bronze rule → Post Import Checks Required? = Discretionary, Reason = "Decision to inspect is discretionary"                                                                                                                                                                     |
| AC-6                 | ✅ Implemented | Manual override (AC-2 to No) → Inspection Quota Counter incremented by 1 on Place of Origin                                                                                                                                                                                                  |
