---
description: "Use when writing, reviewing, or updating tests in Defra.Imports.IntegrationTests, or discussing test data/scenario setup for Dataverse/Logic Apps/Service Bus integration tests. Covers Marktek.Fluent.Testing.Engine deprecation and the required use of Defra.Imports.Scenarios/ScenarioBuilder."
applyTo: "tests/Defra.Imports.IntegrationTests/**"
---

# Integration Test Setup Standards

`Marktek.Fluent.Testing.Engine` and `MarkTek.Fluent.Testing.RecordGeneration` are **legacy**. They remain only to support existing tests that have not yet been migrated — do not extend, copy, or reuse their patterns in new work.

## Rule

All **new** integration tests must build their test data and pre-conditions using:

- [Defra.Imports.Scenarios](../../tests/Defra.Imports.Scenarios/) — the shared scenario/data-builder library.
- The `ScenarioBuilder` package's chain-of-builder pattern, combined with `Bogus` fakers, to construct realistic Dataverse records via the API rather than hand-rolled record generation.

Do not add a `PackageReference` to `Marktek.Fluent.Testing.Engine` or `MarkTek.Fluent.Testing.RecordGeneration` for new test projects or new test areas.

## What this means in practice

- **Test setup / pre-conditions**: use or extend a scenario step builder in `Defra.Imports.Scenarios` instead of a Marktek record generator. If the scenario you need doesn't exist yet, add a new step builder there rather than a Marktek-based one in `Defra.Imports.IntegrationTests`.
- **Read-only scenario reuse**: pass a `cacheKey` to `BuildAsync` where the scenario only performs reads afterwards, to avoid redundant record creation.
- **Incremental builds**: chain scenario steps by passing the previously built scenario (and the `out` last-built-step) into subsequent `BuildAsync` calls, rather than re-running an entire builder chain.
- **Existing Marktek-based tests**: leave working tests as-is unless the ticket specifically asks for migration. Don't block unrelated changes on a rewrite.
- **Migrating an existing test**: if you touch a test that still uses Marktek and a suitable scenario/step builder already exists in `Defra.Imports.Scenarios`, prefer switching it over as part of the change.

## Reference

See [CONTRIBUTING.md](../../CONTRIBUTING.md) "Scenario builder" section for the full builder-pattern rationale, and the `integration-test-design` skill for the broader integration test design process.
