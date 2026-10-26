# Final report template

Keep the report factual and short. Quote script output verbatim rather than paraphrasing it.

````markdown
# Migration report: <PluginA> and <PluginB> (AB#<id> | AB# not supplied)

## Outcome
<One or two sentences: what was migrated; verified by unit tests only; not committed.>

## Layers created
| Layer | Type | File |
| --- | --- | --- |

## Files deleted
- <path> (and its `.projitems` entry)

## Verification
- Gate (`Test-Change.ps1 -PathFilter '<new and changed files>'`, no skip switches):
  ```text
  BUILD …
  WARNINGS …
  IN SCOPE …
  TESTS …
  RESULT …
  ```
- Removed warnings: <count>, all from deleted legacy files.
- Warnings in new and changed files, including test files (from `IN SCOPE`): <none | list>.
- Stale-reference sweep: <clean | expected mentions only (ADR history)>.
- Tests: <n> new, including <n> ported from <n> legacy cases.

## Flagged bugs and bad practices (preserved, not fixed)
1. <file:member>: <what, and why it is suspect>

## Behaviour changes
- Step-contract validation (misconfigured registrations only): <the checks, per handler>.
- `PluginBase` standing differences:
  - unexpected failures are wrapped in `InvalidPluginExecutionException`;
  - the unused legacy trace writer is no longer created;
  - start and completion trace lines are added.
- <Any other approved change, or "None">

## Working-tree changes that are not mine
- <path and status from the intake `git status`, or "None">

## Open questions
- <The AB# work item, if still missing>
- Live Dataverse validation has not been done.
- <Others, or nothing further>
````
