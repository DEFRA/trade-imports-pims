# Epic: Risk Assessment and Business Rules

## Purpose

Enable EU Imports Business Rules Admins to configure and maintain the rules that PIMS uses to automatically classify Import Records by risk level and determine whether a post-import check is required.

## Business Value

- Consistent, auditable and policy-compliant risk classification on every Import Record.
- Eliminates manual risk assessment for standard cases, freeing caseworkers to focus on complex or high-risk situations.
- Configurable rules allow policy changes to be applied without code releases.

## Capability Description

PIMS automatically determines the risk level (P1, P2, P3) and post-import check requirement for each Import Record based on:

- **Commodity Risk Levels** — configurable rules mapping Country + Commodity Type to a Risk Level.
- **Gold/Bronze Commodity rules** — define which commodity types (and optionally countries) are subject to Place of Origin trust level-based inspection logic.
- **Inspection Coverage Rules** — define the random inspection thresholds:
  - 2% of all P3 ITAHC Import Records (every ~50th record)
  - 10% of all P2 Import Records (every 10th record)
- **P1 Discretionary** — where a Gold/Bronze determination cannot be made, the inspection decision is left to caseworker discretion.

All rules are managed through Dynamics 365 entities with appropriate security role restrictions.

## Functional Scope

- Create and update Commodity Risk Level records (Country × Commodity Type → Risk Level)
- View and select Countries from reference data
- View and select Commodity Types from reference data
- Create and update Gold/Bronze Commodity records (Commodity Type + Country list)
- Update Inspection Coverage Rules (System Administrator only)
- Automatic risk assessment on Import Record create/update:
  - Evaluate Commodity Risk Level rules to set Risk Level
  - Evaluate Gold/Bronze rules and Place of Origin Trust Level to determine Post Import Check requirement
  - Apply 10% P2 random inspection counter
  - Apply 2% P3 random inspection counter (ITAHC records only)
  - Set Post Import Checks Required? and Post Import Checks Required Reason on Import Record

## Associated User Stories

| Story                                                                   | Title                                       |
| ----------------------------------------------------------------------- | ------------------------------------------- |
| [US-011](../user-stories/US-011-Manage-Commodity-Risk-Levels.md)        | Manage Commodity Risk Levels                |
| [US-012](../user-stories/US-012-Manage-Gold-Bronze-Commodities.md)      | Manage Gold/Bronze Commodities              |
| [US-013](../user-stories/US-013-Manage-Inspection-Coverage-Rules.md)    | Manage Inspection Coverage Rules            |
| [US-014](../user-stories/US-014-Automated-Risk-Assessment-P1.md)        | Automated Risk Assessment — P1 Consignments |
| [US-015](../user-stories/US-015-Automated-Risk-Assessment-P2.md)        | Automated Risk Assessment — P2 Random 10%   |
| [US-016](../user-stories/US-016-Automated-Risk-Assessment-P3-Random.md) | Automated Risk Assessment — P3 Random 2%    |

## Source Jira Issues

IMTA-5865, IMTA-5867, IMTA-5888, IMTA-5891, IMTA-5892, IMTA-5894, IMTA-5895, IMTA-5914, IMTA-5915, IMTA-5916, IMTA-5933
