# Epic: Team and Geographic Assignment

## Purpose

Enable Import Records, ITAHCs and DOCOMs to be assigned to the correct geographic regional team for processing and risk assessment.

## Business Value

- Consignments are routed to the responsible regional team automatically or manually, reducing misrouting.
- In the event of a regional disaster, other teams can access records to maintain continuity.

## Capability Description

PIMS supports assignment of Import Records to geographic teams (IRMS Scotland, IRMS Wales, IRMS England). ITAHCs and DOCOMs automatically created by the TRACES integration are assigned to the appropriate regional team. Caseworkers can also manage the list of APHA Regions.

## Functional Scope

- Assign an Import Record to a geographic team (Scotland, Wales, England)
- View another team's Import Records in the event of a regional disaster
- Automatically assign ITAHC/DOCOM records to the correct region when created by TRACES integration
- Create and update APHA Region reference records
- Search for APHA Regions

## Associated User Stories

| Story                                                                 | Title                             |
| --------------------------------------------------------------------- | --------------------------------- |
| [US-026](../user-stories/US-026-Geographic-Team-Assignment.md)        | Geographic Team Assignment        |
| [US-027](../user-stories/US-027-Auto-Assign-ITAHC-DOCOM-to-Region.md) | Auto-Assign ITAHC/DOCOM to Region |
| [US-034](../user-stories/US-034-Manage-APHA-Region.md)                | Manage APHA Region                |

## Source Jira Issues

IMTA-5863, IMTA-6165, IMTA-6661
