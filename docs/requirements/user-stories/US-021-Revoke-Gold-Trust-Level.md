# US-021: Revoke Gold Trust Level After Unsatisfactory Outcome

## Summary

As an EU Imports Caseworker,  
I want to be able to decide whether to revoke the Gold Trust Level of a Place of Origin when a Post Import Check outcome is Unsatisfactory,  
So that I can make an informed assessment of risk associated with that Place of Origin.

## Description

When a Post Import Check for an Import Record associated with a Gold Place of Origin results in an Unsatisfactory outcome, a mandatory field is shown to the caseworker asking whether to reset the Trust Level to Bronze. This is a deliberate decision gate, not automatic, to allow the caseworker to exercise judgement.

## Acceptance Criteria

- [x] **AC-1 (Mandatory decision field on unsatisfactory outcome for Gold Place of Origin):**  
  When a user completes the Completion stage of the Business Process Flow on an Import Record where:
  - Post Import Check Outcome = Unsatisfactory, AND
  - The Import Record is associated with a Gold/Bronze Commodity rule, AND
  - The linked Place of Origin has Trust Level = Gold,
  
  PIMS displays a mandatory field "Reset Gold Trust Level to Bronze?" with options Yes or No. The user cannot save without making a selection.

- [x] **AC-2 (Revoke Gold if user selects Yes):**  
  If the user selects Yes, PIMS sets the Trust Level of the associated Place of Origin to Bronze.

- [x] **AC-3 (No revocation if user selects No):**  
  If the user selects No, the Trust Level remains Gold.

## Business Rules

- [BR-013](../business-rules.md#br-013) — Trust Level revoked on Unsatisfactory + Yes selection

## Dependencies

- [US-017](US-017-Manage-Place-of-Origin.md), [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md) (Place of Origin and Trust Level)
- [US-023](US-023-Post-Import-Check-Management.md) (Post Import Check outcome)

## Traceability

### Source Jira Issues

- IMTA-6669

### Original Links

- IMTA-6669
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/ShowHideResetTrustLeveltoBronze-D5F0F569-4647-EA11-A812-000D3AB5D037.xaml
- src/solutions/defra_Imports/src/Workflows/ImportApplication-RevertVerifiedPlaceofOrigintoBro-00A435DD-6F3A-4E04-89D6-ACE88292A8EE.xaml

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: None identified

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                                                                                                                               |
| -------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/ShowHideResetTrustLeveltoBronze-D5F0F569-4647-EA11-A812-000D3AB5D037.xaml shows mandatory "Reset Gold Trust Level to Bronze?" field when: Post Import Check Outcome = Unsatisfactory + Gold/Bronze Commodity + Gold Place of Origin |
| AC-2                 | ✅ Implemented | User selects Yes → Trust Level set to Bronze via workflow src/solutions/defra_Imports/src/Workflows/ImportApplication-RevertVerifiedPlaceofOrigintoBro-00A435DD-6F3A-4E04-89D6-ACE88292A8EE.xaml                                                                                       |
| AC-3                 | ✅ Implemented | User selects No → Trust Level remains Gold                                                                                                                                                                                                                                             |
