# US-007: Receive ITAHC from TRACES Classic

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically receive ITAHCs from TRACES Classic,  
So that I can save the effort of manually re-keying certificate data and redirect that time to risk reduction activities.

## Description

ITAHCs created in TRACES Classic are received and automatically created in PIMS via Azure Integration Services. The integration intercepts the notification email mechanism used by the CIT team and routes ITAHC data via an Azure Service Bus Queue into PIMS. ITAHC records created this way are distinguishable from manually created records by being associated with a TRACES service account.

**Note:** The ITAHC field mapping schema is defined in the **Defra - EU Imports - D365 - Entity Schemas** workbook, which is held outside this repository. That workbook provides the complete TRACES-to-D365 attribute-level mapping for both ITAHC and DOCOM. Key mapping details: Consignor Name maps to `defraimp_consignorname` (mandatory); the certificate structure maps to the Consignment Details and Place of Destination form sections; Identification of Animals and Commodity Complements are stored as JSON text fields.

## Acceptance Criteria

- [x] **AC-1:** ITAHCs created in TRACES Classic are received and automatically created in PIMS within 30 minutes of the time that creation notification emails would have been received by the CIT team.

- [x] **AC-2:** ITAHCs are automatically created in PIMS with the fields specified in the TRACES→D365 field mapping schema (now documented in the Entity Schemas workbook — see DEP-001 in [assumptions-and-constraints.md](../assumptions-and-constraints.md)).

## Business Rules

- [BR-027](../business-rules.md#br-027) — ITAHC replacement chain maintained

## Dependencies

- TRACES Classic integration API/message format agreed (DEP-001)
- Azure Service Bus Queue and Logic App provisioned (DEP-004)
- [US-047](US-047-Manage-Failed-TRACES-Receipts.md) (Failed TRACES receipts are captured and reprocessed when inbound ITAHC handling fails)
- [US-009](US-009-Auto-Create-Import-Record-From-ITAHC.md) (Auto-create Import Record triggered when ITAHC received from TRACES)

## Traceability

### Source Jira Issues

- IMTA-6598

### Original Links

- IMTA-6598
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/ITAHC-CreateImportRecordFromITAHC-35DDCA57-47E9-459D-9900-7505B63F87CD.xaml

## Implementation Confidence

Medium

## Conformance Snapshot (2026-07-22)

- Status: ✅ Fully Implemented
- Conflicts/Gaps: Attribute-level field mapping is defined in the external Entity Schemas workbook rather than in this baseline, so mapping traceability is incomplete here.

## Acceptance Criteria Conformance

| Acceptance Criterion | Status        | Evidence                                                                                                                                                                    |
| -------------------- | ------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented | Workflow src/solutions/defra_Imports/src/Workflows/ITAHC-CreateImportRecordFromITAHC-35DDCA57-47E9-459D-9900-7505B63F87CD.xaml auto-creates records from TRACES integration |
| AC-2                 | ✅ Implemented | Field mapping schema configured in workflow; the attribute-level field list is held in the external Entity Schemas workbook                                                  |
