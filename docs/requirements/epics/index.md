# Epic Catalogue

This page lists all 9 epics in the PIMS requirements baseline. Each epic groups related user stories around a coherent area of system functionality.

**Total:** 9 epics · 47 user stories ([US-021](../user-stories/US-021-Revoke-Gold-Trust-Level.md) appears in two epics)

---

| Epic                                                                             | Description                                                                                                                                                                                                | Stories |
| -------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------- |
| [Consignment Record Management](./EPIC-Consignment-Record-Management.md)         | Enable EU Imports Caseworkers to create, manage and complete all types of consignment health certificate records and the Import Record that links them, throughout the full case lifecycle.                | 12      |
| [External System Integration](./EPIC-External-System-Integration.md)             | Automate the receipt of health certificate data from TRACES Classic (ITAHC, DOCOM) and importer notification data from IPAFFS into PIMS, eliminating manual re-keying and enabling timely risk assessment. | 6       |
| [Risk Assessment & Business Rules](./EPIC-Risk-Assessment-and-Business-Rules.md) | Enable EU Imports Business Rules Admins to configure and maintain the rules that PIMS uses to automatically classify Import Records by risk level and determine whether a post-import check is required.   | 6       |
| [Place of Origin Trust Level](./EPIC-Place-of-Origin-Trust-Level.md)             | Enable caseworkers to manage Places of Origin and allow PIMS to automatically maintain Trust Levels (Gold/Bronze) based on post-import check outcomes, to support risk-based inspection decisions.         | 6       |
| [Post Import Check Management](./EPIC-Post-Import-Check-Management.md)           | Enable caseworkers to manage the lifecycle of Post Import Checks from flagging through to outcome recording, including manual override capabilities.                                                       | 3       |
| [Import Query Management](./EPIC-Import-Query-Management.md)                     | Enable caseworkers to raise, track, assign and resolve formal queries against Import Records, supporting communication with importers and third parties.                                                   | 1       |
| [Team & Geographic Assignment](./EPIC-Team-and-Geographic-Assignment.md)         | Enable Import Records, ITAHCs and DOCOMs to be assigned to the correct geographic regional team for processing and risk assessment.                                                                        | 3       |
| [Reporting & Analytics](./EPIC-Reporting-and-Analytics.md)                       | Provide caseworkers, team leaders and data analysts with operational dashboards and exportable reports to monitor EU imports activity and demonstrate Defra policy compliance.                             | 7       |
| [Audit & Compliance](./EPIC-Audit-and-Compliance.md)                             | Ensure that all case decisions, field changes, inspection decisions and counter changes in PIMS are fully audited and traceable to demonstrate regulatory compliance.                                      | 4       |

---

## Epic Detail

### Consignment Record Management

> [EPIC-Consignment-Record-Management](./EPIC-Consignment-Record-Management.md)

| Story                                                                       | Title                                   |
| --------------------------------------------------------------------------- | --------------------------------------- |
| [US-001](../user-stories/US-001-Manage-Import-Record.md)                    | Manage Import Record                    |
| [US-002](../user-stories/US-002-Manage-ITAHC.md)                            | Manage ITAHC                            |
| [US-003](../user-stories/US-003-Manage-Import-Notification.md)              | Manage Import Notification              |
| [US-004](../user-stories/US-004-Manage-DOCOM.md)                            | Manage DOCOM                            |
| [US-005](../user-stories/US-005-Manage-CVED.md)                             | Manage CVED                             |
| [US-029](../user-stories/US-029-Document-Attachment.md)                     | Document Attachment                     |
| [US-030](../user-stories/US-030-IV65-Due-Date-Calculation.md)               | IV65 Response Due Date Calculation      |
| [US-031](../user-stories/US-031-Completion-Date-Recording.md)               | Completion Date Recording               |
| [US-032](../user-stories/US-032-Warble-Fly-Declaration-Date.md)             | Warble Fly Treatment Declaration Date   |
| [US-033](../user-stories/US-033-Quick-Create-Import-Record.md)              | Quick Create Import Record              |
| [US-044](../user-stories/US-044-View-Importer-Notification.md)              | View Importer Notification in PIMS      |
| [US-046](../user-stories/US-046-Match-Inbound-Records-to-Import-Records.md) | Match Inbound Records to Import Records |

---

### External System Integration

> [EPIC-External-System-Integration](./EPIC-External-System-Integration.md)

| Story                                                                         | Title                                     |
| ----------------------------------------------------------------------------- | ----------------------------------------- |
| [US-006](../user-stories/US-006-Receive-Importer-Notification-From-IPAFFS.md) | Receive Importer Notification from IPAFFS |
| [US-007](../user-stories/US-007-Receive-ITAHC-From-TRACES.md)                 | Receive ITAHC from TRACES Classic         |
| [US-008](../user-stories/US-008-Receive-DOCOM-From-TRACES.md)                 | Receive DOCOM from TRACES Classic         |
| [US-009](../user-stories/US-009-Auto-Create-Import-Record-From-ITAHC.md)      | Auto-Create Import Record from ITAHC      |
| [US-010](../user-stories/US-010-Auto-Create-Import-Record-From-DOCOM.md)      | Auto-Create Import Record from DOCOM      |
| [US-047](../user-stories/US-047-Manage-Failed-TRACES-Receipts.md)             | Manage Failed TRACES Receipts             |

---

### Risk Assessment & Business Rules

> [EPIC-Risk-Assessment-and-Business-Rules](./EPIC-Risk-Assessment-and-Business-Rules.md)

| Story                                                                   | Title                                       |
| ----------------------------------------------------------------------- | ------------------------------------------- |
| [US-011](../user-stories/US-011-Manage-Commodity-Risk-Levels.md)        | Manage Commodity Risk Levels                |
| [US-012](../user-stories/US-012-Manage-Gold-Bronze-Commodities.md)      | Manage Gold/Bronze Commodities              |
| [US-013](../user-stories/US-013-Manage-Inspection-Coverage-Rules.md)    | Manage Inspection Coverage Rules            |
| [US-014](../user-stories/US-014-Automated-Risk-Assessment-P1.md)        | Automated Risk Assessment — P1 Consignments |
| [US-015](../user-stories/US-015-Automated-Risk-Assessment-P2.md)        | Automated Risk Assessment — P2 Random 10%   |
| [US-016](../user-stories/US-016-Automated-Risk-Assessment-P3-Random.md) | Automated Risk Assessment — P3 Random 2%    |

---

### Place of Origin Trust Level

> [EPIC-Place-of-Origin-Trust-Level](./EPIC-Place-of-Origin-Trust-Level.md)

| Story                                                                       | Title                                                |
| --------------------------------------------------------------------------- | ---------------------------------------------------- |
| [US-017](../user-stories/US-017-Manage-Place-of-Origin.md)                  | Manage Place of Origin                               |
| [US-018](../user-stories/US-018-Place-of-Origin-Trust-Level-Maintenance.md) | Place of Origin Trust Level Maintenance              |
| [US-019](../user-stories/US-019-Lock-Place-of-Origin-to-Bronze.md)          | Lock Place of Origin to Bronze                       |
| [US-020](../user-stories/US-020-Update-Place-of-Origin-on-Import-Record.md) | Update Place of Origin on Import Record              |
| [US-021](../user-stories/US-021-Revoke-Gold-Trust-Level.md)                 | Revoke Gold Trust Level After Unsatisfactory Outcome |
| [US-022](../user-stories/US-022-Defer-Post-Import-Check-Counter.md)         | Defer Post Import Check Counter                      |

---

### Post Import Check Management

> [EPIC-Post-Import-Check-Management](./EPIC-Post-Import-Check-Management.md)

| Story                                                                 | Title                                                |
| --------------------------------------------------------------------- | ---------------------------------------------------- |
| [US-021](../user-stories/US-021-Revoke-Gold-Trust-Level.md)           | Revoke Gold Trust Level After Unsatisfactory Outcome |
| [US-023](../user-stories/US-023-Post-Import-Check-Management.md)      | Post Import Check Management                         |
| [US-024](../user-stories/US-024-Manual-Post-Import-Check-Override.md) | Manual Post Import Check Override                    |

---

### Import Query Management

> [EPIC-Import-Query-Management](./EPIC-Import-Query-Management.md)

| Story                                                       | Title                   |
| ----------------------------------------------------------- | ----------------------- |
| [US-025](../user-stories/US-025-Import-Query-Management.md) | Import Query Management |

---

### Team & Geographic Assignment

> [EPIC-Team-and-Geographic-Assignment](./EPIC-Team-and-Geographic-Assignment.md)

| Story                                                                 | Title                             |
| --------------------------------------------------------------------- | --------------------------------- |
| [US-026](../user-stories/US-026-Geographic-Team-Assignment.md)        | Geographic Team Assignment        |
| [US-027](../user-stories/US-027-Auto-Assign-ITAHC-DOCOM-to-Region.md) | Auto-Assign ITAHC/DOCOM to Region |
| [US-034](../user-stories/US-034-Manage-APHA-Region.md)                | Manage APHA Region                |

---

### Reporting & Analytics

> [EPIC-Reporting-and-Analytics](./EPIC-Reporting-and-Analytics.md)

| Story                                                                     | Title                                              |
| ------------------------------------------------------------------------- | -------------------------------------------------- |
| [US-037](../user-stories/US-037-Daily-Huddle-Dashboard.md)                | EU Imports — Daily Huddle Stats Dashboard          |
| [US-038](../user-stories/US-038-Daily-Stats-Dashboard.md)                 | EU Imports — Daily Stats Dashboard                 |
| [US-039](../user-stories/US-039-IRMS-Stats-Dashboard.md)                  | EU Imports — Daily IRMS Stats Dashboard            |
| [US-040](../user-stories/US-040-Border-Control-Metrics-Dashboard.md)      | EU Imports — Border Control Metrics Dashboard      |
| [US-041](../user-stories/US-041-APHA-Border-Control-Metrics-Dashboard.md) | EU Imports — APHA Border Control Metrics Dashboard |
| [US-042](../user-stories/US-042-FAET-Weekly-Report.md)                    | Farming Analysis and Evidence Team Weekly Report   |
| [US-043](../user-stories/US-043-Inspection-Coverage-Report.md)            | Inspection Coverage Audit Report                   |

---

### Audit & Compliance

> [EPIC-Audit-and-Compliance](./EPIC-Audit-and-Compliance.md)

| Story                                                                | Title                            |
| -------------------------------------------------------------------- | -------------------------------- |
| [US-028](../user-stories/US-028-Generate-Unique-Reference-Number.md) | Generate Unique Reference Number |
| [US-035](../user-stories/US-035-Audit-Import-Records.md)             | Audit Import Records             |
| [US-036](../user-stories/US-036-Audit-Post-Import-Checks.md)         | Audit Post Import Checks         |
| [US-045](../user-stories/US-045-Counter-History-Tracking.md)         | Counter History Tracking         |

---

## Roll-up Statistics

| Metric                      | Value                                                           |
| --------------------------- | --------------------------------------------------------------- |
| Total epics                 | 9                                                               |
| Total user stories          | 47                                                              |
| Largest epic                | Consignment Record Management (12 stories)                      |
| Smallest epic               | Import Query Management (1 story)                               |
| Average stories per epic    | 5.3                                                             |
| Stories shared across epics | 1 ([US-021](../user-stories/US-021-Revoke-Gold-Trust-Level.md)) |
| Source records consolidated | 65 → 47 stories                                                 |

---

*See also: [User Story Catalogue](../user-stories/index.md) · [Business Rules](../business-rules.md) · [Implementation Conformance Matrix](../implementation-conformance-matrix.md)*
