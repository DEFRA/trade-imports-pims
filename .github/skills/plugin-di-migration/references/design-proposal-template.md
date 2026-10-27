# Design proposal template

Fill this in, post it, then stop and wait for explicit approval. Keep it factual. Cite the file and member for every behaviour claim.

```markdown
# Design proposal: <PluginA> and <PluginB> (AB#<id> | AB# not supplied)

## Summary
<What the plug-ins do today, in two or three sentences, and the shape they will take.>

## Registrations
From Get-PluginStepRegistration.ps1:

| Plug-in | Message | Entity | Stage | Mode | Filtering attributes | Images |
| --- | --- | --- | --- | --- | --- | --- |

## Layers and files
| Layer | Type | File | New, changed or deleted |
| --- | --- | --- | --- |
| Entry point | <PluginA> | src/pluginassemblies/Defra.Imports.Plugins/<Area>/<PluginA>.cs | changed |
| Handler | <PluginA>Handler | … | new |
| Application service | I<UseCase>, <UseCase> | src/common/Defra.Imports.Application/<Area>/… | new |
| Input context | <Context> and <Entity>Extensions.<Method> | src/common/Defra.Imports.Dataverse/Projections/…, …/Extensions/… | new |
| Queries | <Area>Queries | src/common/Defra.Imports.Dataverse/Queries/… | new |
| Constants | … | … | new or reused |
| Composition root | registrations | src/pluginassemblies/Defra.Imports.Plugins/PluginCompositionRoot.cs | changed |
| Legacy | <BusinessLogic classes> and their .projitems entries | … | deleted |
| Tests | … | tests/Defra.Imports.UnitTests/… | new, ported or deleted |

## Handler contract
- Checks: <entity, stage, mode, message, Target, images>.
- Application calls: <one or two calls and what is passed>.
- Applying the result: <how and when it is persisted, and by whom>.
- Catching: <none | catch-and-continue preserved from <file:member>>.
- Logging: <none | ILogWriter, because <file:member> logs>.

## Behaviour preserved
- <behaviour> (<file:member>)

## Suspected bugs and bad practices (preserved and flagged with TODO comments)
1. <file:member>: <what and why>

## Behaviour changes requiring approval
- Step-contract validation (pre-approved): <each check, per handler>.
- PluginBase standing differences (accepted): wrapping of unexpected failures; the unused legacy trace writer is no longer created; start and completion trace lines.
- <any other change, or "None">

## Convention changes
<A change to ADR 0001 conventions (Layer ownership or the project table), or "None".>

## Test plan
- Characterisation: <n> legacy cases in <classes>; mapping to new tests is in the session notes.
- New tests: <kinds from test-checklist.md, and who writes them>.

## Delegation plan
<Briefs W1–W8 to use, in order, or "none".>

## Questions
1. <question>
```
