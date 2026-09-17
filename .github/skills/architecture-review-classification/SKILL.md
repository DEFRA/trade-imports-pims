---
name: architecture-review-classification
description: 'Classify a requirement (Epic/Feature/User Story) or a proposed design change into an Architecture Review Level (1 Pattern Conformance / 2 Pattern Extension / 3 New Pattern-Strategic Change) and an Architecture Complexity rating (Low/Medium/High/Strategic), and perform pattern-fit triage against existing approved ADRs and patterns. Use before producing any architecture artefact, to scale the depth of analysis, the number of options evaluated, and the artefact set produced (SAD, ADR, Architecture Review Summary) to the size, risk, and strategic impact of the change. Used by the Solution Architect agent (or any future architecture-focused agent) as the first classification gate in every design workflow.'
---

# Architecture Review Classification

## Intended Agent

This skill may only be used by the Solution Architect agent.

If the current agent is not the Solution Architect agent:

- Stop.
- Explain that this skill is owned by the Solution Architect.
- Recommend handing off to the Solution Architect agent.

## Purpose

Determine, before any architecture artefact is produced, whether a requirement or design change fits an existing approved pattern, extends one, or requires a new pattern — and how much analytical depth and documentation is proportionate to it. This prevents both under-analysis of strategic change and over-documentation of routine, pattern-conformant work.

## Boundaries

This skill **classifies only**. It does not evaluate design options (`architecture-options-analysis`), write ADRs (`adr-management`), or assess risk/debt (`risk-and-debt-analysis`) — its output tells the calling agent which of those activities to invoke, and at what depth.

## Trigger Conditions

Use this skill whenever:

- A new Epic/Feature/User Story requires an architectural recommendation, SAD, or ADR.
- An existing design or implementation is being reviewed and its scope of impact is unclear.
- It is unclear whether a proposed change is routine or strategic, or whether an existing ADR already governs the area.

## Expected Inputs

- The requirement/acceptance criteria or the proposed change description.
- Any existing ADRs and architecture artefacts under `/architecture` relevant to the affected area.
- Awareness of existing solutions, tables, flows, plug-ins, and connectors that may already implement a similar pattern.

## Process

1. **Gather context** — read the requirement/change description and search `/architecture` and `/architecture/adr` for governing patterns.
2. **Pattern-fit triage** — state whether the requirement:
   - (a) **Fits** an existing approved pattern/ADR,
   - (b) **Extends** an existing approved pattern, or
   - (c) **Requires** a new pattern or ADR.

   Where an approved ADR already governs the area, treat it as the architectural source of truth and check the requirement against it rather than designing independently.

3. **Classify Architecture Review Level** using the table below:

   | Level | Fits when… | Expected output |
   |---|---|---|
   | **1 – Pattern Conformance** | Fits an existing approved pattern; no material change to direction; no significant security/integration/licensing/operational impact; covered by existing ADRs | Lightweight architecture assessment or Architecture Review Summary; references to relevant ADRs/patterns; no new ADR or SAD required |
   | **2 – Pattern Extension** | Extends an existing approved pattern but stays consistent with direction; moderate design considerations; limited security/operational/integration/ALM/licensing impact | Targeted architecture assessment; ADR update or new ADR where justified; only the design artefacts necessary to communicate the change |
   | **3 – New Pattern / Strategic Change** | Introduces a new pattern, materially affects solution direction, or has significant cross-solution impact; significant integration/security/ALM/operational/licensing implications; strategic consequences | Full architecture assessment; ADR creation; SAD and supporting artefacts where justified |

4. **Classify Architecture Complexity** as **Low / Medium / High / Strategic**, based on:
   - Number of systems affected
   - Security impact
   - Integration complexity
   - Licensing impact
   - ALM impact
   - Operational impact
   - Cross-solution impact
   - Data model impact
   - Architectural novelty

5. **Reconcile mismatches** — a low Review Level with unexpectedly high Complexity (or vice versa) is a signal to reconsider the classification before proceeding; state the reconciled classification and why.

6. **Recommend the proportionate artefact set** for the calling agent to produce (e.g. "Level 1/Low → Architecture Review Summary only; no ADR"), so downstream skills (`adr-management`, `architecture-fitness-assessment`) are invoked at the right depth, not by default.

## Output Standards

Return a short classification block:

- **Pattern Fit** — Fits / Extends / New, with the governing ADR(s) referenced if any.
- **Review Level** — 1, 2, or 3, with rationale.
- **Complexity** — Low/Medium/High/Strategic, with the driving factors named.
- **Recommended Artefact Set** — which of SAD, ADR, Architecture Review Summary, Design Review Report are proportionate.
- **Confidence** — how certain the classification is, and what would change it (e.g. missing information about cross-solution impact).

## Escalation Guidance

- If classification cannot be determined because the requirement is ambiguous or missing acceptance criteria, do not guess — flag the gap and recommend the requirement be routed back to the Product Analyst before classification proceeds.
- If pattern-fit triage surfaces a conflict between the proposed change and an existing approved ADR, surface the conflict explicitly rather than resolving it silently; recommend it be raised for a stakeholder decision.
