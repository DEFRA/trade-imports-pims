# US-057: Manage Watchlist Records

## Summary

As an EU Imports Caseworker,
I want to add a place of origin, place of destination, consignee, transporter or veterinarian to a watchlist,
So that they can be monitored and related certificates and Import Records flagged automatically.

## Description

The Watchlist entity covers five watch types: Place of Origin, Place of Destination, Consignee, Transporter and Veterinarian. Each entry records why a party is being monitored, for how long, and carries a dated, authored comment trail.

## Acceptance Criteria

- **AC-1 (Add an existing record to the watchlist):** An EU Imports Caseworker can add a place of origin, place of destination, consignee, transporter or veterinarian that already exists in PIMS to the watchlist, entering: Watch Type (mandatory; filters which records can be selected as Name), Name (search and select, filtered by Watch Type), Start Date (mandatory), End Date (optional; cannot be before the Start Date), Comments (mandatory).

- **AC-2 (Add a new record to the watchlist):** Where the party to be watched does not already exist in PIMS, an EU Imports Caseworker can create it (Name, Address Line 1, City, Postcode and Country mandatory; Address Line 2/3 optional) as part of adding it to the watchlist; the new record is stored in the relevant underlying entity (Place of Origin, Place of Destination, Consignee, Transporter or Veterinarian) and the caseworker continues entering the watchlist details from AC-1.

- **AC-3 (Amend a Watchlist record):** An EU Imports Caseworker can amend an existing Watchlist record's End Date (cannot be before the Start Date) and add a new comment.

- **AC-4 (Audit on save):** When a Watchlist record is created or amended, PIMS records who created it and the date and time of creation.

- **AC-5 (Comments):** An EU Imports Caseworker can add a comment to a Watchlist record; the comment is stored related to the Watchlist record, with the author and date/time recorded.

- **AC-6 ("Active" definition):** A Watchlist entry is active when the current date is on or after its Start Date, and either it has no End Date or the current date is on or before its End Date.

## Business Rules

- [BR-054](../business-rules.md#br-054) — Watchlist "active" definition and flagging trigger points

## Dependencies

- [US-017](US-017-Manage-Place-of-Origin.md) (Place of Origin watch type)
- [US-058](US-058-Flag-Watched-Parties.md) (consumes active Watchlist entries to flag ITAHCs/Import Records)

## Traceability

### Source Jira Issues

- IMTA-7479
- IMTA-8012
