# Adoption record template

- **Where.** Add the record to ADR 0001 after the last "Adoption Record" section and before "Open Questions / Assumptions".
  - Find both headings with `Select-String -Path architecture/adr/0001-plugin-workflow-di-scaffold.md -Pattern '^## (Adoption Record|Open Questions)'`.
- **Style.** Match the existing records' wording and level of detail, and omit any bullet that does not apply.
- **What else may change.** Do not edit earlier records or other sections, except convention text that the approved design changed.

```markdown
## Adoption Record: `<PluginA>` and `<PluginB>` (<ordinal> migration)

<What the plug-ins do, in a few words> now follow the pattern. They are proven by unit tests only; they have not been validated in a live Dataverse environment.

- **Entry point identity:** `<full type name>` keeps its name, namespace and assembly, so the registered steps (<stage>, <mode> `<Message>` of `<entity>`, <filtering attributes>, <images>) need no re-registration. The `[CrmPluginRegistration]` attributes were removed; they <agreed with the solution metadata | had drifted from the solution metadata (<how>)>.
- **Layers:** <the handler: what it validates, copies and projects; the extension and context; the application service and the rules and writes it owns; the queries and what they replaced>.
- **Contexts:** <only for a new kind of context, or a new reason for one>.
- **Convention changes:** <only when the approved design changed a convention; name the ADR text that was updated>.
- **Deleted:** <types>, their `.projitems` entries and their <n> test classes (all <n> cases were ported).
- **Behaviour preserved:** <each preserved behaviour, including the catch-and-continue or "no catch">.
- **Step-contract validation:** each handler validates the entity, stage (<stage>), mode (<mode>), message (`<Message>`) and `Target`<, and the `<name>` image,> and fails loudly with `InvalidOperationException`, surfaced as `InvalidPluginExecutionException`. This changes behaviour for a misconfigured registration only. Separately, `PluginBase` wraps any other failure in an `InvalidPluginExecutionException` (the legacy base let the raw exception escape), and the unused legacy trace writer is no longer created.
- **Items for later investigation (preserved, commented in code):** <each flagged item, separated by semicolons>.
```
