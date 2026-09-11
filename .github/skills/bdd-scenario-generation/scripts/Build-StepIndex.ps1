<#
.SYNOPSIS
    Builds a lightweight index of Reqnroll step binding definitions (keyword, capture pattern, source location)
    so the bdd-scenario-generation skill can check for reusable step bindings without loading every step
    definition file into context.

.DESCRIPTION
    Scans C# step definition files for [Given], [When], [Then] and [StepDefinition] attributes and writes a
    YAML index summarising each binding's keyword, capture pattern, source file, and line number.

    The index is a disposable, point-in-time artefact reflecting the current state of the step definitions.
    It must NOT be committed to source control — by default it is written to a new file in the OS temp
    directory so no .gitignore entry or repository cleanup is required. Re-run the script whenever step
    bindings may have changed (e.g. at the start of a new BDD scenario authoring session).

.PARAMETER StepsPath
    Path to the folder containing step definition (.cs) files. Defaults to the repository's Reqnroll spec
    project (tests/Defra.Imports.Specs/StepDefinitions), resolved from the current git repository root.

.PARAMETER OutputPath
    Path to write the YAML index to. Defaults to a new file in the OS temp directory.

.EXAMPLE
    ./Build-StepIndex.ps1
    Builds the index using default paths and prints the output file path.

.EXAMPLE
    ./Build-StepIndex.ps1 -StepsPath "tests/Defra.Imports.Specs/StepDefinitions" -OutputPath "$env:TEMP/step-index.yml"
#>

#requires -Version 7.0
[CmdletBinding()]
param(
    [string]$StepsPath,
    [string]$OutputPath
)

$ErrorActionPreference = "Stop"

$repoRoot = $null
try {
    $repoRoot = (git rev-parse --show-toplevel 2>$null)
    if ($LASTEXITCODE -ne 0) { $repoRoot = $null }
} catch {
    $repoRoot = $null
}
if ($repoRoot) { $repoRoot = $repoRoot.Trim() }

if (-not $StepsPath) {
    if (-not $repoRoot) {
        throw "Unable to determine repository root. Run this script from within the trade-imports-pims repository, or pass -StepsPath explicitly."
    }

    $StepsPath = Join-Path $repoRoot "tests/Defra.Imports.Specs/StepDefinitions"
}

if (-not (Test-Path $StepsPath)) {
    throw "Steps path not found: $StepsPath"
}

if (-not $OutputPath) {
    $OutputPath = Join-Path ([System.IO.Path]::GetTempPath()) "bdd-step-index-$([guid]::NewGuid().ToString('N')).yml"
}

# Relative paths in the index are anchored to the repo root when known, otherwise to the scanned folder.
$basePathForRelative = if ($repoRoot) { $repoRoot } else { $StepsPath }

$attributePattern = '\[(Given|When|Then|StepDefinition)\(@?"((?:[^"\\]|\\.)*)"\)'
$files = Get-ChildItem -Path $StepsPath -Recurse -Filter "*.cs"

$lines = @("bindings:")
$bindingCount = 0

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $stepMatches = [regex]::Matches($content, $attributePattern)

    if ($stepMatches.Count -eq 0) {
        continue
    }

    $relativePath = [System.IO.Path]::GetRelativePath($basePathForRelative, $file.FullName) -replace '\\', '/'
    $escapedRelativePath = $relativePath.Replace("'", "''")
    $lines += "  - file: '$escapedRelativePath'"
    $lines += "    steps:"

    foreach ($m in $stepMatches) {
        $keyword = $m.Groups[1].Value
        $pattern = $m.Groups[2].Value.Replace("'", "''")
        $line = $content.Substring(0, $m.Index).Split("`n").Count

        $lines += "      - keyword: $keyword"
        $lines += "        pattern: '$pattern'"
        $lines += "        line: $line"
        $bindingCount++
    }
}

$lines | Out-File -FilePath $OutputPath -Encoding utf8

Write-Output "Step index written to $OutputPath ($($files.Count) files scanned, $bindingCount bindings found)"
