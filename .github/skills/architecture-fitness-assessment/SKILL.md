---
name: architecture-fitness-assessment
description: 'Run the final quality gate on an architecture artefact before it is finalised or published: verify traceability to source work items and acceptance criteria, confirm every recommendation carries a Decision Confidence rating, confirm NFR/debt/ALM/non-breaking-change coverage is complete, and assemble the artefact into its correct standard structure (Solution Architecture Document, Design Review Report, Architecture Review Summary, Operational Support Model, Reusable Capability Assessment). Use as the last step before any architecture artefact is presented, committed, or linked from a work item. Reusable by any architecture-focused agent.'
---

# Architecture Fitness Assessment

## Intended Agent

This skill may only be used by the Solution Architect agent.

If the current agent is not the Solution Architect agent:

- Stop.
- Explain that this skill is owned by the Solution Architect.
- Recommend handing off to the Solution Architect agent.

## Purpose

Verify that an architecture artefact assembled from other skills' output — `architecture-review-classification`, `architecture-options-analysis`, `power-platform-architecture-design`, `adr-management`, `risk-and-debt-analysis` — is complete, traceable, and correctly structured before it is published. This skill is a **verification and assembly gate**: it checks that each contributing skill's output is present and consistent, and holds the document templates so calling agents don't have to. It does not re-derive classification, options, ALM guidance, ADR content, or risk/debt findings — a gap found here is sent back to the owning skill, not fixed in place.

## Trigger Conditions

Use this skill before finalising, publishing, or linking to a work item any SAD, solution design, ADR, Design Review Report, Architecture Review Summary, or Operational Support Model — i.e. whenever the outputs of multiple other architecture skills are being assembled into one stakeholder-facing document.

## Expected Inputs

- The draft artefact and the outputs of the skills that contributed to it (classification, options analysis, Power Platform/ALM guidance, ADR content, risk/debt content).
- The originating work item(s) and their acceptance criteria.

## Responsibilities

### 1. Traceability

Confirm the chain Business Objective → Epic → Feature → User Story → Acceptance Criteria → Solution Design/SAD → ADRs → Solution Components holds:

- Every recommendation, ADR, review report, and Architecture Review Summary names its originating work item(s) and the acceptance criteria it satisfies.
- Where a design influences multiple Epics/Features/User Stories, this is stated explicitly.

### 2. Fitness checklist

Verify each item below is present and adequately covered — do not re-perform the underlying analysis, only confirm the owning skill's output was applied and return any gap to that skill:

| Check | Owning skill | Fails when |
|---|---|---|
| Review Level and Complexity classified; artefact set/depth matches | `architecture-review-classification` | No classification stated, or artefact depth doesn't match it |
| Alignment with approved ADRs and existing patterns; reuse considered | `adr-management` / `architecture-options-analysis` | Conflicting ADR not surfaced, or no reuse consideration recorded |
| Options evaluated with trade-offs and a Decision Confidence on every significant recommendation | `architecture-options-analysis` | A recommendation has no alternatives, rationale, or confidence rating |
| NFRs (incl. observability) assessed; risk register and debt classifications complete | `risk-and-debt-analysis` | Any NFR category silently omitted, or debt/risk left unclassified |
| ALM path defined for any new configuration/reference data; no manual/undocumented deployment step | `power-platform-architecture-design` | A new config/reference record has no stated migration mechanism |
| No broken contract/integration; obsolete components marked (not deleted) and logged in `architecture/deprecated-components.md` | `power-platform-architecture-design` | Unresolved dependents on a retired component, or the log wasn't updated |
| Operational support and security implications considered | `risk-and-debt-analysis` | Support/security not addressed for a material change |
| Incremental delivery / Minimum Viable Implementation considered where appropriate | calling agent | No MVI discussion for a multi-stage change |
| Acceptance criteria satisfied | calling agent | A stated acceptance criterion isn't addressed anywhere in the artefact |

Any material divergence between implementation and approved architecture must be recorded with impact/risk and a remediation path — do not resolve it silently.

### 3. Document assembly standards

Assemble the verified content into the appropriate standard structure:

**Solution Architecture Document (SAD) / solution design:**
Requirement summary and work item link; solution vision and guiding principles (where not produced separately); options considered and why the recommendation was chosen; chosen approach and components (context/logical/physical diagrams as needed); how each acceptance criterion and NFR is satisfied; Reusable Capability Assessment; security design considerations; integration architecture considerations; ALM Considerations; Technical/Architecture Debt Impact; risks and mitigations; cost and licensing impact; dependencies and cross-solution impact; operational support implications; Minimum Viable Implementation/incremental plan; Decision Confidence; open questions/assumptions.

**Reusable Capability Assessment:** existing capabilities reused/extended; new reusable capabilities introduced; configuration-driven behaviours introduced; expected future reuse opportunities; alternative requirement-specific approaches considered; justification for any reusable abstractions; why the chosen approach best balances reuse and simplicity.

**Design Review Report:** structured findings, each with Severity (Critical/High/Medium/Low), Description, Impact, Recommendation (with Decision Confidence), Priority. Record any material divergence between implementation and approved architecture explicitly, with impact/risk and a remediation path.

**Architecture Review Summary** (lightweight, PR-adjacent): Recommendation; Alternatives Considered; Key Architectural Risks; Cost Implications; ADR References; Required Approvals; Decision Confidence; Open Questions.

**Operational Support Model:** monitoring/alerting approach, escalation path, known limitations, and any manual/operational processes required once live.

## Workflow

1. Collect the outputs of the contributing skills for the artefact being assembled.
2. Confirm traceability from work item through to solution components (Responsibility 1).
3. Work through the fitness checklist row by row (Responsibility 2); do not publish while any row is unresolved.
4. Assemble the content into the correct document structure for the artefact type, scaled to the Review Level/Complexity classification (a Level 1/Low change needs only an Architecture Review Summary, not a full SAD).
5. State what was included and what was deliberately omitted as disproportionate, and why.

## Output Standards

The finished artefact must use British English, lead with outcome/impact for executive readers before technical detail, use consistent terminology, avoid implementation detail not required by an acceptance criterion, and follow a clear-headings, summary-first, detail-below structure.

## Escalation Guidance

- Any unresolved checklist item blocks publication — return it to the relevant contributing skill (or the calling agent) rather than publishing an incomplete artefact.
- Escalate any material divergence between implementation and approved architecture found during assembly, with impact/risk and a recommended remediation path, rather than resolving it silently.
- Escalate Medium/Low overall confidence, or unresolved Critical/High Design Review Report findings, to stakeholders or the Product Analyst before the artefact is treated as final.
