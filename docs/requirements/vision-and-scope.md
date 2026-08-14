# Vision and Scope

## 1. Business Objectives

1. Replace manual, paper-based and spreadsheet-driven EU imports case management processes with a centralised, auditable digital system.
2. Ensure consistent application of Defra policy for risk-based post-import checks on animal and commodity consignments entering the UK from the EU.
3. Reduce the administrative burden on EU Imports Caseworkers by automating risk assessment, inspection flagging and counter management.
4. Provide real-time operational visibility for team leaders and managers through dashboards and reports.
5. Maintain full auditability of all case decisions for regulatory compliance purposes.
6. Integrate with external veterinary and import notification systems (TRACES Classic, IPAFFS) to eliminate duplicate data entry.

---

## 2. Product Vision

**PIMS** (Post Import Management System) is a Dynamics 365-based case management platform that:

- Receives consignment health certificate data automatically from TRACES Classic (ITAHC, DOCOM) and importer notification data from IPAFFS.
- Uses the implemented Importer Notification processing flow to support downstream Import Record creation, matching and risk assessment activities.
- Enables caseworkers to create, manage and complete Import Records across the full lifecycle from initial triage through to post-import check outcome.
- Applies configurable business rules to automatically determine the risk level and post-import check requirement for every consignment.
- Provides team leaders, managers and reporting teams with dashboards and exportable reports to meet Defra operational and policy reporting requirements.

---

## 3. In-Scope Capabilities

| #   | Capability                                                                                                                      |
| --- | ------------------------------------------------------------------------------------------------------------------------------- |
| 1   | Import Record creation, management and lifecycle                                                                                |
| 2   | ITAHC, DOCOM and CVED record management                                                                                         |
| 3   | Import Notification legacy terminology handling and Importer Notification management                                            |
| 4   | Automated inbound integration with TRACES Classic (ITAHC, DOCOM)                                                                |
| 5   | Automated inbound integration with IPAFFS (Importer Notifications)                                                              |
| 6   | Automated Import Record creation from TRACES-sourced health certificates and implemented IPAFFS Importer Notification workflows |
| 7   | Configurable commodity risk level rules (Country/Commodity/Risk Level)                                                          |
| 8   | Gold/Bronze commodity rules maintenance                                                                                         |
| 9   | Place of Origin management and trust level (Gold/Bronze) maintenance                                                            |
| 10  | Automated risk assessment (P1, P2, P3) and post-import check flagging                                                           |
| 11  | Random inspection coverage rules (2% P3, 10% P2)                                                                                |
| 12  | Manual override of post-import check requirement                                                                                |
| 13  | Post Import Check record creation and outcome recording                                                                         |
| 14  | Import Query management (creation, assignment, resolution)                                                                      |
| 15  | Geographic team assignment (North, South, West; Scotland, Wales, England)                                                       |
| 16  | Document/file attachment to Import Records                                                                                      |
| 17  | IV65 response due date calculation                                                                                              |
| 18  | Unique reference number generation                                                                                              |
| 19  | Warble fly treatment declaration tracking                                                                                       |
| 20  | APHA Region management                                                                                                          |
| 21  | Operational dashboards (Daily Huddle, Daily Stats, IRMS Stats, Border Control Metrics, APHA Border Control Metrics)             |
| 22  | Farming Analysis and Evidence Team (FAET) weekly report                                                                         |
| 23  | Inspection coverage audit report                                                                                                |
| 24  | Full field-level audit of Import Records and Post Import Checks                                                                 |
| 25  | Counter history tracking for risk assessment and trust level counters                                                           |
| 26  | ITAHC/DOCOM status tracking (Replaced By / Replaces chain)                                                                      |
| 27  | Matching inbound certificates and notifications to candidate Import Records                                                     |
| 28  | TRACES failed-receipt investigation and controlled reprocessing                                                                 |

Business-facing requirements use **Import Record** as the canonical term. Technical schema names and some workflow artefacts may still use `importapplication` / "Import Application".

---

## 4. Out-of-Scope Capabilities

| #   | Item                                   | Rationale                                                    |
| --- | -------------------------------------- | ------------------------------------------------------------ |
| 1   | Outbound data push to TRACES or IPAFFS | No story describes outbound integration                      |
| 2   | Importer self-service portal           | All user stories are internal caseworker operations          |
| 3   | Financial processing or fee collection | No stories reference payment functionality                   |
| 4   | Non-EU imports case management         | Stories are explicitly scoped to EU imports                  |
| 5   | Veterinary certificate issuing         | PIMS records and tracks certificates; it does not issue them |
| 6   | HR / workforce management              | Out of system scope                                          |

---

## 5. Major Stakeholders

| Stakeholder                       | Role                                                                                              |
| --------------------------------- | ------------------------------------------------------------------------------------------------- |
| EU Imports Caseworker             | Primary user; creates and manages Import Records and Queries                                      |
| EU Imports Caseworker Admin       | Manages administrative reference data (APHA Regions)                                              |
| EU Imports Business Rules Admin   | Maintains configurable business rules (risk levels, inspection coverage, Gold/Bronze commodities) |
| EU Imports Team Leader            | Reviews audit trails; monitors team performance                                                   |
| Data Team Member (FAET)           | Runs weekly analytical exports                                                                    |
| CIT Team (TRACES)                 | Receives notification emails from TRACES Classic                                                  |
| IPAFFS System                     | External system that submits Importer Notifications to PIMS                                       |
| TRACES Classic                    | External system that sends health certificate data (ITAHC, DOCOM) to PIMS                         |
| Dynamics 365 System Administrator | Platform-level administration                                                                     |
| Defra Policy Team                 | Sets inspection coverage thresholds and risk policies                                             |

---

## 6. Success Criteria

| #   | Criterion                                                                                                                                                            |
| --- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 1   | ITAHC and DOCOM records are automatically created in PIMS within the agreed processing window of creation in TRACES Classic                                          |
| 2   | Importer Notifications from IPAFFS are received and processed within the agreed processing window                                                                    |
| 3   | Automated risk assessment correctly classifies 100% of Import Records according to configured rules                                                                  |
| 4   | Random inspection coverage (2% P3, 10% P2) is applied consistently and auditably                                                                                     |
| 5   | All field changes on Import Records and Post Import Checks are captured in the audit trail                                                                           |
| 6   | Caseworkers can complete core workflows (create Import Record, link Place of Origin, flag for post-import check) without manual re-keying of health certificate data |
| 7   | Dashboards provide near-real-time operational data without external tooling                                                                                          |
| 8   | Every business decision is traceable to a configurable rule                                                                                                          |

The processing-window and coverage figures behind criteria 1, 2 and 4 are specified once, as measurable targets, in the [Non-Functional Requirements](non-functional-requirements.md) (NFR-PER-\* and NFR-COM-\*). They are not restated here.
