# Behaviour freeze

A migration changes where code lives, not what it does. The authority is [Confirmed migration constraints](../../../../architecture/adr/0001-plugin-workflow-di-scaffold.md#confirmed-migration-constraints) in ADR 0001. This file turns those constraints into checks.

## Frozen

Keep each of these exactly as the legacy code has it, even when it looks wrong:

- **Database effects:** which records are read, created and updated, in what order and when.
  - Interleaved reads and writes stay interleaved.
  - An unconditional `Update` stays unconditional.
  - A read that happens even when its result is unused still happens.
- **Values:** what is written to the target, to new records and to outputs, including nulls and text built from missing attributes.
- **Errors:** exception types and messages, deliberate business exceptions and user-visible text.
- **Catch-and-continue:** if the legacy code caught, traced and carried on, the new code does the same at the same point. If it did not catch, nothing new catches.
- **Logging:** take `ILogWriter` only where the legacy code logged, and keep its messages. It resolves to `TimestampedTracingLogWriter`, which timestamps messages as the legacy `Plugin` base did. Any change to trace text needs approval and goes in the report.
- **Identity and transactions:** the executing user. Add no retries, transaction wrappers, deferred writes or `SaveChanges()`.

## Never add a failure mode

Do not add validation, guards or null checks that the legacy code did not have.

A previous migration added "attribute must be present" checks. They would have made real `Create` payloads fail, because a `Create` `Target` holds only the attributes that were supplied, and every other attribute reads as null. Projections copy values and never check that they are present.

## The one pre-approved change: step-contract validation

- Each handler checks the invocation against the registration printed by `Get-PluginStepRegistration.ps1`:
  - the primary entity;
  - the stage;
  - the mode;
  - the message, compared ignoring case;
  - the `Target`, and any image it reads.
- A mismatch throws `InvalidOperationException`, which `PluginBase` surfaces as `InvalidPluginExecutionException`. This changes behaviour only for a misconfigured registration.
- Validation comes before any catch-and-continue block, so a contract failure is never swallowed (see `PopulateAnimalNumberHandler`).
- List each check in the design proposal, the adoption record and the final report.

## Flag, do not fix

Mark every suspected bug or bad practice that you preserve with a `TODO:` comment that ends exactly like this:

```csharp
// TODO: DateTime.Now is server local time, not UTC. This should be investigated, but is preserved by the refactoring.
```

- Comments from earlier migrations use other wordings, and some lack the `TODO:` prefix. Leave them as they are.
- List every flagged item in the design proposal, the adoption record and the final report.

Earlier migrations flagged these, which shows the kind of thing to look for:

- `DateTime.Now`, which is server local time, rather than UTC.
- Lookups with no ordering, tie-break or `TopCount`, so an arbitrary match is used.
- Every exception swallowed inside the post-operation transaction, which a failed service call may already have doomed.
- An update sent even when the value is unchanged, which triggers downstream plug-ins, flows and audit.
- The first pre-image used, whatever its name.
- A record name built from an attribute that a `Create` `Target` may omit.
- Only the first match acted on, with nothing to prevent duplicate records.

## Standing differences introduced by `PluginBase`

Report these for every migration. They are accepted consequences of the pattern and do not need approval each time.

- **Unexpected failures are wrapped.** `PluginBase` traces any exception and rethrows it as `InvalidPluginExecutionException("The plug-in could not complete. See the plug-in trace log for details.", inner)`. The legacy `Plugin` base let the raw exception escape. An `InvalidPluginExecutionException` thrown by the handler passes through unchanged.
- **The unused legacy trace writer is no longer created.** The legacy base built a `TracingServiceLogWriter` and a `RepositoryFactory` on every call, whether or not the plug-in used them.
- **Trace output.** `PluginBase` adds "Starting handler …" and "Completed handler …" lines.

The identity is unchanged, so this is not a difference. The legacy base called `CreateOrganizationService(Guid.Empty)`, and `PluginBase` passes `context.UserId`. Both mean the executing user; see [Execution identity](../../../../architecture/adr/0001-plugin-workflow-di-scaffold.md#execution-identity).

## Changes that need approval

Anything else needs approval. Examples:

- a different exception, message or trace text;
- an extra or missing read or write;
- a change in when a write happens;
- a fixed bug.

Put each one under "Behaviour changes requiring approval" in the design proposal, and implement it only once it is approved.
