# US-008: Receive DOCOM from TRACES Classic

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically receive DOCOMs from TRACES Classic,  
So that I can save the effort of manually re-keying certificate data and redirect that time to risk reduction activities.

## Description

DOCOMs created in TRACES Classic are received and automatically created in PIMS via Azure Integration Services, mirroring the ITAHC integration ([US-007](US-007-Receive-ITAHC-From-TRACES.md)). DOCOM records created this way are distinguishable from manually created records by being associated with a TRACES service account.

**Note:** The specific DOCOM field mapping schema is not specified in this baseline. Attribute-level mapping traceability remains incomplete until that schema is confirmed.

## Acceptance Criteria

- **AC-1:** DOCOMs created in TRACES Classic are received and automatically created in PIMS within 30 minutes of the time that creation notification emails would have been received by the CIT team.

- **AC-2:** DOCOMs are automatically created in PIMS with the fields specified in the agreed field mapping schema. The attribute-level field list is not specified in this baseline — see [assumptions-and-constraints.md](../assumptions-and-constraints.md) DEP-001.

## Business Rules

None additional to the DOCOM entity rules.

## Dependencies

- TRACES Classic integration API/message format agreed (DEP-001)
- Azure Service Bus Queue and Logic App provisioned (DEP-004)
- [US-047](US-047-Manage-Failed-TRACES-Receipts.md) (Failed TRACES receipts are captured and reprocessed when inbound DOCOM handling fails)
- [US-010](US-010-Auto-Create-Import-Record-From-DOCOM.md) (Auto-create Import Record triggered when DOCOM received from TRACES)

## Traceability

### Source Jira Issues

- IMTA-6599
