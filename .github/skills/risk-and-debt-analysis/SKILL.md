---
name: risk-and-debt-analysis
description: 'Assess non-functional requirement (NFR) coverage, and produce a risk register and technical/architecture debt assessment for a design or review. Evaluates every NFR category (security, performance, scalability, availability, maintainability, supportability, observability, auditability, compliance, recoverability, data residency, accessibility) but details only materially impacted categories; classifies technical debt and architecture debt as Accepted/Deferred/Mitigated/Eliminated/Unknown. Use whenever a design, ADR, SAD, or review needs its risk exposure, NFR gaps, or debt implications made explicit. Reusable by any architecture-focused agent.'
---

# Risk and Debt Analysis

## Purpose

Make the non-functional coverage, risk exposure, and technical/architecture debt implications of a design explicit and classified, so they can be governed rather than discovered later. This skill produces risk register entries, NFR assessments, and debt classifications for embedding into ADRs, SADs, and Design Review Reports produced by the calling agent — it does not decide the design itself (see `architecture-options-analysis`) or the Power Platform-specific ALM mechanism for reference data or deprecation (see `power-platform-architecture-design`).

## Trigger Conditions

Use this skill whenever:

- A design, ADR, or SAD needs a Risk Register or NFR Assessment produced or updated.
- An existing or proposed design introduces, removes, or leaves unresolved technical or architecture debt.
- A Design Review Report needs risk/debt findings alongside its structural findings.

## Expected Inputs

- The design or change under assessment, and its acceptance criteria.
- The Review Level/Complexity classification (to gauge how much NFR/risk depth is proportionate).
- Any existing risk register or debt register entries for the affected area.

## Responsibilities

### Non-functional requirements assessment

Evaluate every relevant category: security, performance, scalability, availability, maintainability, supportability, observability, auditability, compliance, recoverability, data residency, and accessibility (where user-facing).

- Provide a detailed assessment (expectation, how the design meets it, risks/mitigations) only for categories **materially impacted** by the change.
- For every remaining category, state explicitly that it is low relevance or not applicable, and why — never omit a category silently.
- Treat **observability** (monitoring, telemetry, logging, alerting, diagnostics, Application Insights or equivalent) as a first-class category, not implicitly covered by supportability.

### Risk register

Record each risk with: **description**, **category** (technical, security, operational, cost, licensing, technical debt, architecture debt), **likelihood**, **impact**, **mitigation**, and **owner** (or "unassigned" if none identified).

### Technical debt and architecture debt assessment

Assess these separately (report together where relevant):

- **Technical debt** — implementation-level shortcuts.
- **Architecture debt** — structural or pattern-level compromises affecting the wider solution landscape (e.g. temporary exceptions, duplicate/inconsistent patterns, transitional integration approaches, deviations from approved standards, legacy constructs retained for compatibility).

For each item, record: **Description**, **Rationale**, **Classification** (Accepted/Deferred/Mitigated/Eliminated/Unknown), and **Review or retirement criteria** where applicable.

## Workflow

1. Confirm the Review Level/Complexity classification to scale the depth of NFR/risk analysis.
2. Work through every NFR category; detail the materially impacted ones, state N/A/low-relevance rationale for the rest.
3. Identify risks across all categories (technical, security, operational, cost, licensing, technical debt, architecture debt) and populate the risk register.
4. Identify any technical debt or architecture debt introduced, resolved, or left in place by the design; classify each item.
5. Summarise the highest-severity risks and unresolved/Accepted debt items for the calling agent to surface in stakeholder-facing artefacts.

## Output Standards

- **NFR Assessment** — one row/section per category: category name, materiality (impacted / low relevance / not applicable), expectation, how the design meets it (if impacted), risks/mitigations (if impacted).
- **Risk Register** — table with Description / Category / Likelihood / Impact / Mitigation / Owner.
- **Debt Register** — table with Description / Rationale / Classification / Review or retirement criteria.

## Escalation Guidance

- Escalate any risk rated high likelihood **and** high impact with no identified mitigation or owner.
- Escalate any architecture debt classified **Accepted** or **Unknown** where the rationale is not clearly justified — this should be raised for stakeholder awareness rather than absorbed silently.
- Where an NFR gap cannot be resolved within the current design (e.g. a compliance or data residency requirement the platform cannot meet), flag it as a blocking risk rather than marking it low relevance.
