# AGENTS.md

## Purpose

This repository contains source code, configuration, documentation, architecture assets, and delivery artefacts for a Power Platform package.

All agents working in this repository must follow the guidance in this document regardless of their individual specialisation.

The objectives are:

- Maintain a high standard of quality and consistency
- Follow documented architecture and engineering standards
- Minimize unnecessary rework
- Ensure traceability from requirements through implementation and testing
- Preserve maintainability and operational readiness

---

# Repository Context

This repository delivers:

- Business capabilities through software solutions
- Supporting documentation
- Automated testing
- Deployment and operational artefacts

Primary stakeholders may include:

- Product Owners
- Business Analysts
- Solution Architects
- Developers
- Test Engineers
- Platform Administrators
- Operations Teams

---

# Source of Truth

When information conflicts, use the following precedence order:

1. Explicit user instructions
2. Approved Architecture Decision Records (ADRs)
3. Approved design documentation
4. Repository standards and conventions
5. Existing implementation
6. Assumptions

Do not override documented architectural decisions without clearly identifying the conflict.

Do not treat existing code as authoritative when a documented decision exists.

---

# Engineering Principles

All agents must:

- Prefer clarity over cleverness
- Prefer maintainability over short-term optimisation
- Prefer simple solutions over unnecessary complexity
- Reduce duplication where practical
- Follow established patterns before introducing new ones
- Make assumptions explicit
- Leave artefacts in a better state than they were found

Avoid:

- Premature optimisation
- Unnecessary abstractions
- Hidden dependencies
- Large-scale refactoring without justification
- Introducing new technologies without clear rationale

---

# Architecture Principles

When proposing or implementing changes:

- Align with documented architecture
- Maintain separation of concerns
- Minimise coupling
- Maximise cohesion
- Consider scalability
- Consider security
- Consider supportability
- Consider operational impact

Where trade-offs exist, explain them clearly.

---

# Security Requirements

All agents must:

- Follow least-privilege principles
- Never expose secrets, credentials, or tokens
- Avoid hard-coded credentials
- Consider data classification requirements
- Validate inputs where applicable
- Consider authentication and authorization impacts
- Highlight security concerns when identified

If security implications are uncertain, raise them explicitly.

---

# Quality Standards

All delivered work should be:

- Understandable
- Testable
- Maintainable
- Traceable
- Documented appropriately

Changes should:

- Have clear business value
- Align with requirements
- Include appropriate validation
- Avoid introducing unnecessary technical debt

---

# Documentation Standards

All documentation must be written in clear, concise, grammatically correct British English. Documentation should be free from spelling and typographical errors and use consistent terminology throughout.

Documentation should be updated when changes affect:

- Behaviour
- Architecture
- Deployment
- Operations
- Configuration
- APIs
- User processes

Documentation should explain:

- Why a decision was made
- What was implemented
- Operational considerations
- Known limitations

---

# Testing Expectations

Where applicable:

- Define testable acceptance criteria
- Validate expected behaviour
- Consider positive and negative scenarios
- Consider edge cases
- Consider regression impact

A change should not be considered complete solely because implementation is finished.

Verification is required.

---

# Repository Structure

Use the following conventions unless existing repository standards specify otherwise.

```text
/docs
    Business, functional and technical documentation

/architecture
    ADRs, architecture diagrams, design decisions

/src
    Source code

/tests
    Automated tests

/scripts
    Utilities and automation scripts

/pipelines
    CI/CD definitions
```

Place new content in the most appropriate location.

Avoid creating new top-level folders without clear justification.

---

# Change Management

Before making significant changes:

- Understand the existing design
- Review relevant documentation
- Review related code
- Identify dependencies
- Assess impacts

For significant decisions:

- Record rationale
- Document assumptions
- Document constraints
- Document trade-offs

---

# Definition of Done

Work is complete only when:

- Requirements are satisfied
- Acceptance criteria are addressed
- Quality checks have been performed
- Documentation has been updated where appropriate
- Risks have been identified
- Dependencies have been documented
- Outstanding assumptions are highlighted

---

# Agent Collaboration

Agents should operate within their area of responsibility while collaborating effectively with specialists.

Examples:

- Requirements concerns should be routed to requirements-focused agents
- Architecture concerns should be routed to architecture-focused agents
- Implementation concerns should be routed to development-focused agents
- Quality concerns should be routed to testing-focused agents

Do not make specialist decisions outside your domain when a dedicated specialist is available.

---

# Communication Standards

When producing recommendations:

- State assumptions
- Explain reasoning
- Identify risks
- Highlight alternatives where appropriate
- Clearly distinguish facts from opinions

When uncertainty exists:

- Acknowledge uncertainty
- Describe available options
- Recommend next steps

Do not present assumptions as facts.

---

# Preferred Behaviour

Agents should:

- Read existing artefacts before creating new ones
- Reuse existing patterns where appropriate
- Respect established standards
- Make incremental improvements
- Minimise disruption to existing solutions

Agents should not:

- Rewrite working solutions without justification
- Introduce unnecessary complexity
- Ignore documented decisions
- Create duplicate documentation
- Invent requirements that are not supported by available information

---

# Repository-Specific Standards

## Environments

This Power Platform package is developed using an ephemeral approach to development and test environments. Development and CI environments are clearly distinguished by their URLs. 

All work is developed and tested in isolation in these environments before being merged into main.

## Coding

Coding standards are defined in the relevant instructions files.

## Power Platform

We prefer a low-code approach to implementation. A good rule of thumb is that any code written should be **highly reusable** and **unlikely to change** (e.g. platform extensions rather than business processes. This means making CWAs and plug-ins as configurable, generic, and granular as possible.

Business logic primarily lives in declarative components such as workflows, actions, business process flows, and cloud flows. Atomic processes should be encapsulated as actions.

Plug-in handlers are rarely required; they are only needed when you must alter the behaviour of managed actions or trigger logic on messages not supported by workflows or flows. Plug-in steps don't offer the scoping functionality of workflows and flows (especially important when dealing with out-of-the-box components) or the input & output functionality found in actions and custom workflow activities (meaning less flexible and reusable).

An environment may have many solutions deployed to it over its lifetime. For this reason, we must ensure that our solution is future-proof and compatible with other solutions that may be introduced. This principle can be applied in countless different ways, but some examples might be:

- Avoid organisation scoped processes or plug-in handlers on out-of-the-box actions for out-of-the-box entities as this may prevent these entities from being reused by other parts of the business
- Avoid customising managed forms or views as other solutions can also introduce changes to these, create new forms for your app instead

## Versioning

Solutions are versioned automatically based on the Git history. Ensure that you write commit and pull request titles that conform to Conventional Commits. A Git commit where there are changes within a solution metadata folder counts as a version increment for that solution. 

| Commit message                | Increment | Explanation                                                                    |
| ----------------------------- | --------- | ------------------------------------------------------------------------------ |
| feat!: my breaking feature    | Major     | Exclamation is present after the commit type.                                  |
| feat: my non-breaking feature | Minor     | No exclamation is present after the commit type and commit type is 'feat'.     |
| fix: my non-breaking bug fix  | Patch     | No exclamation is present after the commit type and commit type is not 'feat'. |

If you're making changes for a solution (e.g. plug-in or web resource changes) that sit outside the solution metadata folder, you must increment the version for the solution manually by including one of the following lines in the commit body:

`+solutionVer(<solutionName>): major`
`+solutionVer(<solutionName>): minor`
`+solutionVer(<solutionName>): patch`

Where `<solutionName>` is replaced by the unique name of the solution.

## Release Management

We require Azure Boards work items to be linked to GitHub pull requests for traceability. We must use the `AB#12345` syntax in both the pull request title and body.
