---
name: architecture-options-analysis
description: 'Evaluate multiple design options for a non-trivial architectural decision and produce a structured, trade-off-explicit recommendation with an assessed Decision Confidence and a reuse assessment (reusable capability, extension, or requirement-specific). Use whenever a design decision at Review Level 2 or 3 needs genuine options considered rather than a single assumed approach presented as a foregone conclusion. Reusable by any architecture-focused agent producing recommendations, SADs, or ADR content.'
---

# Architecture Options Analysis

## Intended Agent

This skill may only be used by the Solution Architect agent.

If the current agent is not the Solution Architect agent:

- Stop.
- Explain that this skill is owned by the Solution Architect.
- Recommend handing off to the Solution Architect agent.

## Purpose

Ensure every non-trivial architectural decision results from genuine option evaluation, with trade-offs, reuse implications, and confidence made explicit, rather than a single assumed approach. This skill produces the structured recommendation content that other artefacts (ADRs, SADs, Architecture Review Summaries) reference or embed — it does not decide governance status (see `adr-management`), classify review depth (see `architecture-review-classification`), or perform full NFR/risk/debt analysis (see `risk-and-debt-analysis`).

## Trigger Conditions

Use this skill whenever:

- A design decision has more than one plausible implementation approach.
- A requirement could be met by a new component, an extension of an existing one, or a configuration change.
- A recommendation is about to be written into an ADR, SAD, or Architecture Review Summary without documented alternatives, trade-offs, or a confidence rating.

**Skip or scale down for Level 1 (Pattern Conformance)** changes per `architecture-review-classification` — where an existing approved pattern/ADR already answers the question, restate that pattern rather than re-deriving options from scratch. If pattern-fit triage found a governing ADR, treat its original option evaluation as prior art and only re-evaluate what has materially changed.

## Expected Inputs

- The requirement/problem statement and its acceptance criteria.
- The Architecture Review Level/Complexity classification from `architecture-review-classification` (to scale how many options and how much depth is proportionate).
- Any governing ADR identified by pattern-fit triage, and its original option evaluation.
- Knowledge of existing components, patterns, and platform capabilities available for reuse.

## Responsibilities

1. **Enumerate options**, treating the obvious/assumed approach as one candidate among several rather than a foregone conclusion. Include at minimum:
   - At least one lower-code / simplification / configuration-driven / existing-platform alternative.
   - An AI-enabled or agentic alternative **only** where genuinely relevant to the business problem, with clear business/operational/economic justification — never merely to appear thorough.
2. **Evaluate each option** as a comparative lens for choosing between them — governance fit, security, scalability, maintainability, supportability, licensing, cost, and technical debt implications. This is a lighter-weight comparison to inform the choice, not a substitute for the full NFR assessment and risk register produced by `risk-and-debt-analysis`.
3. **Assess reusability** for each option — whether it should be a reusable platform service, an extension of an existing reusable capability, or a requirement-specific implementation. Prefer configuration-driven reuse over framework-driven reuse (bespoke generic engines, plug-in frameworks), and justify reusable design by a reasonable expectation of future value rather than mere possibility — never build speculative frameworks or abstractions for hypothetical future requirements alone. (Platform-specific reuse patterns, e.g. for Power Platform, are detailed in `power-platform-architecture-design`.)
4. **Select and justify** the recommended option, documenting what is gained and given up relative to the alternatives.
5. **Assess Decision Confidence** for the recommendation:
   - **High** — validated requirements, established patterns, approved ADRs, sufficient repository context, minimal material uncertainty.
   - **Medium** — sound but relies on one or more assumptions, incomplete information, or areas requiring validation during delivery.
   - **Low** — significant requirements gaps, architectural unknowns, external dependencies, or unresolved decisions materially affect confidence.

   Always explain the primary factors behind the rating, distinguish confirmed facts from assumptions, recommend validation activities where Medium/Low, and never suppress a recommendation merely because confidence is low — provide the best available recommendation while being transparent about the uncertainty.

## Workflow

1. Confirm the Review Level/Complexity classification; for Level 1, restate the governing pattern instead of running this workflow.
2. List candidate options (minimum: the obvious approach, a lower-code/configuration-driven alternative, and — where relevant — an AI/agentic alternative), reusing any prior ADR evaluation as a starting point.
3. Evaluate each option against the criteria in Responsibility 2.
4. Assess the reuse dimension for each option (Responsibility 3).
5. Select the recommended option and document the trade-off rationale.
6. Assign and justify the Decision Confidence rating.

## Output Standards

Every recommendation produced by this skill must include:

- **Recommendation** — the proposed approach, stated plainly.
- **Rationale** — why this option best meets the requirement and its constraints.
- **Alternatives Considered** — the other options evaluated and why they were not chosen.
- **Benefits** — the value delivered by the recommendation.
- **Risks** — what could go wrong and its likely severity (hand off detailed risk register entries to `risk-and-debt-analysis`).
- **Assumptions** — what is being taken as true pending confirmation.
- **Dependencies** — other systems, teams, solutions, or decisions this relies on.
- **Licensing Impact** — any change to licence requirements, connector usage, or entitlements.
- **Operational Considerations** — monitoring, support, and maintenance impact once live.
- **Decision Confidence** — High/Medium/Low, with reasons and any action needed to increase confidence.

Where reuse was assessed, additionally state: why reuse is valuable in this case; expected consumers of the reusable capability; additional complexity introduced; operational/maintenance implications; and why a simpler requirement-specific design was not selected instead.

## Escalation Guidance

- Where information needed to evaluate an option is absent, do not invent requirements — document the assumption/constraint/open question and flag the gap back to the calling agent (which routes it to the Product Analyst if it is a requirements gap).
- Escalate Medium/Low confidence recommendations to stakeholders or the Product Analyst rather than proceeding as if the decision were settled.
- If pattern-fit triage surfaces a conflict between a candidate option and an existing approved ADR, surface it explicitly rather than silently favouring the new option.
