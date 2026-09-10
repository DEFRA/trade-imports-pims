---
name: bdd-scenario-generation
description: "Generate Behaviour Driven Development (BDD) Given/When/Then scenarios from requirements, user stories and acceptance criteria. Use when writing Gherkin-style scenarios, generating happy-path/error/validation/edge-case scenarios, verifying acceptance criteria coverage, identifying missing behavioural requirements, or flagging ambiguities that impact testability. Follows requirements-discovery and backlog-generation; precedes automated test authoring."
---

# BDD Scenario Generation

## Purpose

Convert requirements, user stories, and acceptance criteria into Given/When/Then scenarios that describe observable system behaviour. This skill generates scenarios and analyses coverage; it does not write automated test code or step definitions.

## When to Use

- A user story or acceptance criteria set needs to be expressed as testable BDD scenarios.
- Acceptance criteria exist but their behavioural coverage (happy path, error, validation, edge case) has not been checked.
- Stakeholders, developers and testers need a shared, business-readable description of expected behaviour before implementation or test automation begins.

## Expected Inputs

Requirements, user stories (As a/I want/So that), acceptance criteria, business rules — in any combination. If none of these are supplied, ask for at least one before generating scenarios.

## Working Principles

- Focus on behaviour rather than implementation — describe what the system does, not how.
- Prefer multiple simple scenarios over one complex scenario.
- Use concise British English and business language a non-technical stakeholder can follow.
- Keep scenarios understandable by business and technical stakeholders alike.
- Treat automated acceptance scenarios as a cost, not a default. Every scenario becomes a deployment gate: a single failure blocks promotion to higher environments. Write scenarios sparingly, reserving them for genuine business-critical behaviour and the negative paths that must never regress.
- Reuse existing step bindings wherever possible instead of authoring new ones, but never at the expense of scenario clarity or any other principle in this document — a slightly awkward reused step is preferable to a duplicate binding, but a genuinely misleading one is not worth reusing.
- Prefer extending an existing scenario with an additional assertion over creating a near-duplicate scenario, but only where the extension keeps the scenario's single-action, single-behaviour focus intact — a genuinely distinct business scenario still deserves its own `Scenario:`.

## Applicability Assessment (Run Before Generating Scenarios)

Not every user story or acceptance criterion warrants a new automated BDD scenario. Before generating any scenario, assess whether one is justified.

1. **Classify the change.** Determine whether the story represents:
   - A genuine business rule, workflow, calculation, permission, or integration behaviour — a strong candidate for a scenario.
   - A negative/failure path that, if it silently broke, would cause material harm (data loss, incorrect approval, security bypass, financial impact) — a strong candidate for a scenario.
   - A cosmetic, layout, copy, styling, or minor UI/UX change with no behavioural or business-rule impact — generally **not** a candidate for a new automated scenario.
   - A change already exercised by an existing step binding/scenario — do not duplicate; note that existing coverage applies instead of writing a new one.
2. **Check for existing coverage.** If the workspace or supplied context indicates the behaviour is already covered by an existing feature file or step binding, state this and recommend no new scenario rather than generating a duplicate.
3. **Weigh the deployment-gate cost.** For borderline cases, ask: if this scenario failed intermittently or the underlying UI text/layout changed again, would blocking a release be proportionate to the risk being guarded against? If not, recommend a lighter-weight verification (manual check, unit/component test, visual review) instead of a BDD scenario.
4. **Recommend, don't silently decide.** For each user story or acceptance criterion assessed, output one of:
   - **Scenario recommended** — genuine business behaviour or imperative negative path; proceed to generate.
   - **Scenario not recommended** — minor/cosmetic change, or already covered; state the reason and suggest an alternative form of verification if any.
   - **Needs clarification** — impact/severity is unclear; ask before deciding.
     Only generate full Given/When/Then scenarios for items marked "Scenario recommended".

## Scenario Standards

- Focus on observable behaviour and outcomes, not internal mechanisms.
- Use business language; avoid UI element names, field IDs, API calls, or code references.
- Keep each scenario independent — it must not rely on the state left behind by another scenario.
- Express the outcome (`Then`) clearly and, where possible, in a way that is directly verifiable.
- One action per scenario — a scenario should contain a single contiguous `When` (with `And` steps for the same action if needed), not multiple separate `When`/`Then` pairs chained together (e.g. `When`/`Then`/`When`/`Then`). If a second action is needed to observe a further outcome, it belongs in its own scenario. Multiple `Then`/`And` assertions verifying the outcome of that one action are fine and encouraged where they aid clarity.
- Abstract procedural detail that isn't relevant to the scenario's behaviour into a single higher-level step, rather than spelling out every intermediate step needed to reach that state. For example, prefer `Given I have processed an application` over enumerating each step of the process (assign, review, approve) unless one of those intermediate steps is relevant to the desired behaviour being documented by the scenario.
- Use business-level terminology, not product/UI-level terminology, unless the requirement explicitly mandates a specific UI element or label. For example, prefer `And I have recorded a visit` over `And I have selected the Visit tab / And I have created a visit` — the tab name may be implementation detail, not a business requirement.

## Persona and Login Step

Every scenario must begin with the mandatory login step:

```gherkin
Given I am logged in to the 'EU Imports' app as '<persona alias>'
```

1. **The persona alias must be one of the `aliases`** defined for a persona in [environment.json](../../../tests/Defra.Imports.Specs/environment.json) (e.g. `a caseworker`, `a business rules admin`, `a team leader`, `a caseworker with export to Excel permissions`). Do not invent a new persona alias — if the required role combination doesn't exist, flag it as a testability concern rather than fabricating one.
2. **A scenario runs as exactly one persona.** Do not switch persona partway through a scenario; if the behaviour genuinely requires two different personas interacting (e.g. one user submits, another approves), split it into separate scenarios or treat the second persona's action as an existing precondition rather than acting it out inline.
3. **Choose the least-privileged persona** that satisfies the scenario's precondition, unless the scenario is specifically testing permission/role-based behaviour, in which case the persona choice is the point of the scenario.

## Folder and Naming Conventions

Feature files are organised by table/entity, then by action, then by business scenario. Follow this structure unless the repository defines an existing, conflicting convention — in which case follow the existing convention.

1. **Folder per table/entity** — one folder for each Dataverse table or business entity the scenarios relate to, named after the entity in plural form (e.g. `Contacts/`, `Applications/`).
2. **Feature file per action** — within the entity folder, one `.feature` file per distinct user-facing action or capability associated with that table (e.g. `View contacts.feature`, `View a contact.feature`, `Create a contact.feature`, `Update a contact.feature`). A feature file represents a single action, not the whole entity — do not combine multiple actions (e.g. Create and Update) into one feature file.
3. **`Feature:` name matches the action** — the Gherkin `Feature:` line should read the same as the file name (e.g. `Feature: Update a contact`).
4. **Scenario per business scenario** — within a feature file, one `Scenario:` per genuine business scenario, named specifically enough to distinguish it from sibling scenarios (e.g. `Update a contact with a new name`, `View contacts that are active`, `View a contact's applications`). Avoid generic names such as "Success" or "Test 1".
5. **Only include scenarios that passed the Applicability Assessment** — a feature file may legitimately contain fewer scenarios than acceptance criteria if some were marked "not recommended".

Example structure:

```
Contacts/
  View contacts.feature
  View a contact.feature
  Create a contact.feature
  Update a contact.feature
```

When existing feature files for the same table/action already exist, add new scenarios to that file rather than creating a duplicate feature file.

## Step Binding Reuse

Before writing `Given`/`When`/`Then` steps, check whether an existing step binding already covers the required behaviour. Reusing bindings reduces duplication and avoids growing the step definition surface unnecessarily — but do not force-fit an existing step whose wording distorts the scenario's meaning, and do not skip a genuinely required negative-path or validation step just because no binding exists yet.

1. **Build the step index** by running the script at [scripts/Build-StepIndex.ps1](./scripts/Build-StepIndex.ps1). It scans the Reqnroll step definition files (`tests/Defra.Imports.Specs/StepDefinitions` by default) for `[Given]`/`[When]`/`[Then]`/`[StepDefinition]` attributes and writes a YAML index of each binding's keyword, capture pattern, source file, and line number to a temporary file — never to a path inside the repository.
2. **Query the index, not the source files.** Read the generated YAML to check for a matching or near-matching capture pattern before deciding whether a step is reusable. Only open the underlying `.cs` file if the index alone doesn't clarify the binding's exact behaviour.
3. **Prefer reuse when the existing step's wording and behaviour genuinely match** the scenario being written. Prefer a new step when reuse would require awkward phrasing, misrepresent the action, or bind unrelated behaviour together.
4. **Re-run the script per session, not per scenario.** The index is a point-in-time snapshot; regenerate it at the start of a scenario-authoring session (or after step bindings are known to have changed) rather than caching it indefinitely, and never commit the generated file to source control.

## Scenario Reuse

Before adding a new scenario, check whether an existing scenario in the target feature file already covers most of the same behaviour and could be extended with an additional assertion instead.

1. **Build the scenario index** by running the script at [scripts/Build-ScenarioIndex.ps1](./scripts/Build-ScenarioIndex.ps1). It scans `.feature` files (`tests/Defra.Imports.Specs/Features` by default) for `Feature:`/`Scenario:`/`Scenario Outline:` lines and any preceding `@tag`s, and writes a YAML index of each scenario's name, tags, source file, and line number to a temporary file — never to a path inside the repository.
2. **Query the index, not every feature file.** Read the generated YAML to check whether a scenario with a similar name or tag already exists in the relevant feature file before drafting a new one. Use the recorded `line` to jump straight to the candidate scenario rather than reading the whole file.
3. **Extend rather than duplicate when the existing scenario's `Given`/`When` already matches** and only the expected outcome differs — add a `Then`/`And` assertion to the existing scenario. Write a new scenario when the precondition or action genuinely differs, or when extending would violate the one-action-per-scenario or single-behaviour principles.
4. **Re-run the script per session, not per scenario.** As with the step index, this is a point-in-time snapshot — regenerate it at the start of a scenario-authoring session rather than caching it, and never commit the generated file to source control.

## Procedure

1. **Confirm the source material** — identify the requirement, user story or acceptance criteria being converted. If the business objective is unclear, treat it as a blocking question rather than assuming.
2. **Run the Applicability Assessment** for each user story/acceptance criterion and record the recommendation (Scenario recommended / Scenario not recommended / Needs clarification) before writing any Gherkin.
3. **Build or refresh the step binding and scenario indexes** per Step Binding Reuse and Scenario Reuse, so existing bindings and scenarios can be considered while drafting scenarios.
4. **Identify the feature** the scenarios belong to, and name it from the user's perspective, following the Folder and Naming Conventions — only for items marked "Scenario recommended".
5. **Generate happy-path scenarios** — the primary successful flow(s) described by the acceptance criteria.
6. **Generate validation scenarios** — required fields, format rules, boundary values, and business rule constraints, where these represent genuine business risk rather than minor UI polish.
7. **Generate error scenarios** — failure conditions, system errors, and rejected actions imperative to prevent, including the expected user-facing outcome.
8. **Generate edge-case scenarios** only where the edge case carries real business or data-integrity risk — unusual but plausible conditions (e.g. concurrency, empty states, limits) that are not covered by the categories above.
9. **Verify acceptance criteria coverage** — map every acceptance criterion to at least one scenario or to an explicit "not recommended" decision. Do not invent acceptance criteria that were not supplied or implied.
10. **Identify missing behavioural requirements** — behaviours implied by the feature but not covered by any supplied acceptance criteria.
11. **Highlight ambiguities** that impact testability — vague terms, undefined thresholds, or conflicting rules that prevent a scenario from being written unambiguously.

## Output Format

First, present the applicability assessment as a table:

| User Story / AC | Classification                                               | Decision                                                     | Rationale                    |
| --------------- | ------------------------------------------------------------ | ------------------------------------------------------------ | ---------------------------- |
| <reference>     | Business rule / Negative path / Cosmetic / Existing coverage | Scenario recommended / Not recommended / Needs clarification | <1-2 sentence justification> |

Then, for items marked "Scenario recommended" only, state the target file path per the Folder and Naming Conventions, followed by the feature content:

```
<Entity>/<Action>.feature

Feature: <feature name>

@scenario-type:happy-path
Scenario: <scenario name>
  Given I am logged in to the 'EU Imports' app as '<persona alias>'
  And <additional precondition>
  When <action or event>
  Then <expected outcome>
  And <additional outcome>
```

Group scenarios under their feature, and tag each scenario with its category using `@scenario-type:<category>` immediately above the `Scenario:` line, using one of: `@scenario-type:happy-path`, `@scenario-type:validation`, `@scenario-type:error`, `@scenario-type:edge-case`.

## Additional Analysis

After the scenarios, include:

1. Acceptance Criteria Coverage — which criteria are covered, by which scenario(s), and which were deliberately left uncovered per the applicability assessment.
2. Missing Scenarios — behaviours not covered by supplied requirements or acceptance criteria.
3. Testability Concerns — anything that cannot be verified as written.
4. Ambiguities Detected — unclear or conflicting requirements found during generation.
5. Deployment-Gate Risk — note any recommended scenario whose flakiness or maintenance cost could disproportionately block deployments, and any "not recommended" item where the team should reconsider if risk tolerance changes.

State "None identified" explicitly for any section with nothing to report rather than omitting it.

## Success Criteria

This skill succeeds when stakeholders, developers and testers share a common, unambiguous understanding of expected behaviour, every supplied acceptance criterion is traceable to either a scenario or an explicit, justified decision not to automate it, scenarios are limited to genuine business behaviour and imperative negative paths, and any gaps or ambiguities are surfaced explicitly rather than silently resolved.

## Handoff

This skill produces scenario _content_, not automated test code. Automated step definitions or test implementation should be handled by a dedicated test-authoring skill or agent for the target framework.
