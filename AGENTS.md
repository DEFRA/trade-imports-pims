# AGENTS.md

## Purpose

This repository contains source code, configuration, documentation, architecture assets, and delivery artefacts for a Power Platform package.

All agents working in this repository must follow the guidance in this document regardless of their individual specialization.

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

Please refer to the [CONTRIBUTING.md](./CONTRIBUTING.md)