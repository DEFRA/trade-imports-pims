# US-006: Receive Importer Notification from IPAFFS

## Summary

As an EU Imports Caseworker,  
I want PIMS to automatically receive Importer Notifications from IPAFFS when submitted by an importer,  
So that PIMS can create or update the implemented notification record, support matching to related certificates and Import Records, and commence a risk assessment.

## Description

PIMS receives Importer Notifications from IPAFFS via an Azure Service Bus Queue. Messages are triggered by IPAFFS when a notification status changes to Submitted, Amended or Cancelled. PIMS processes each message to create or update the corresponding Importer Notification record. Failed messages are routed to the Dead Letter Queue.

IPAFFS notification types received by PIMS: **CVEDA**, **CVEDP**, **CED**, **IMP**. The full set of IPAFFS notification statuses is: DRAFT, SUBMITTED, VALIDATED, REJECTED, IN_PROGRESS, AMEND, MODIFY, REPLACED, CANCELLED, DELETED. PIMS processes messages for the statuses relevant to caseworker activity (Submitted, AMEND/MODIFY, Cancelled). Importer Notifications also carry `replaces` and `replacedBy` reference fields supporting amendment chain tracking, parallel to the TRACES ITAHC replacement model.

The Importer Notification entity is distinct from the legacy Import Notification concept ([US-003](US-003-Manage-Import-Notification.md)). For the PIMS form and security role configuration related to viewing these records, see [US-044](US-044-View-Importer-Notification.md).

The implemented IPAFFS flow uses the Importer Notification as the inbound entity and supports downstream Import Record creation or update workflows from that data. Earlier spike wording that described IPAFFS as creating ITAHC records is treated as legacy wording rather than the implemented solution shape.

Related records (e.g. Additional Permanent Addresses) that are removed in an IPAFFS update are retained in PIMS as active records, as they may already be referenced by Import Records. IPAFFS commodity identifiers are translated to the D365 commodity classification using Commodity Type Mapping to support downstream case processing.

## Acceptance Criteria

- [x] **AC-1:** When an Importer Notification in IPAFFS is created or updated with status Submitted, Amended or Cancelled, IPAFFS sends a JSON message to PIMS within 30 minutes.

- [x] **AC-2:** When an Importer Notification message is received from IPAFFS, PIMS creates or updates the corresponding Importer Notification record with all mapped attributes. Field updates are reflected on the record; cleared fields are captured in the audit history rather than overwriting data.

- [x] **AC-3:** When an IPAFFS update removes a previously present related record (e.g. an Additional Permanent Address), the equivalent PIMS record remains active and is not deleted.

- [x] **AC-4:** When an IPAFFS update modifies a related record, the change is reflected in the equivalent PIMS record.

- [x] **AC-5:** When an IPAFFS update introduces a new related record, a new equivalent PIMS record is created.

- [~] **AC-6:** If the processing of an inbound IPAFFS message fails, the message is placed on the Dead Letter Queue for manual investigation.

- [x] **AC-7:** When IPAFFS commodity identifiers are used for downstream Import Record processing, PIMS translates them to the D365 commodity classification using Commodity Type Mapping.

## Business Rules

- [BR-029](../business-rules.md#br-029) — Related records retained on PIMS even when removed from IPAFFS update
- [BR-033](../business-rules.md#br-033) — Failed IPAFFS messages go to Dead Letter Queue
- [BR-034](../business-rules.md#br-034) — IPAFFS commodity identifiers translated to D365 commodity classification

## Dependencies

- Azure Service Bus Queue provisioned (see [assumptions-and-constraints.md](../assumptions-and-constraints.md) DEP-004)
- IPAFFS JSON message schema agreed (DEP-002)
- [US-001](US-001-Manage-Import-Record.md) (Importer Notification processing may support downstream Import Record creation or update)
- [US-044](US-044-View-Importer-Notification.md) (Caseworker view of Importer Notification records)

## Traceability

### Source Jira Issues

- IMTA-5862
- IMTA-5864
- IMTA-7222

### Original Links

- IMTA-5862
- IMTA-5864
- IMTA-7222
## Implementation Traceability

### Plugins
- None evidenced in this review.

### Web Resources
- None evidenced in this review.

### Shared Libraries
- None evidenced in this review.

### Solution Components
- src/solutions/defra_Imports/src/Workflows/

## Implementation Confidence

High

## Conformance Snapshot (2026-07-22)

- Status: ⚠️ Partially Implemented
- Conflicts/Gaps: AC-6 (Dead Letter Queue handling) partially evident through business rule reference only

## Acceptance Criteria Conformance

| Acceptance Criterion | Status                  | Evidence                                                                                          |
| -------------------- | ----------------------- | ------------------------------------------------------------------------------------------------- |
| AC-1                 | ✅ Implemented           | Importer Notification entity with IPAFFS-specific fields (`defraimp_ipaffsid`)                    |
| AC-2                 | ✅ Implemented           | Workflow src/solutions/defra_Imports/src/Workflows/ creates/updates Importer Notification records |
| AC-3                 | ✅ Implemented           | Related records (e.g., Additional Permanent Addresses) retained on entity                         |
| AC-4                 | ✅ Implemented           | Changes to related records reflected in PIMS records                                              |
| AC-5                 | ✅ Implemented           | New related records created via Importer Notification workflow                                    |
| AC-6                 | ⚠️ Partially Implemented | Failed messages routing to Dead Letter Queue mentioned in business rules                          |
| AC-7                 | ✅ Implemented           | Commodity Type Mapping entity (`defraimp_commoditytypemapping`) for IPAFFS-to-D365 translation    |
