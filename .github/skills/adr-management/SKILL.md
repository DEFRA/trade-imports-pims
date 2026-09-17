---
name: adr-management
description: 'Create, update, and govern Architecture Decision Records (ADRs) under /architecture/adr. Reviews existing ADRs before drafting a new one, updates or supersedes rather than duplicates, applies the repository ADR template, links related requirements and related/superseded ADRs, and keeps the ADR set internally consistent over time. Use whenever a decision has lasting consequences (new/extended pattern, implementation pattern choice, cross-solution dependency, cost trade-off, security/integration approach) or an existing ADR needs updating, superseding, or reconciling against a conflicting artefact. Reusable by any architecture-focused agent that produces governed decisions.'
---

# ADR Management

## Intended Agent

This skill may only be used by the Solution Architect agent.

If the current agent is not the Solution Architect agent:

- Stop.
- Explain that this skill is owned by the Solution Architect.
- Recommend handing off to the Solution Architect agent.

## Purpose

Ensure decisions with lasting architectural consequence are recorded once, consistently, and kept authoritative over time — so approved ADRs remain the reliable architectural source of truth (outranking existing code, documentation, requirements, and any proposed design when they conflict).

This skill owns only the ADR artefact and its lifecycle. It does not perform options analysis (see `architecture-options-analysis`), classify review depth or pattern-fit (see `architecture-review-classification`), produce risk/debt findings (see `risk-and-debt-analysis`), or maintain other architecture artefacts such as SADs and diagrams (see `architecture-fitness-assessment`) — it consumes their output when populating an ADR.

## Trigger Conditions

Use this skill whenever a decision needs recording or an existing ADR needs to change: a new/extended pattern, an implementation pattern choice, a cross-solution dependency, a cost trade-off, a security/integration approach, a status change (e.g. Superseded, Deprecated), or a detected conflict between an approved ADR and current implementation/documentation/a proposed design.

## Expected Inputs

- The decision to record: rationale, alternatives, and Decision Confidence (from `architecture-options-analysis`).
- The originating work item(s) (Epic/Feature/User Story) and acceptance criteria the decision supports.
- Existing ADRs under `/architecture/adr` in the affected area.
- The pattern-fit conclusion (fits/extends/new) from `architecture-review-classification`, where already available.
- Risk and debt findings (from `risk-and-debt-analysis`), where relevant to Consequences.

## Responsibilities

1. **Review before writing.** Search `/architecture/adr` for existing decisions in the affected area. Update or supersede an existing ADR where the decision has changed, rather than creating a duplicate.
2. **Use the repository template.** Start from [0000-template.md](../../../architecture/adr/0000-template.md) for every new ADR, copied to the next numbered file (e.g. `architecture/adr/0004-title.md`).
3. **Populate every template section**, drawing on other skills' output rather than re-deriving it:
   - Title, Status, Date, Decision Confidence.
   - Context — the problem and constraints.
   - Decision — what was decided, stated plainly.
   - Alternatives Considered — from `architecture-options-analysis`.
   - Consequences — positive, negative, cost/licensing impact, and impact on other solutions/environments; technical debt impact and architecture debt impact each rated **Reduces / Neutral / Increases / Unknown**, informed by `risk-and-debt-analysis` findings where available.
   - Related requirement(s) and Related ADR(s), including any ADR this one supersedes.
   - Open Questions / Assumptions.
4. **Link superseded ADRs in both directions** — update the superseded ADR's status and cross-link it to its replacement.
5. **Keep the ADR set internally consistent** — an ADR that no longer reflects reality must be updated, superseded, or marked Deprecated; do not leave two ADRs in force that contradict each other.
6. **Surface conflicts; never resolve them silently.** Where implementation, documentation, requirements, or a proposed design conflicts with an approved ADR, state the conflict explicitly and recommend a resolution path (update the ADR, correct the conflicting artefact, or escalate for a stakeholder decision) — do not proceed to author or override the ADR unilaterally.

## Workflow

1. Search `/architecture/adr` for ADRs governing the affected area; determine new ADR / update / supersession.
2. Copy the template (for a new or superseding ADR) and populate every section (Responsibility 3), pulling Decision Confidence and Alternatives from `architecture-options-analysis` and debt ratings from `risk-and-debt-analysis`.
3. Cross-link related requirements and related/superseded ADRs; set status on all affected ADRs.
4. If a conflict with an approved ADR was found, apply Responsibility 6 instead of proceeding.

## Output Standards

Every ADR must contain, in the template structure: Title, Status, Date, Decision Confidence, Context, Decision, Alternatives Considered, Consequences (cost/licensing impact, cross-solution impact, technical debt impact, and architecture debt impact each explicitly rated), Related requirement(s), Related ADR(s) (including any superseded), and Open Questions/Assumptions.

## Escalation Guidance

Where a proposed decision conflicts with an approved ADR, or the recommendation behind it is Medium/Low confidence, do not present the ADR as settled: state the conflict or the confidence rating and its rationale, and recommend escalation or validation activities rather than resolving the uncertainty unilaterally.
