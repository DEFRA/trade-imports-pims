# Epic: Place of Origin Trust Level

## Purpose

Enable caseworkers to manage Places of Origin and allow PIMS to automatically maintain Trust Levels (Gold/Bronze) based on post-import check outcomes, to support risk-based inspection decisions.

## Business Value

- Trusted, repeatedly compliant places of origin receive reduced inspection frequency (1-in-10), reducing unnecessary inspections.
- Non-compliant or unknown places of origin receive increased inspection frequency, protecting UK biosecurity.
- Transparent, auditable trust level decisions with full counter history.

## Capability Description

PIMS maintains a Place of Origin registry. Each Place of Origin has a Trust Level (Gold or Bronze) and a set of counters tracking:

- Number of consecutive satisfactory outcomes (used for Gold promotion)
- Number of Import Records linked to the Place of Origin
- Number of Import Records since the last post-import check

Rules govern automatic Trust Level transitions:
- New Places of Origin default to Bronze.
- 3 consecutive satisfactory outcomes promote a Place of Origin to Gold.
- An unsatisfactory outcome resets the consecutive count to 0.
- A caseworker can manually lock a Place of Origin to Bronze (with a mandatory reason).
- Gold Trust Level can be revoked to Bronze following an unsatisfactory outcome (caseworker decision).
- Counter changes are audited with reasons via the Counter History entity.

## Functional Scope

- Create and update Places of Origin
- Search for a Place of Origin by name or postcode
- Link a Place of Origin to an Import Record (lookup with Country of Origin auto-population)
- Update the Place of Origin on an existing Import Record (with counter adjustment)
- View related Import Records from a Place of Origin
- Automatic Trust Level maintenance (Bronze default, Gold promotion, Bronze demotion)
- Lock to Bronze with mandatory reason and audit record
- Unlock from Bronze with mandatory reason and restoration of previous Trust Level
- Revoke Gold Trust Level to Bronze after unsatisfactory outcome
- Defer post-import check counter when manual inspection scheduled early
- Full counter history tracking

## Associated User Stories

| Story                                                                       | Title                                                |
| --------------------------------------------------------------------------- | ---------------------------------------------------- |
| [US-017](../user-stories/US-017-Manage-Place-of-Origin.md)                  | Manage Place of Origin                               |
| [US-018](../user-stories/US-018-Place-of-Origin-Trust-Level-Maintenance.md) | Place of Origin Trust Level Maintenance              |
| [US-019](../user-stories/US-019-Lock-Place-of-Origin-to-Bronze.md)          | Lock Place of Origin to Bronze                       |
| [US-020](../user-stories/US-020-Update-Place-of-Origin-on-Import-Record.md) | Update Place of Origin on Import Record              |
| [US-021](../user-stories/US-021-Revoke-Gold-Trust-Level.md)                 | Revoke Gold Trust Level After Unsatisfactory Outcome |
| [US-022](../user-stories/US-022-Defer-Post-Import-Check-Counter.md)         | Defer Post Import Check Counter                      |

## Source Jira Issues

IMTA-5885, IMTA-5886, IMTA-5887, IMTA-5890, IMTA-5984, IMTA-6666, IMTA-6668, IMTA-6669, IMTA-6677, IMTA-6680, IMTA-6950
