# US-053: Create Import Record at Point of Matching

## Summary

As an EU Imports Caseworker,
I want to create an Import Record directly when confirming a match,
So that I don't have to navigate away to create it separately.

## Description

When a candidate match ([US-052](US-052-Review-Resolve-Candidate-Matches.md)) is confirmed, PIMS offers to create the resulting Import Record directly from the Match Record, carrying across identifying fields from the Importer Notification and marking the source ITAHC as linked.

## Acceptance Criteria

- **AC-1 (Import Record creation offered at confirmation):** When an EU Imports Caseworker confirms a Match Record as a valid match and no Import Record is yet linked, PIMS offers the option to create the Import Record directly from the Match Record.

- **AC-2 (Choice of address source):** When creating the Import Record, the caseworker can choose, per address, whether to source it from the ITAHC/DOCOM or from the Importer Notification.

- **AC-3 (Fields copied on creation):** The Import Record is created from the ITAHC/DOCOM data, with the following fields additionally copied from the Importer Notification: Notification Reference Number, CPH Number, Notification Submission Date, Importing from a Charity flag, the Importer's Permanent Destination / Final Destination address (unique to the Importer Notification), and — where the Importer Notification indicates a charity consignor — the Second Consignor (charity) name and address fields.

- **AC-4 (ITAHC marked as linked):** When an Import Record is created from an ITAHC via matching, PIMS sets a "Related Import Record" flag on the ITAHC to true.

- **AC-5 (Manual Import Record Type picklist):** When manually creating an Import Record outside of the matching flow, the Import Record Type picklist offers ITAHC, CHEDA and CHEDP only; DOCOM is not offered as a manually-selectable type, since DOCOM-linked Import Records are created automatically on receipt ([US-010](US-010-Auto-Create-Import-Record-From-DOCOM.md)) or via this matching flow.

## Business Rules

- None additional.

## Dependencies

- [US-052](US-052-Review-Resolve-Candidate-Matches.md) (the confirmed match this story acts on)
- [US-001](US-001-Manage-Import-Record.md), [US-002](US-002-Manage-ITAHC.md)

## Traceability

### Source Jira Issues

- IMTA-7592
- IMTA-7648
- IMTA-7719
- IMTA-7720
- IMTA-7778
