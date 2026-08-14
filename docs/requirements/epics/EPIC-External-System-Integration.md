# Epic: External System Integration

## Purpose

Automate the receipt of health certificate data from TRACES Classic (ITAHC, DOCOM) and importer notification data from IPAFFS into PIMS, eliminating manual re-keying and enabling timely risk assessment.

## Business Value

- Caseworkers save significant time previously spent manually entering data from PDF certificates and email notifications.
- Time saved can be redirected to risk assessment activities that reduce biological risk.
- Consignments are processed faster and more consistently.
- Failed messages are captured for investigation, preventing silent data loss.

## Capability Description

PIMS integrates with two external systems:

1. **TRACES Classic** — sends ITAHC and DOCOM health certificate data to PIMS via Azure Integration Services (Service Bus, Logic Apps). Associated workflows create PIMS certificate records, support downstream Import Record creation, and require explicit failed-receipt handling when processing does not complete successfully.
2. **IPAFFS** — sends Importer Notification data to PIMS via Azure Service Bus when an importer submits, amends or cancels a pre-notification. PIMS creates or updates the implemented Importer Notification entity accordingly, supports downstream Import Record creation/update workflows from that data, and routes failed messages to a Dead Letter Queue.

## Functional Scope

- Provision Azure Service Bus Queues for TRACES and IPAFFS inbound channels
- Receive and process ITAHC and DOCOM data from TRACES Classic within 30 minutes
- Automatically create ITAHC and DOCOM records in PIMS from TRACES data
- Automatically create a linked Import Record when an ITAHC or DOCOM is received from TRACES
- Receive and process Importer Notification data from IPAFFS (Submitted, Amended, Cancelled) within 30 minutes
- Create or update Importer Notification records in PIMS from IPAFFS data
- Translate IPAFFS commodity identifiers to D365 commodity classification using Commodity Type Mapping
- Preserve related records on PIMS even when the IPAFFS update removes them (see BR-029)
- Route failed IPAFFS messages to the Dead Letter Queue (BR-033)
- Capture, expose and support controlled reprocessing of failed TRACES receipts (BR-035)

## Associated User Stories

| Story                                                                         | Title                                     |
| ----------------------------------------------------------------------------- | ----------------------------------------- |
| [US-006](../user-stories/US-006-Receive-Importer-Notification-From-IPAFFS.md) | Receive Importer Notification from IPAFFS |
| [US-007](../user-stories/US-007-Receive-ITAHC-From-TRACES.md)                 | Receive ITAHC from TRACES Classic         |
| [US-008](../user-stories/US-008-Receive-DOCOM-From-TRACES.md)                 | Receive DOCOM from TRACES Classic         |
| [US-009](../user-stories/US-009-Auto-Create-Import-Record-From-ITAHC.md)      | Auto-Create Import Record from ITAHC      |
| [US-010](../user-stories/US-010-Auto-Create-Import-Record-From-DOCOM.md)      | Auto-Create Import Record from DOCOM      |
| [US-047](../user-stories/US-047-Manage-Failed-TRACES-Receipts.md)             | Manage Failed TRACES Receipts             |

## Source Jira Issues

IMTA-5862, IMTA-5864, IMTA-6598, IMTA-6599, IMTA-6600, IMTA-6601, IMTA-6626, IMTA-7201, IMTA-7222, IMTA-7240
