# Epic: Watchlist and Flagging

## Purpose

Enable EU Imports Caseworkers to place a place of origin, place of destination, consignee, transporter or veterinarian onto a watchlist, and have PIMS automatically flag any ITAHC or Import Record involving a watched party so that it receives additional scrutiny.

## Business Value

- Surfaces previously-identified risks (e.g. a non-compliant premises or transporter) automatically on every future consignment that involves them, without relying on caseworker memory.
- Provides a single, auditable record of why a party is being monitored, with a comment trail.

## Capability Description

PIMS maintains a Watchlist entity covering five watch types: Place of Origin, Place of Destination, Consignee, Transporter and Veterinarian. Each watchlist entry has a start date, an optional end date and a mandatory reason, and is considered "active" between its start and end dates (or indefinitely if no end date is set). When an ITAHC is created, an Importer Notification is created, or an Import Record is created or matched, PIMS checks whether any of the involved parties are on an active watchlist entry and, if so, flags the record so the caseworker can open the watchlist details directly from the flag.

## Functional Scope

- Create and amend Watchlist records for an existing or newly-created place of origin, place of destination, consignee, transporter or veterinarian
- Add dated, authored comments to a Watchlist record
- Flag an ITAHC on creation when an involved party is on an active watchlist entry
- Flag an Importer Notification on creation when an involved party is on an active watchlist entry
- Flag an Import Record on creation from a flagged ITAHC or Importer Notification, and on confirmation of a match, when an involved party is on an active watchlist entry
- View Watchlist record details and comments directly from a flag on the ITAHC, Importer Notification or Import Record

## Associated User Stories

| Story | Title |
| --- | --- |
| [US-057](../user-stories/US-057-Manage-Watchlist-Records.md) | Manage Watchlist Records |
| [US-058](../user-stories/US-058-Flag-Watched-Parties.md) | Flag Watched Parties on ITAHC and Import Record |

## Source Jira Issues

IMTA-7479, IMTA-7482, IMTA-8012, IMTA-8015, IMTA-8220
