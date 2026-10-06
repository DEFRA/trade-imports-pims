# US-046: Match Inbound Records to Import Records

## Summary

As an EU Imports Caseworker,  
I want PIMS to identify and present candidate Import Records for inbound certificates and notifications,  
So that I can review likely matches without repeating manual searches.

## Description

PIMS includes a matching capability centred on Match Record processing. When relevant inbound data is created or updated, matching logic searches for potentially related Import Records and presents them for review.

This story captures the matching behaviour supported by Match Record processing and the Match View. It also captures the Work Schedule Number column confirmed on the "8. Match View" saved query.

**The detailed matching algorithm, configuration, review screens, business process and related capabilities (Watchlist flagging, stale-notification archiving, multi-commodity flagging and commodity comparison) are specified in [EPIC-Matching-Process](../epics/EPIC-Matching-Process.md) and [EPIC-Watchlist-and-Flagging](../epics/EPIC-Watchlist-and-Flagging.md). This story remains as the original summary-level description and should be read alongside those epics rather than in isolation.**

## Acceptance Criteria

- **AC-1:** When relevant inbound certificate or notification data is created or updated, PIMS runs matching logic to identify candidate related Import Records. See [US-051](US-051-Automated-Matching.md) for the detailed algorithm.

- **AC-2:** Candidate related Import Records are presented in a match-oriented view or equivalent review surface for caseworker assessment. See [US-052](US-052-Review-Resolve-Candidate-Matches.md) for the detailed review and resolution process.

- **AC-3:** The Match View includes Work Schedule Number where that value is available on the related Import Record.

## Business Rules

- [BR-053](../business-rules.md#br-053) — Matching algorithm weighted mean calculation

## Dependencies

- [US-001](US-001-Manage-Import-Record.md) (Import Record)
- [US-006](US-006-Receive-Importer-Notification-From-IPAFFS.md) (Importer Notification receipt)
- [US-050](US-050-Configure-Matching-Algorithm.md) through [US-056](US-056-Compare-Commodity-Details.md) (detailed Matching Process stories), [US-057](US-057-Manage-Watchlist-Records.md), [US-058](US-058-Flag-Watched-Parties.md) (Watchlist)

## Traceability

### Source Jira Issues

- IMTA-5872
- IMTA-6720
