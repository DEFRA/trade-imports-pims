# Epic: Audit and Compliance

## Purpose

Ensure that all case decisions, field changes, inspection decisions and counter changes in PIMS are fully audited and traceable to demonstrate regulatory compliance.

## Business Value

- Defra can demonstrate to regulators and auditors that inspection policy is being applied correctly.
- Any disputed decision can be investigated by examining the full change history.
- Counter history provides transparency into why automated inspection flags were raised.

## Capability Description

PIMS enables full auditability through:

- D365 organisation-level auditing on Import Records and Post Import Checks
- Unique reference number generation for all Import Records
- Counter History entity tracking every increment/decrement/reset of risk and trust level counters

## Functional Scope

- Enable D365 audit log on Import Record and Post Import Check entities and all custom fields
- Generate unique reference numbers for Import Records (format: `GB.{YEAR}.{9-digit-sequence}`)
- Record Counter History for all risk assessment counters (P1/P2/P3 auto-number counters)
- Record Counter History for all Place of Origin trust level counters
- Each Counter History entry records: Import Record, counter type, operation, reason, previous value, current value

## Associated User Stories

| Story                                                                | Title                            |
| -------------------------------------------------------------------- | -------------------------------- |
| [US-028](../user-stories/US-028-Generate-Unique-Reference-Number.md) | Generate Unique Reference Number |
| [US-035](../user-stories/US-035-Audit-Import-Records.md)             | Audit Import Records             |
| [US-036](../user-stories/US-036-Audit-Post-Import-Checks.md)         | Audit Post Import Checks         |
| [US-045](../user-stories/US-045-Counter-History-Tracking.md)         | Counter History Tracking         |

## Source Jira Issues

IMTA-5986, IMTA-6128, IMTA-6132, IMTA-6950
