# US-050: Configure the Matching Algorithm

## Summary

As an EU Imports Administrator,
I want to configure the fields and weightings used by PIMS's automated matching algorithm,
So that match accuracy can be tuned without a code change.

## Description

[US-051](US-051-Automated-Matching.md) calculates a weighted percentage match score between an ITAHC/DOCOM and an Importer Notification. The fields considered in that calculation, and their relative weighting, are held in a dedicated configuration entity rather than hard-coded, so an EU Imports Administrator can adjust them as matching accuracy is tuned.

## Acceptance Criteria

- **AC-1 (Configuration entity):** PIMS holds matching-field configuration as a set of records, each with a Field Name and a Weighting. Weighting is a positive decimal greater than zero; PIMS normalises the configured weightings (dividing each by the sum of all active weightings) before applying them in the weighted mean calculation ([BR-053](../business-rules.md#br-053)), so weightings need not sum to any fixed total.

- **AC-2 (Add a field):** An EU Imports Administrator with PIMS Administrator permissions can add a new field to the matching algorithm from the supported field list and assign it a weighting. The maximum number of fields that can be configured is subject to confirmation.

- **AC-3 (Remove a field):** An EU Imports Administrator with PIMS Administrator permissions can remove a field from the matching algorithm. A minimum number of configured fields is enforced so the algorithm cannot be left without enough fields to produce a meaningful score; the exact minimum is subject to confirmation.

- **AC-4 (Amend a weighting):** An EU Imports Administrator with PIMS Administrator permissions can amend the weighting of an existing configured field.

## Business Rules

- [BR-053](../business-rules.md#br-053) — Matching algorithm weighted mean calculation

## Dependencies

- [US-051](US-051-Automated-Matching.md) (consumes this configuration)

## Traceability

### Source Jira Issues

- IMTA-7595
