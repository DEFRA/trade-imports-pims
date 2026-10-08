# US-058: Flag Watched Parties on ITAHC and Import Record

## Summary

As an EU Imports Caseworker,
I want an ITAHC or Import Record to be flagged when it involves a party that is active on the watchlist,
So that I am alerted to the risk and can view the watchlist details directly.

## Description

When an ITAHC is created, or an Import Record is created or matched, PIMS checks whether its place of origin, place of destination, consignee, transporter or veterinarian is active on the watchlist ([US-057](US-057-Manage-Watchlist-Records.md)) and flags the record accordingly, with one flag per matching watchlist entry. Each trigger point re-evaluates the watchlist entries that are active at that moment rather than reusing flags raised earlier.

## Acceptance Criteria

- **AC-1 (Flag on ITAHC creation):** When an ITAHC is created and one or more of its place of origin, place of destination, consignee, transporter or veterinarian is active on the watchlist, PIMS flags the ITAHC with a flag for each matching watchlist entry.

- **AC-2 (Flag on Import Record creation from an ITAHC):** When an Import Record is created from an ITAHC, PIMS re-evaluates the Import Record's place of origin, place of destination, consignee, transporter and veterinarian against the watchlist entries that are active at that point in time, and flags the Import Record with a flag for each matching active entry. The ITAHC's existing flags are not copied, so an entry that has since expired does not produce a flag and an entry added since the ITAHC was created does.

- **AC-3 (Flag on Import Record matching):** When an ITAHC is matched to an Importer Notification ([US-052](US-052-Review-Resolve-Candidate-Matches.md)) and an Import Record is created from that match, PIMS re-evaluates the parties against the watchlist entries that are active at the point of matching, on the same basis as AC-2.

- **AC-4 (Viewing flag details):** An EU Imports Caseworker viewing a flagged ITAHC or Import Record can select a flag to open the related Watchlist record, view all its details and comments, edit the record and add a new comment.

- **AC-5 (Flag on Importer Notification creation):** When an Importer Notification is created and one or more of its place of destination, consignee, transporter or place of origin is active on the watchlist, PIMS flags the Importer Notification with a flag for each matching watchlist entry, on the same basis as AC-1.

- **AC-6 (Flag on Import Record creation from an Importer Notification):** When an Import Record is created from a flagged Importer Notification, PIMS re-evaluates the Import Record's parties against the watchlist entries that are active at that point in time and flags the Import Record accordingly, on the same basis as AC-2.

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
