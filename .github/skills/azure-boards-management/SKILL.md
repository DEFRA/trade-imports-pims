---
name: azure-boards-management
description: 'Create, update, query and organise Azure Boards work items (Epics, Features, User Stories, Tasks) from requirements and backlog content produced by requirements-discovery and backlog-generation. Configuration is loaded from .agent-config/azure-devops.json and validated against config.schema.json before use. Use when creating or updating Epics/Features/User Stories/Tasks in Azure DevOps, changing work item state or fields, linking parent-child or related work items, querying existing backlog items, checking for duplicates before backlog creation, validating Azure Boards configuration, or executing Azure Boards CLI/REST operations. Follows backlog-generation; used by the Product Analyst agent to execute backlog changes.'
---

# Azure Boards Management

## Purpose

Execute CRUD and query operations against Azure Boards so that approved requirements and backlog content are accurately reflected as work items. This skill executes backlog changes; it does not discover requirements (see `requirements-discovery`) or decompose them into stories (see `backlog-generation`), and it does not decide requirement quality or priority — those remain the responsibility of the Product Analyst agent.

## When to Use

- Epics, Features, User Stories, or Tasks need to be created in Azure Boards from approved backlog content.
- An existing work item's fields, state, or relationships need to be updated.
- Parent-child or related-item links need to be created or verified.
- Existing work items need to be queried before creating new ones (duplicate/overlap check, hierarchy check, status check).
- A pull request needs an `AB#` work item reference resolved or confirmed per [AGENTS.md](../../../AGENTS.md).

## Expected Inputs

Validated requirements or backlog content (typically from `backlog-generation` or the Product Analyst agent), a repository configuration file conforming to [config.schema.json](./config.schema.json), and — for updates — the target work item identifier(s).

## Configuration

This skill is configuration-driven. [config.schema.json](./config.schema.json) is the **authoritative** definition of valid configuration — do not accept or invent configuration properties that are not defined in the schema.

- **Load** repository configuration from `.agent-config/azure-devops.json` in the workspace root.
- **Validate** the loaded configuration against [config.schema.json](./config.schema.json) before performing any operation. Do not proceed to query, create, or update work items on invalid or unvalidated configuration.
- **Treat [examples/azure-devops.example.json](./examples/azure-devops.example.json) as a template only** — never read from or write to the example file as if it were the active configuration, and never treat its values as defaults for a real project.
- If `.agent-config/azure-devops.json` is missing, tell the user it is required, show them the example file as a starting template, and stop — do not guess an organisation or project.
- If validation fails, explain each validation issue clearly (which field, why it failed, what the schema requires) before continuing. Do not attempt the requested operation with invalid configuration.

### Configuration Reference

**Required fields:**

- `organization` — Azure DevOps organisation URL or name.
- `project` — Azure DevOps project name.
- `workItemTypes.epic`, `workItemTypes.feature`, `workItemTypes.story`, `workItemTypes.task` — the process template's actual work item type names for each logical role this skill uses.

**Optional fields:**

- `defaultAreaPath` — applied to new work items when no more specific Area Path is supplied.
- `defaultIterationPath` — applied to new work items when no more specific Iteration Path is supplied.

**Required version field:**

- `schemaVersion` — required and must be one of the schema versions currently supported by [config.schema.json](./config.schema.json); used to detect when migration guidance applies.

**Validation behaviour:**

- The schema uses `additionalProperties: false` at both the top level and within `workItemTypes` — configuration with unrecognised properties is invalid, not silently ignored.
- All four `workItemTypes` entries are required even if a project only uses some of them today, so the skill always has an unambiguous type name to use for each logical role.
- Validation must run every time configuration is loaded; do not cache a previously validated result across sessions.

**Migration guidance when the schema changes:**

- Compare the configuration's `schemaVersion` against the current [config.schema.json](./config.schema.json) before relying on it.
- If the schema has gained new required fields since the configuration's `schemaVersion`, tell the user which fields are missing and offer to help populate them, rather than silently defaulting.
- If the schema has gained new optional fields (e.g. team, area/iteration path mappings, custom work item types, state mappings, multiple projects), the existing configuration remains valid — do not force an update, but mention the new capability if it is relevant to the current request.
- Never remove or rename a configuration property in `.agent-config/azure-devops.json` without explicit user confirmation, even if the schema evolves.

## Preferred Execution Methods

Select the first available mechanism, in order:

1. **Azure DevOps MCP tools**, if an MCP server exposing Azure Boards operations is available.
2. **Azure DevOps CLI** (`az boards ...`) via terminal execution.
3. **Azure DevOps REST API** via terminal execution (e.g. `curl`/`Invoke-RestMethod`), only if the CLI cannot perform the operation.

Confirm which mechanism is available before starting; do not assume MCP tools exist without checking. State which mechanism was used in the output.

### CLI Field Value Handling (Windows)

- When passing rich-text field values (e.g. `System.Description`, `Microsoft.VSTS.Common.AcceptanceCriteria`) via `--fields "Field=$value"` to `az boards work-item create`/`update`, the value must not contain embedded newline characters. On Windows, `az` is a `.cmd` wrapper invoked through `cmd.exe`, which truncates an argument at the first embedded newline, silently dropping the remainder of that field and any `--fields` arguments that follow it, with no error and a successful exit code.
- Build multi-line HTML/rich-text content as a single-line string (e.g. join paragraph `<div>...</div>` blocks with no separator) before passing it as a CLI argument.
- After any create or update that sets field content via the CLI, re-fetch the work item and check the affected field length/content. Do not treat a non-error exit code as confirmation that the value was written as intended.

## Required Validation

Before any operation, confirm:

- `.agent-config/azure-devops.json` has been loaded and validates against [config.schema.json](./config.schema.json) (see Configuration above). Do not proceed on invalid or missing configuration.

Before creating a work item, confirm:

- Title exists.
- Description exists.
- Acceptance criteria exist, where the work item type supports them (User Stories; Features/Epics where defined).
- A parent relationship is identified where the work item type requires one (User Stories must sit under a Feature; Features must sit under an Epic, per `backlog-generation`).
- The target Azure DevOps organisation and project are known (from validated configuration).

Do not create a work item when required information is missing — report the gap instead (see Error Handling).

Before updating a work item, confirm:

- The work item identifier is known and confirmed with the user for significant or destructive changes.
- The change does not silently remove existing information (state, description, links) unless explicitly instructed.

## Work Item Standards

- **Epics** represent business outcomes, not solutions or technical initiatives.
- **Features** represent significant business capabilities that group related User Stories.
- **User Stories** use the format:

  ```
  As a <role>
  I want <capability>
  So that <benefit>
  ```

- **Acceptance criteria** are included on every User Story, and on Features/Epics where the team defines them, following the testability standards in `backlog-generation`.
- **Tasks** are created only when explicitly requested, and are linked to a parent User Story.

## Procedure

1. **Load and validate configuration** from `.agent-config/azure-devops.json` against [config.schema.json](./config.schema.json). If missing or invalid, stop and report per Configuration above.
2. **Confirm the execution mechanism** available (MCP, CLI, or REST) and the target organisation/project from the validated configuration.
3. **Query first.** Search Azure Boards for existing related work items before creating anything — check for duplicates, overlapping scope, and existing parent items.
4. **Validate inputs** against the Required Validation checklist for the operation being performed (create vs update).
5. **Resolve hierarchy.** For creates, identify or confirm the parent Epic/Feature. For updates, confirm existing parent-child and related links are not broken by the change.
6. **Execute the operation** (create, update, state change, field change, link creation) using the selected mechanism and the work item type names from `workItemTypes` in configuration.
7. **Verify the result** — re-query the created/updated work item to confirm the operation applied as expected.
8. **Report the outcome** using the Output Format below.

## Output Format

For every operation (or batch of operations), report:

1. Operation performed (create / update / state change / field change / link / query)
2. Work item type
3. Work item identifier(s)
4. Relationships created or changed
5. Validation warnings
6. Any failed operations, with reason

## Error Handling

- Explain validation failures clearly and specifically (which field or relationship is missing) — this applies equally to configuration schema failures and work item field failures.
- Highlight missing required information rather than guessing a value.
- Do not create incomplete work items when required information is absent — ask the user or hand back to the Product Analyst agent for the missing detail instead.
- Do not perform any Azure Boards operation against configuration that has failed schema validation.
- If the execution mechanism itself fails (auth, permissions, connectivity), report the failure and the mechanism attempted; do not silently fall back without stating so.

## Working Principles

- Preserve traceability: every User Story links to a Feature, every Feature links to an Epic.
- Maintain hierarchy integrity: never leave a work item orphaned when a parent is known.
- Avoid duplicate work items — always query before creating.
- Read before write, and prefer updating an existing item over creating a duplicate.
- Use British English.
- Validate before creating or updating.
- Never rely on configuration properties that are not defined in [config.schema.json](./config.schema.json).
- Confirm significant or destructive changes (state changes that close/remove work, deletions, removing links) with the user before executing.

## Success Criteria

This skill succeeds when Azure Boards accurately reflects the approved requirements and backlog structure it was given, with no duplicate or orphaned work items, full traceability from User Story to Feature to Epic, and every operation reported clearly enough that the user can verify what changed.

## Handoff

This skill executes backlog operations; it does not originate requirements or decompose them. For discovering requirements, use `requirements-discovery`. For decomposing requirements into epics/features/stories/acceptance criteria, use `backlog-generation`. For requirement quality, INVEST checks, and overall backlog ownership, defer to the Product Analyst agent, which should invoke this skill to execute the resulting Azure Boards changes.
