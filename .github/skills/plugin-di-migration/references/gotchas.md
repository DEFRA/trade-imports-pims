# Gotchas

Mistakes that earlier migrations made or nearly made. Read this before the first edit. Code pitfalls (language version, copying and cloning entities, `.projitems` entries, `using` order) are in [csharp-source.instructions.md](../../../instructions/csharp-source.instructions.md).

## Searching

- `git grep` skips untracked files, so it misses everything created in this session. Use a filesystem search, as in brief W1 of [worker-briefs.md](./worker-briefs.md).
- Match whole words. A fragment such as `WatchFlag` also matches the unrelated `AssociateWatchFlags*` files, both in searches and in `-PathFilter`.

## Projects

- These `.projitems` files glob `**\*.cs`:
  - `Defra.Imports.Application`;
  - `Defra.Imports.Dataverse`;
  - `Defra.Imports.Repositories`.
- These list every file:
  - `Defra.Imports.Infrastructure`;
  - `Defra.Imports.BusinessLogic`.
- A stale entry in either breaks both heads, and so does a missing one.
- The unit test project is SDK-style and globs its files, so test files need no project entry.
- A namespace move touches unmigrated files in both heads. Rebuild both: the gate does.

## Generated model

- Never open whole generated files. `defraimp_importernotification.cs` is about 200 KB and `systemuser.cs` is larger. Use `Get-DataverseEntityPropertyClrType.ps1` (see step 2 of the skill).
- Casing differs between entities, for example `defraimp_Name` on one and `defraimp_name` on another. This applies to class names, properties and `Fields` constants, so look up each one.
- `Clone<T>` on an early-bound entity with an empty Id is already handled. Do not reimplement it.
- A `Create` `Target` holds only the attributes that were supplied; every other attribute reads as null.
- A missing table or column means the model needs regenerating. Stop and ask (ground rule 3).

## PowerShell

- Each call starts in a fresh process, so `cd` does not persist. Use absolute paths with .NET file APIs.
- `pwsh -File` passes an array argument as one string. So call `Get-DataverseEntityPropertyClrType.ps1` with `-Command` when you pass several attribute names. `Get-PluginStepRegistration.ps1` and `-PathFilter` split commas themselves.
- Long output is truncated and saved to a temporary file. Read it with a bounded `view` range or a targeted search, never in full.
- One build at a time per working tree. The verification scripts queue behind each other; IDE builds and direct `dotnet build` calls do not.

## File tools

- Write repository files with `create` and `edit` only, never from PowerShell.
- `edit` needs a non-empty `old_str` that matches the current text exactly. View the file first, because earlier edits may have changed it.
- `create` cannot overwrite a file, and its parent folder must exist. Create folders with `New-Item -ItemType Directory -Path <absolute path>`.
- Do not make bulk regex edits; they have corrupted files before.
- After a series of edits, check for stray runs of spaces and missing blank lines.
- Delete with `Remove-Item -LiteralPath`, never `git rm`, so the index is left alone.
