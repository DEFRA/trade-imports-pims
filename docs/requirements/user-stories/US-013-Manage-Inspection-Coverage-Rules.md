# US-013: Manage Inspection Coverage Rules

## Summary

As a Dynamics 365 System Administrator,  
I want to be able to update Inspection Coverage Rules,  
So that PIMS can use these rules to automatically determine which Import Records should be flagged for a random Post Import Check.

## Description

Inspection Coverage Rules define the thresholds at which random post-import checks are triggered. Rules are explicitly named (e.g., "10% Rule" for P2 frequency, "2% All-Case-Random" for P3 random coverage, "Gold Commodity Every X" for Gold commodity checks) and are associated with risk levels to enable PIMS to apply the correct rule per risk classification. The Number of Records Until Inspection field holds the threshold count (e.g., 10 for 10% frequency, 50 for 2% frequency). These rules are held in a custom entity and are only editable by the Dynamics 365 System Administrator. All other business user roles are restricted to read-only access to prevent unauthorised policy changes.

## Acceptance Criteria

- **AC-1:** A Dynamics 365 System Administrator can update an existing Inspection Coverage Rule record with the following mandatory fields:
  - Rule Name (Free text — Mandatory)
  - Risk Level (Lookup — Mandatory)
  - Number of Records Until Inspection (Whole Number — Mandatory)

- **AC-2:** Only the Dynamics 365 System Administrator role can create or update Inspection Coverage Rule records. All other business user security roles are restricted to read access.

- **AC-3:** Inspection Coverage Rules are associated to a Risk Level, enabling PIMS to apply the correct rule per risk level.

## Business Rules

- [BR-007](../business-rules.md#br-007) — P2 10% rule uses Inspection Coverage Rule threshold
- [BR-008](../business-rules.md#br-008) — P3 2% rule uses Inspection Coverage Rule threshold
- [BR-030](../business-rules.md#br-030) — Only System Administrators can update Inspection Coverage Rules

## Dependencies

- [US-015](US-015-Automated-Risk-Assessment-P2.md) (P2 random 10% rule consumes this configuration)
- [US-016](US-016-Automated-Risk-Assessment-P3-Random.md) (P3 random 2% rule consumes this configuration)

## Traceability

### Source Jira Issues

- IMTA-5891
