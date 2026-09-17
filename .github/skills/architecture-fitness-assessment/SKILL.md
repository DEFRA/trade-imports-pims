---
name: architecture-fitness-assessment
description: 'Run the final quality gate on an architecture artefact before it is finalised or published: verify traceability to source work items and acceptance criteria, confirm every recommendation carries a Decision Confidence rating, confirm NFR/debt/ALM/non-breaking-change coverage is complete, and assemble the artefact into its correct standard structure (Solution Architecture Document, Design Review Report, Architecture Review Summary, Operational Support Model, Reusable Capability Assessment). Use as the last step before any architecture artefact is presented, committed, or linked from a work item. Reusable by any architecture-focused agent.'
---

# Architecture Fitness Assessment

## Purpose

Confirm that an architecture artefact produced by combining the outputs of other skills (`architecture-review-classification`, `architecture-options-analysis`, `power-platform-architecture-design`, `adr-management`, `risk-and-debt-analysis`) is complete, internally consistent, traceable, and structured to the repository's expected artefact templates before it is published. This skill is the single place document-assembly standards live, so calling agents do not need to hold document templates themselves — it does not perform the underlying architectural reasoning, only verifies and assembles it.

## Trigger Conditions

Use this skill:

- Before finalising or publishing any SAD, solution design, ADR, Design Review Report, Architecture Review Summary, or Operational Support Model.
- Before linking an architecture artefact back to a work item.
- When assembling the outputs of multiple other skills into a single stakeholder-facing document.

## Expected Inputs

- The draft artefact content and the outputs of the other skills that contributed to it (classification, options analysis, Power Platform/ALM guidance, ADR content, risk/debt content).
- The originating work item(s) and their acceptance criteria.

## Responsibilities

### Architecture Traceability

Maintain traceability from business objectives to implemented components: Business Objective → Epic → Feature → User Story → Acceptance Criteria → Solution Design/SAD → ADRs → Solution Components.

- Confirm every recommendation, design artefact, ADR, review report, and Architecture Review Summary identifies its originating work item(s) and traces to one or more acceptance criteria.
- Make explicit where a design influences multiple Epics/Features/User Stories.

### Architecture Fitness checklist

Before finalising or publishing any output, verify:

- Alignment with approved ADRs (including whether an existing ADR was reviewed and superseded/updated rather than duplicated), existing architectural principles, and Power Platform guidance.
- Architecture Review Level and Complexity have been classified and the depth of analysis/artefacts matches them.
- Reuse opportunities and existing approved patterns have been considered.
- Acceptance criteria are satisfied and relevant NFRs (including observability) have been addressed.
- Technical debt and architecture debt have each been identified and classified.
- Any material divergence between implementation and approved architecture has been recorded, with impact/risk assessed and a remediation path recommended.
- Licensing, cost, and ALM impact have been assessed — including the migration path for any new configuration/reference record, not left as an implicit manual-creation assumption.
- The design contains no manual, undocumented deployment/configuration step.
- No existing contract/integration is broken; any component change/retirement removes dependencies first, marks components obsolete rather than deleting them outright, and updates `architecture/deprecated-components.md`.
- Operational support requirements and security implications have been considered.
- Incremental delivery has been considered and the Minimum Viable Implementation identified where appropriate.
- Recommendation confidence has been assessed and stated for every significant recommendation; assumptions distinguished from facts; validation activities identified where confidence is Medium or Low.

### Document assembly standards

Assemble the verified content into the appropriate standard structure:

**Solution Architecture Document (SAD) / solution design:**
Requirement summary and work item link; solution vision and guiding principles (where not produced separately); options considered and why the recommendation was chosen; chosen approach and components (context/logical/physical diagrams as needed); how each acceptance criterion and NFR is satisfied; Reusable Capability Assessment; security design considerations; integration architecture considerations; ALM Considerations; Technical/Architecture Debt Impact; risks and mitigations; cost and licensing impact; dependencies and cross-solution impact; operational support implications; Minimum Viable Implementation/incremental plan; Decision Confidence; open questions/assumptions.

**Reusable Capability Assessment:** existing capabilities reused/extended; new reusable capabilities introduced; configuration-driven behaviours introduced; expected future reuse opportunities; alternative requirement-specific approaches considered; justification for any reusable abstractions; why the chosen approach best balances reuse and simplicity.

**Design Review Report:** structured findings, each with Severity (Critical/High/Medium/Low), Description, Impact, Recommendation (with Decision Confidence), Priority. Record any material divergence between implementation and approved architecture explicitly, with impact/risk and a remediation path.

**Architecture Review Summary** (lightweight, PR-adjacent): Recommendation; Alternatives Considered; Key Architectural Risks; Cost Implications; ADR References; Required Approvals; Decision Confidence; Open Questions.

**Operational Support Model:** monitoring/alerting approach, escalation path, known limitations, and any manual/operational processes required once live.

## Workflow

1. Collect the outputs of the contributing skills for the artefact being assembled.
2. Confirm Architecture Traceability from work item through to solution components.
3. Work through the Architecture Fitness checklist item by item; do not proceed to publish while any item is unresolved.
4. Assemble the content into the correct document structure for the artefact type, scaled to the Review Level/Complexity classification (a Level 1/Low change needs only an Architecture Review Summary, not a full SAD).
5. State what was included and what was deliberately omitted as disproportionate, and why.

## Output Standards

The finished artefact must use British English, lead with outcome/impact for executive readers before technical detail, use consistent terminology, avoid implementation detail not required by an acceptance criterion, and follow a clear-headings, summary-first, detail-below structure.

## Escalation Guidance

- Any unresolved checklist item blocks publication — return it to the relevant contributing skill (or the calling agent) rather than publishing an incomplete artefact.
- Escalate any material divergence between implementation and approved architecture found during assembly, with impact/risk and a recommended remediation path, rather than resolving it silently.
- Escalate Medium/Low overall confidence, or unresolved Critical/High Design Review Report findings, to stakeholders or the Product Analyst before the artefact is treated as final.
