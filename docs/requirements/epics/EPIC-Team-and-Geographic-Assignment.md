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
- Record and auto-set the Devolved Office on Importer Notifications, ITAHCs and DOCOMs, and inherit it onto the Import Record
- Manually assign Import Records/Importer Notifications to dedicated APHA owner Teams

## Associated User Stories

| Story                                                                 | Title                             |
| ----------------------------------------------------------------------- | --------------------------------- |
| [US-026](../user-stories/US-026-Geographic-Team-Assignment.md)        | Geographic Team Assignment        |
| [US-027](../user-stories/US-027-Auto-Assign-ITAHC-DOCOM-to-Region.md) | Auto-Assign ITAHC/DOCOM to Region |
| [US-034](../user-stories/US-034-Manage-APHA-Region.md)                | Manage APHA Region                |
| [US-059](../user-stories/US-059-Devolved-Office-Assignment.md)        | Devolved Office Assignment        |
| [US-060](../user-stories/US-060-APHA-Owner-Team-Assignment.md)        | APHA Owner Team Assignment        |

## Source Jira Issues

IMTA-5863, IMTA-6165, IMTA-6661, IMTA-7473, IMTA-7475, IMTA-7780, IMTA-7784, IMTA-8103, IMTA-8108, EDA-618, EDA-619
