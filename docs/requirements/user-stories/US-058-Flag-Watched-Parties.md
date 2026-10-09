# US-058: Flag Watched Parties on ITAHC and Import Record

## Summary

As an EU Imports Caseworker,
I want an ITAHC or Import Record to be flagged when it involves a party that is active on the watchlist,
So that I am alerted to the risk and can view the watchlist details directly.

## Description

When an ITAHC or an Importer Notification is created, PIMS checks whether its place of origin, place of destination, consignee, transporter or veterinarian (as applicable) is active on the watchlist ([US-057](US-057-Manage-Watchlist-Records.md)) and flags the record accordingly, with one flag per matching watchlist entry. When an Import Record is created — from an ITAHC, from an Importer Notification, or from a confirmed match of the two — PIMS applies to the Import Record the flags already present on the source ITAHC and/or Importer Notification at the point of creation.

## Acceptance Criteria

- **AC-1 (Flag on ITAHC creation):** When an ITAHC is created and one or more of its place of origin, place of destination, consignee, transporter or veterinarian is active on the watchlist, PIMS flags the ITAHC with a flag for each matching watchlist entry.

- **AC-2 (Flag on Import Record creation from an ITAHC):** When an Import Record is created from an ITAHC, PIMS applies to the Import Record the flags already present on that ITAHC at the point of creation, with one flag carried over per matching watchlist entry.

- **AC-3 (Flag on Import Record matching):** When an ITAHC is matched to an Importer Notification ([US-052](US-052-Review-Resolve-Candidate-Matches.md)) and an Import Record is created from that match, PIMS applies to the Import Record the flags already present on both the matched ITAHC/DOCOM and the matched Importer Notification at the point of creation, on the same basis as AC-2.

- **AC-4 (Viewing flag details):** An EU Imports Caseworker viewing a flagged ITAHC or Import Record can select a flag to open the related Watchlist record, view all its details and comments, edit the record and add a new comment.

- **AC-5 (Flag on Importer Notification creation):** When an Importer Notification is created and one or more of its place of destination, consignee, transporter or place of origin is active on the watchlist, PIMS flags the Importer Notification with a flag for each matching watchlist entry, on the same basis as AC-1.

- **AC-6 (Flag on Import Record creation from an Importer Notification):** When an Import Record is created from an Importer Notification, PIMS applies to the Import Record the flags already present on that Importer Notification at the point of creation, on the same basis as AC-2.

## Business Rules

- [BR-054](../business-rules.md#br-054) — Watchlist "active" definition and flagging trigger points

## Dependencies

- [US-057](US-057-Manage-Watchlist-Records.md) (source of watchlist entries)
- [US-002](US-002-Manage-ITAHC.md), [US-001](US-001-Manage-Import-Record.md), [US-053](US-053-Create-Import-Record-at-Matching.md)

## Traceability

### Source Jira Issues

- IMTA-7482
- IMTA-8015
- IMTA-8220
