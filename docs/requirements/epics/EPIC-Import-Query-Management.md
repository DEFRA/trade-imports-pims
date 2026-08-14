# Epic: Import Query Management

## Purpose

Enable caseworkers to raise, track, assign and resolve formal queries against Import Records, supporting communication with importers and third parties.

## Business Value

- All queries and their resolution history are centralised and searchable.
- Overdue queries are visible, preventing items from falling through the cracks.
- Team leaders can monitor query workload and assignment.

## Capability Description

PIMS provides an Import Query custom activity entity that supports the full query lifecycle: creation, assignment to a caseworker, note and file attachment, and resolution.

## Functional Scope

- Create and update Import Query records (with auto-generated query number)
- View all queries in the system with multiple filter views (active, completed, overdue, mine)
- View queries within an Import Record (related subgrid)
- Search for queries by query number or Import Record name
- Attach notes and files to a query (including queries not owned by the user)
- Assign a query to another caseworker
- Close a query as resolved (with resolution date auto-recorded)

## Associated User Stories

| Story                                                       | Title                   |
| ----------------------------------------------------------- | ----------------------- |
| [US-025](../user-stories/US-025-Import-Query-Management.md) | Import Query Management |

## Source Jira Issues

IMTA-6185, IMTA-6255
