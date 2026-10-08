# Epic: Matching Process

## Purpose

Enable EU Imports Caseworkers to automatically identify, review and confirm candidate matches between inbound health certificates (ITAHC, DOCOM) and Importer Notifications, and to create the resulting Import Record without repeating manual searches.

## Business Value

- Removes the manual effort of searching for a likely-matching certificate or notification.
- Reduces mismatched or duplicate Import Records by surfacing a ranked, comparable candidate list before confirmation.
- Keeps the active matching workload manageable by archiving stale, unmatched notifications.

## Capability Description

PIMS periodically runs a configurable, weighted matching algorithm across unmatched ITAHCs/DOCOMs and Importer Notifications, creating a Match Record for each candidate pair that meets a configurable confidence threshold. Caseworkers review candidates side-by-side, confirm or reject them through a guided business process, and can create the resulting Import Record directly from a confirmed match. Notifications left unmatched beyond a configurable age are archived.

This epic supersedes the ad-hoc, high-level matching description previously carried solely by [US-046](../user-stories/US-046-Match-Inbound-Records-to-Import-Records.md), which remains in place as a summary-level story cross-referencing the detailed stories below.

## Functional Scope

- Configure the fields and weightings used by the matching algorithm
- Automatically calculate a weighted percentage match between an ITAHC/DOCOM and an Importer Notification
- Review candidate matches via dedicated views (Unmatched, Matched, Rejected, All) and a side-by-side comparison screen
- Confirm, reject or reactivate a candidate match through a guided business process
- Create an Import Record directly from a confirmed match, with a choice of address source
- Archive unmatched Importer Notifications older than a configurable threshold
- Flag certificates/notifications with more than one commodity code for caseworker review
- Compare commodity identifiers across ITAHC, Importer Notification, Import Record and Match Record

## Associated User Stories

| Story | Title |
| --- | --- |
| [US-050](../user-stories/US-050-Configure-Matching-Algorithm.md) | Configure the Matching Algorithm |
| [US-051](../user-stories/US-051-Automated-Matching.md) | Automated Matching of Certificates to Importer Notifications |
| [US-052](../user-stories/US-052-Review-Resolve-Candidate-Matches.md) | Review and Resolve Candidate Matches |
| [US-053](../user-stories/US-053-Create-Import-Record-at-Matching.md) | Create Import Record at Point of Matching |
| [US-054](../user-stories/US-054-Archive-Stale-Unmatched-Notifications.md) | Archive Stale Unmatched Importer Notifications |
| [US-055](../user-stories/US-055-Flag-Multiple-Commodity-Certificates.md) | Flag Multiple-Commodity Certificates for Caseworker Review |
| [US-056](../user-stories/US-056-Compare-Commodity-Details.md) | Compare Commodity Details Across Certificates and Import Records |

## Source Jira Issues

IMTA-5872, IMTA-7341, IMTA-7368, IMTA-7377, IMTA-7386, IMTA-7394, IMTA-7587, IMTA-7588, IMTA-7589, IMTA-7591, IMTA-7592, IMTA-7595, IMTA-7596, IMTA-7648, IMTA-7649, IMTA-7688, IMTA-7719, IMTA-7720, IMTA-7721, IMTA-7778, IMTA-7783, IMTA-7975
