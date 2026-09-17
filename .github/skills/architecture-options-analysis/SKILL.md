---
name: architecture-options-analysis
description: 'Evaluate multiple design options for a non-trivial architectural decision — including at least one lower-code/simplification/configuration-driven/existing-platform alternative, and an AI-enabled or agentic alternative only where genuinely relevant — and produce a structured, trade-off-explicit architectural recommendation with an assessed Decision Confidence. Also assesses whether a capability should be reusable, an extension of an existing reusable capability, or requirement-specific. Use whenever a design decision needs options considered rather than a single option presented as a foregone conclusion. Reusable by any architecture-focused agent producing recommendations, SADs, or ADR content.'
---

# Architecture Options Analysis

## Purpose

Ensure every non-trivial architectural decision is the result of genuine option evaluation rather than a single assumed approach, with trade-offs, reuse implications, and confidence made explicit. This skill produces the structured recommendation content that other skills/artefacts (ADRs, SADs, Architecture Review Summaries) then reference or embed — it does not itself decide governance status (see `adr-management`) or classify review depth (see `architecture-review-classification`).

## Trigger Conditions

Use this skill whenever:

- A design decision has more than one plausible implementation approach.
- A requirement could be met by a new component, an extension of an existing one, or a configuration change.
- A recommendation is about to be written into an ADR, SAD, or Architecture Review Summary and does not yet have documented alternatives, trade-offs, or a confidence rating.

## Expected Inputs

- The requirement/problem statement and its acceptance criteria.
- The Architecture Review Level/Complexity classification from `architecture-review-classification` (to scale how many options and how much depth is proportionate).
- Knowledge of existing components, patterns, and platform capabilities available for reuse.

## Responsibilities

1. **Challenge the first idea.** Treat the assumed or most obvious approach as one option among several, not a foregone conclusion.
2. **Enumerate options**, including:
   - At least one lower-code / simplification / configuration-driven / existing-platform alternative.
   - An AI-enabled or agentic alternative **only** where genuinely relevant to the business problem, with clear business/operational/economic justification — never merely to appear thorough.
3. **Evaluate each option** against: governance fit, security, scalability, maintainability, supportability, licensing, cost, and technical debt implications.
4. **Assess reusability** for each option under consideration — whether it should be a reusable platform service, an extension of an existing reusable capability, a configuration-driven process, or a requirement-specific implementation. Prefer configuration-driven reuse (configuration tables, environment variables, parameterised flows) over framework-driven reuse (bespoke generic engines, plug-in frameworks). Avoid speculative frameworks, abstractions, or extensibility built solely for hypothetical future requirements — reusable design must be justified by a reasonable expectation of future value, not mere possibility.
5. **Select and justify** the recommended option, documenting what is gained and given up relative to the alternatives.
6. **Assess Decision Confidence** for the recommendation:
   - **High** — validated requirements, established patterns, approved ADRs, sufficient repository context, minimal material uncertainty.
   - **Medium** — sound but relies on one or more assumptions, incomplete information, or areas requiring validation during delivery.
   - **Low** — significant requirements gaps, architectural unknowns, external dependencies, or unresolved decisions materially affect confidence.

   Always explain the primary factors behind the rating, distinguish confirmed facts from assumptions, recommend validation activities where Medium/Low, and never suppress a recommendation merely because confidence is low — provide the best available recommendation while being transparent about the uncertainty.

## Workflow

1. Confirm the Review Level/Complexity classification (from `architecture-review-classification`) to gauge how many options and how much rigour is proportionate.
2. List the candidate options (minimum: the obvious approach, a lower-code/configuration-driven alternative, and — where relevant — an AI/agentic alternative).
3. Evaluate each option against the criteria in Responsibility 3.
4. For each option, assess the reuse dimension (Responsibility 4).
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
