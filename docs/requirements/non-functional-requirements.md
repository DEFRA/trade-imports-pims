# Non-Functional Requirements

Each NFR is labelled as **Explicit** (directly stated in a source story) or **Inferred** (implied by the capability set or regulatory context).

---

## Security

| ID          | Requirement                                                                                                                                                                 | Type     | Source                                                                                                                                                                                                                                                                                                                                                                       |
| ----------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NFR-SEC-001 | User access to PIMS must be controlled by Dynamics 365 Security Roles. Each role must grant the minimum privilege necessary for the user's function.                        | Explicit | IMTA-5863, IMTA-5868, IMTA-5869, IMTA-5870, IMTA-5885, IMTA-5891 |
| NFR-SEC-002 | Inspection Coverage Rules must only be editable by the Dynamics 365 System Administrator role. All other roles must have read-only access.                                  | Explicit | IMTA-5891 AC-2                                                                                                                                                                                                                                                                                                             |
| NFR-SEC-003 | Documents attached to Import Records must not be deletable by business users.                                                                                               | Explicit | IMTA-5913 AC-3                                                                                                                                                                                                                                                                                                             |
| NFR-SEC-004 | Inbound integration endpoints (Azure Service Bus) must be secured using managed identities or shared access signatures; no plaintext credentials in configuration.          | Inferred | IMTA-5864, IMTA-6598, IMTA-6599                                                                                                                                                                                        |
| NFR-SEC-005 | Personal data (importer addresses, CPH numbers, contact details) stored in PIMS must be protected in accordance with UK GDPR. Access must be role-controlled and auditable. | Inferred | IMTA-5869, IMTA-5870                                                                                                                                                                                                                                                     |

---

## Authentication

| ID           | Requirement                                                                                                                                                       | Type     | Source                                                                                                                                                                                                                                                                                                          |
| ------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NFR-AUTH-001 | All user access to PIMS must be authenticated via Azure Active Directory (Entra ID) federated with the Dynamics 365 environment.                                  | Inferred | General D365 platform practice                                                                                                                                                                                                                                                                                  |
| NFR-AUTH-002 | Integration service accounts (TRACES, IPAFFS) must authenticate to PIMS using dedicated service principals with minimal permissions, not shared user credentials. | Inferred | IMTA-6598, IMTA-6599, IMTA-6600, IMTA-6601, IMTA-6661 |

---

## Authorisation

| ID          | Requirement                                                                                                                                                                              | Type     | Source                                                                                                                                                                                                                                                                                                          |
| ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NFR-AUT-001 | PIMS must enforce business-unit-level record ownership, with each Security Role granting access only to records owned by the user's business unit unless explicitly permitted otherwise. | Explicit | IMTA-5868, IMTA-5869, IMTA-5870, IMTA-5885, IMTA-5886 |
| NFR-AUT-002 | In the event of a regional disaster, all geographic teams must be able to view (but not necessarily own) Import Records from other teams.                                                | Explicit | IMTA-5863 AC-2                                                                                                                                                                                                                                                |
| NFR-AUT-003 | The EU Imports Caseworker role must be able to append notes to Import Queries it does not own.                                                                                           | Explicit | IMTA-6185 AC-7                                                                                                                                                                                                                                                |

---

## Auditability

| ID          | Requirement                                                                                                                                                                                                 | Type     | Source                                                           |
| ----------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ---------------------------------------------------------------- |
| NFR-AUD-001 | All changes to updatable fields on Import Records must be captured in the Dynamics 365 audit log, recording the user, date, time and old/new values.                                                        | Explicit | IMTA-5986      |
| NFR-AUD-002 | All changes to updatable fields on Post Import Check records must be captured in the Dynamics 365 audit log.                                                                                                | Explicit | IMTA-6128      |
| NFR-AUD-003 | Every change to the Lock to Bronze field on a Place of Origin must be audited with the user identity, date and time.                                                                                        | Explicit | IMTA-5890 AC-3 |
| NFR-AUD-004 | The system must record a Counter History entry for every increment, decrement or reset of a risk assessment counter (P1/P2/P3) or Place of Origin trust level counter, including the reason for the change. | Explicit | IMTA-6950      |
| NFR-AUD-005 | Audit logs must be retained in accordance with Defra data retention policies.                                                                                                                               | Inferred | Regulatory context                                               |

---

## Availability

| ID          | Requirement                                                                                                                                          | Type     | Source                                                                                                                             |
| ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| NFR-AVA-001 | PIMS must be available during all operational hours of EU Imports caseworker teams. Planned maintenance must be scheduled outside operational hours. | Inferred | Operational context                                                                                                                |
| NFR-AVA-002 | The inbound integration pipeline (Azure Service Bus, Logic Apps) must be available continuously to receive TRACES and IPAFFS messages at any time.   | Inferred | IMTA-5864 AC-1, IMTA-6598 AC-1 |

---

## Performance

| ID          | Requirement                                                                                                                                                      | Type     | Source                                                                                                                                                                                |
| ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NFR-PER-001 | Importer Notifications from IPAFFS must be received and created in PIMS within **30 minutes** of submission.                                                     | Explicit | IMTA-5864 AC-1                                                                                                                      |
| NFR-PER-002 | ITAHC records from TRACES Classic must be received and created in PIMS within **30 minutes** of the time that creation notification emails would have been sent. | Explicit | IMTA-6598 AC-1                                                                                                                      |
| NFR-PER-003 | DOCOM records from TRACES Classic must be received and created in PIMS within **30 minutes** of the time that creation notification emails would have been sent. | Explicit | IMTA-6599 AC-1                                                                                                                      |
| NFR-PER-004 | Risk assessment rules (BR-001 to BR-009) must be evaluated synchronously on save of an Import Record, with no noticeable delay to caseworker operations.         | Inferred | IMTA-5866, IMTA-5892, IMTA-5895 |

---

## Scalability

| ID          | Requirement                                                                                                                    | Type     | Source                                                                                                                                                                                                                                                                                                          |
| ----------- | ------------------------------------------------------------------------------------------------------------------------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NFR-SCA-001 | The Azure Service Bus integration must be able to handle batch receipt of TRACES and IPAFFS messages without message loss.     | Inferred | IMTA-5864, IMTA-6598, IMTA-6599                                                                                                                           |
| NFR-SCA-002 | Reporting dashboards must be capable of displaying accurate metrics as Import Record volumes grow without manual intervention. | Inferred | IMTA-6340, IMTA-6341, IMTA-6343, IMTA-6344, IMTA-6372 |

---

## Resilience

| ID          | Requirement                                                                                                                                                | Type     | Source                                                           |
| ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ---------------------------------------------------------------- |
| NFR-RES-001 | If an inbound message from IPAFFS cannot be processed, it must be placed on the Dead Letter Queue for manual investigation rather than silently discarded. | Explicit | IMTA-5864 AC-3 |
| NFR-RES-002 | The integration pipeline must support re-processing of messages from the Dead Letter Queue without duplicate record creation.                              | Inferred | IMTA-5864 AC-3 |

---

## Accessibility

| ID          | Requirement                                                                                                       | Type     | Source                                   |
| ----------- | ----------------------------------------------------------------------------------------------------------------- | -------- | ---------------------------------------- |
| NFR-ACC-001 | PIMS must meet Dynamics 365's built-in accessibility standards (WCAG 2.1 AA) for all forms, views and dashboards. | Inferred | UK Government accessibility requirements |

---

## Monitoring

| ID          | Requirement                                                                                                                           | Type     | Source                                                                                                                                                                                     |
| ----------- | ------------------------------------------------------------------------------------------------------------------------------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| NFR-MON-001 | The Azure Integration Services pipeline must emit monitoring telemetry to enable detection and alerting on failed message processing. | Inferred | IMTA-5864 AC-3, IMTA-6598, IMTA-6599 |
| NFR-MON-002 | Dead Letter Queue depth must be monitored and alerts raised when messages accumulate.                                                 | Inferred | IMTA-5864 AC-3                                                                                                                           |

---

## Maintainability

| ID          | Requirement                                                                                                                                                                                                | Type     | Source                                                                                                                                                                                                                                             |
| ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NFR-MNT-001 | All configurable business rules (Commodity Risk Levels, Inspection Coverage Rules, Gold/Bronze Commodities, unique reference prefix) must be manageable by authorised business users without code changes. | Explicit | IMTA-5865, IMTA-5888, IMTA-5891, IMTA-6132 |
| NFR-MNT-002 | Teams and team members must replicate successfully into upstream (pre-production and production) environments as part of deployment.                                                                       | Explicit | IMTA-5863 Solution notes                                                                                                                                                                         |

---

## Privacy

| ID          | Requirement                                                                                                                                                            | Type     | Source                                                                                                                   |
| ----------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ------------------------------------------------------------------------------------------------------------------------ |
| NFR-PRV-001 | Personal data of importers (name, address, telephone, email, CPH) stored in PIMS is subject to UK GDPR. Data must not be retained beyond the defined retention period. | Inferred | IMTA-5869, IMTA-5870 |
| NFR-PRV-002 | Access to personal data must be role-controlled and all access events auditable.                                                                                       | Inferred | UK GDPR Article 25 (data protection by design)                                                                           |

---

## Compliance

| ID          | Requirement                                                                                                                                                                | Type     | Source                                                                                                                                                                                |
| ----------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NFR-COM-001 | The 2% random P3 inspection coverage and 10% random P2 inspection coverage must be applied consistently and must be auditable to demonstrate compliance with Defra policy. | Explicit | IMTA-5892, IMTA-5895, IMTA-6658 |
| NFR-COM-002 | All automated risk assessment decisions must be traceable to a specific configurable rule and the rule's value at the time of the decision.                                | Inferred | Regulatory context; IMTA-6950                                                                                                       |
