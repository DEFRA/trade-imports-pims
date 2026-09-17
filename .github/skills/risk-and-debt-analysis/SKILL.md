---
name: risk-and-debt-analysis
description: 'Assess non-functional requirement (NFR) coverage across all 12 categories (security, performance, scalability, availability, maintainability, supportability, observability, auditability, compliance, recoverability, data residency, accessibility), and produce a Risk Register plus a Technical/Architecture Debt Register (classified Accepted/Deferred/Mitigated/Eliminated/Unknown) for a design or review. Use whenever a design, ADR, SAD, or review needs its risk exposure, NFR gaps, or debt implications made explicit. Reusable by any architecture-focused agent.'
---

# Risk and Debt Analysis

## Purpose

Make the NFR coverage, risk exposure, and technical/architecture debt of a design explicit and classified, so they are governed rather than discovered later.

This skill produces the analysis; it does not decide the design (see `architecture-options-analysis`) and does not own the Power Platform-specific ALM mechanism for reference data or deprecation (see `power-platform-architecture-design`). Findings are handed back to the calling agent for embedding into its ADR, SAD, or Design Review Report.

## Trigger Conditions

Use whenever a design, ADR, SAD, or Design Review Report needs one of:

- An NFR Assessment produced or updated.
- A Risk Register produced or updated.
- Technical or architecture debt identified, classified, or reviewed.

## Expected Inputs

- The design or change under assessment, and its acceptance criteria.
- The Review Level/Complexity classification (see `architecture-review-classification`) — sets how much depth is proportionate: Level 1/Low can use a brief pass over categories; Level 2-3/High-Strategic warrants full detail on every materially impacted category and a populated register even for minor risks.
- Any existing risk register or debt register entries for the affected area.

## NFR Categories

Evaluate all 12 categories every time: security, performance, scalability, availability, maintainability, supportability, observability, auditability, compliance, recoverability, data residency, accessibility (where user-facing).

Treat **observability** (monitoring, telemetry, logging, alerting, diagnostics) as first-class — not implicitly covered by supportability.

For each category, classify materiality as **Impacted**, **Low relevance**, or **Not applicable**:

- **Impacted** — give a full assessment: expectation, how the design meets it, risks/mitigations.
- **Low relevance / Not applicable** — state this plus a one-line reason. Never omit a category silently.

## Risk Register

One entry per risk: **Description**, **Category** (technical, security, operational, cost, licensing, technical debt, architecture debt), **Likelihood**, **Impact**, **Mitigation**, **Owner** (or "unassigned").

## Technical and Architecture Debt Register

Assess separately, report together where relevant:

- **Technical debt** — implementation-level shortcuts.
- **Architecture debt** — structural/pattern-level compromises affecting the wider solution landscape (e.g. temporary exceptions, duplicate/inconsistent patterns, transitional integration approaches, deviations from approved standards, legacy constructs retained for compatibility).

One entry per item: **Description**, **Rationale**, **Classification** (Accepted/Deferred/Mitigated/Eliminated/Unknown), **Review or retirement criteria** (where applicable).

## Workflow

1. Confirm the Review Level/Complexity classification to scale analysis depth.
2. Assess all 12 NFR categories; detail impacted ones, give a one-line rationale for the rest.
3. Populate the Risk Register from risks surfaced across all categories.
4. Populate the Debt Register from technical/architecture debt introduced, resolved, or left in place.
5. Hand the three outputs back to the calling agent, flagging anything meeting the Escalation Guidance below for prominent placement in the artefact.

## Output Standards

- **NFR Assessment** — one row/section per category, in the order listed above: category, materiality, expectation (if impacted), how the design meets it (if impacted), risks/mitigations (if impacted).
- **Risk Register** — table: Description / Category / Likelihood / Impact / Mitigation / Owner.
- **Debt Register** — table: Description / Rationale / Classification / Review or retirement criteria.

## Escalation Guidance

Flag these for stakeholder visibility rather than absorbing them silently:

- Any risk rated high likelihood **and** high impact with no identified mitigation or owner.
- Any architecture debt classified **Accepted** or **Unknown** without a clearly justified rationale.
- Any NFR gap the design cannot resolve (e.g. a compliance or data residency requirement the platform cannot meet) — flag as a blocking risk, never as low relevance.
