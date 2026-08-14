# PIMS Requirements Baseline

**Post Import Management System** — Consolidated requirements for Defra EU Imports risk assessment and post-import check management.

PIMS is a Dynamics 365 Model-Driven App used by EU Imports caseworkers to receive, risk-assess, and schedule post-import checks on animal and commodity consignments entering the UK from the EU. It integrates with TRACES Classic (ITAHC/DOCOM) and IPAFFS (Importer Notifications) via Azure Integration Services.

---

## About this baseline

This site is the consolidated, deduplicated and traceable requirements baseline for PIMS. It was reconstructed from the original requirement records (referenced throughout as `IMTA-nnnn`) and from the current implementation, and consolidates 65 source records into 47 user stories.

Where the source records and the implementation disagreed, **the implementation was taken as the source of truth**. Statements that could not be evidenced in either are marked explicitly as not specified.

---

## How to use this site

A reasonable reading order for someone new to PIMS:

1. **[Glossary](requirements/glossary.md)** — settle the terminology first; several terms have legacy synonyms still visible in the application.
2. **[User Stories](requirements/user-stories/index.md)** — the detailed specification, one story per capability, each with acceptance criteria.
3. **[Business Rules](requirements/business-rules.md)** — the numbered decision logic (BR-001 to BR-035) that the stories depend on.
4. **[Process Flows](requirements/diagrams/process-flows.md)** and **[Sequence Diagrams](requirements/diagrams/sequence-diagrams.md)** — how the capabilities fit together, including failure paths.
5. **[Implementation Conformance Matrix](requirements/implementation-conformance-matrix.md)** — which requirements are evidenced in the current implementation, and which are only partially evidenced.

---

## Contents

| Section                                                          | Contents                                                     |
| ---------------------------------------------------------------- | ------------------------------------------------------------ |
| [Vision & Scope](requirements/vision-and-scope.md)               | Business objectives, scope, stakeholders, success criteria   |
| [Glossary](requirements/glossary.md)                             | Canonical terminology and legacy synonyms                    |
| [Business Rules](requirements/business-rules.md)                 | Numbered business rules BR-001 to BR-035                     |
| [Non-Functional Requirements](requirements/non-functional-requirements.md) | Security, audit, performance and compliance requirements |
| [Assumptions & Constraints](requirements/assumptions-and-constraints.md) | Assumptions, dependencies and open confirmations      |
| [Models & Flows](requirements/diagrams/domain-model.md)          | Domain model, process flows, sequence and context diagrams   |
| [Epics](requirements/epics/index.md)                             | 9 epics grouping all 47 user stories                         |
| [User Stories](requirements/user-stories/index.md)               | 47 consolidated user stories                                 |
| [Conformance Matrix](requirements/implementation-conformance-matrix.md) | Implementation evidence for every story                |

---

## Epics at a Glance

| Epic                                                                                              | Stories |
| ------------------------------------------------------------------------------------------------- | ------- |
| [Consignment Record Management](requirements/epics/EPIC-Consignment-Record-Management.md)         | 12      |
| [Reporting & Analytics](requirements/epics/EPIC-Reporting-and-Analytics.md)                       | 7       |
| [External System Integration](requirements/epics/EPIC-External-System-Integration.md)             | 6       |
| [Risk Assessment & Business Rules](requirements/epics/EPIC-Risk-Assessment-and-Business-Rules.md) | 6       |
| [Place of Origin Trust Level](requirements/epics/EPIC-Place-of-Origin-Trust-Level.md)             | 6       |
| [Audit & Compliance](requirements/epics/EPIC-Audit-and-Compliance.md)                             | 4       |
| [Team & Geographic Assignment](requirements/epics/EPIC-Team-and-Geographic-Assignment.md)         | 3       |
| [Post Import Check Management](requirements/epics/EPIC-Post-Import-Check-Management.md)           | 3       |
| [Import Query Management](requirements/epics/EPIC-Import-Query-Management.md)                     | 1       |

[US-021](requirements/user-stories/US-021-Revoke-Gold-Trust-Level.md) appears in two epics, so the column above totals 48 against 47 distinct stories.

---

## Status and currency

- This baseline was reconstructed from the implementation and the surviving requirement records. None of the original authors were available to consult.
- `IMTA-nnnn` identifiers refer to records in a retired internal tracker and are retained for traceability only; they are not resolvable links.
- Evidence paths such as `src/solutions/...` refer to files in this repository.

---

© Crown copyright 2026, Defra. This documentation is licensed under the [Open Government Licence v3.0](https://github.com/DEFRA/trade-imports-pims/blob/main/LICENCE).
