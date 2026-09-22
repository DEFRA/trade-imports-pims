# US-045: Counter History Tracking

## Summary

As an EU Imports Caseworker,  
I want PIMS to record the reason why a risk assessment counter or Place of Origin counter changes,  
So that I can understand why automated inspection flags were raised or cleared.

## Description

Every increment, decrement, non-increment or reset of a risk assessment counter (P1/P2/P3 auto-number counters) or a Place of Origin trust level counter must be recorded in a Counter History entity. Each entry is associated with the Import Record that triggered the change and includes the reason, operation type and before/after values.

## Acceptance Criteria

- **AC-1 (Record reason for Import Record counter changes):**  
  PIMS records a Counter History entry for every change to a P1/P2/P3 auto-number counter with:
  - Import Record (Lookup — Mandatory)
  - Counter History Type = Auto Number (Option Set — Mandatory)
  - Auto Number (Lookup — Mandatory)
  - Operation (Option Set — Mandatory): Increment / Decrement / Did Not Increment / Did Not Decrement / Set to 0
  - Reason (Option Set — Mandatory)
  - Previous Counter Value (Integer — Mandatory)
  - Current Counter Value (Integer — Mandatory)

- **AC-2 (Record reason for Place of Origin counter changes):**  
  PIMS records a Counter History entry for every change to a Place of Origin trust level counter with:
  - Import Record (Lookup — Mandatory)
  - Counter History Type = Place of Origin (Option Set — Mandatory)
  - Auto Number (Lookup — Mandatory)
  - Operation (Option Set — Mandatory): Increment / Decrement / Did Not Increment / Did Not Decrement / Set to 0
  - Reason (Option Set — Mandatory)
  - Previous Counter Value (Integer — Mandatory)
  - Current Counter Value (Integer — Mandatory)

- **AC-3 (Visibility):**  
  Counter History entries are visible against the Auto Number entity and against the Import Record within the Related tab in PIMS.

## Business Rules

None additional (Counter History is an audit mechanism for existing rules).

## Dependencies

- [US-015](US-015-Automated-Risk-Assessment-P2.md), [US-016](US-016-Automated-Risk-Assessment-P3-Random.md) (P2/P3 counters)
- [US-018](US-018-Place-of-Origin-Trust-Level-Maintenance.md), [US-019](US-019-Lock-Place-of-Origin-to-Bronze.md), [US-020](US-020-Update-Place-of-Origin-on-Import-Record.md) (Place of Origin counters)

## Traceability

### Source Jira Issues

- IMTA-6950
