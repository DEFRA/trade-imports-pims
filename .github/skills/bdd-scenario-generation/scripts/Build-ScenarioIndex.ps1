<#
.SYNOPSIS
    Builds a lightweight index of existing Gherkin scenarios (feature, scenario name, tags, source location) so
    the bdd-scenario-generation skill can check for scenarios that could be extended with new assertions instead
    of duplicating a similar scenario.

.DESCRIPTION
    Scans .feature files for `Feature:`, `Scenario:`/`Scenario Outline:`, and any `@tag` lines immediately
    preceding a scenario, and writes a YAML index summarising each scenario's name, tags, source file, and line
    number.

    The index is a disposable, point-in-time artefact reflecting the current state of the feature files. It
    must NOT be committed to source control — by default it is written to a new file in the OS temp directory
    so no .gitignore entry or repository cleanup is required. Re-run the script whenever feature files may have
    changed (e.g. at the start of a new BDD scenario authoring session).

.PARAMETER FeaturesPath
    Path to the folder containing .feature files. Defaults to the repository's Reqnroll spec project's Features
    folder (tests/Defra.Imports.Specs/Features), resolved from the current git repository root.

.PARAMETER OutputPath
    Path to write the YAML index to. Defaults to a new file in the OS temp directory.

.EXAMPLE
    ./Build-ScenarioIndex.ps1
    Builds the index using default paths and prints the output file path.

.EXAMPLE
    ./Build-ScenarioIndex.ps1 -FeaturesPath "tests/Defra.Imports.Specs/Features" -OutputPath "$env:TEMP/scenario-index.yml"
#>

#requires -Version 7.0
[CmdletBinding()]
param(
    [string]$FeaturesPath,
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

$featuresPathWasProvided = -not [string]::IsNullOrWhiteSpace($FeaturesPath)

if (-not $FeaturesPath) {
    if (-not $repoRoot) {
        throw "Unable to determine repository root. Run this script from within the trade-imports-pims repository, or pass -FeaturesPath explicitly."
    }

    $FeaturesPath = Join-Path $repoRoot "tests/Defra.Imports.Specs/Features"
}

if (-not (Test-Path -Path $FeaturesPath -PathType Container) -and $featuresPathWasProvided) {
    throw "Features path not found: $FeaturesPath"
}

if (-not $OutputPath) {
    $OutputPath = Join-Path ([System.IO.Path]::GetTempPath()) "bdd-scenario-index-$([guid]::NewGuid().ToString('N')).yml"
}

# Relative paths in the index are anchored to the repo root when known, otherwise to the scanned folder.
$basePathForRelative = if ($repoRoot) { $repoRoot } else { $FeaturesPath }

$files = @()
if (Test-Path -Path $FeaturesPath -PathType Container) {
    $files = @(Get-ChildItem -Path $FeaturesPath -Recurse -Filter "*.feature" -File)
}

$lines = @("features:")
$scenarioCount = 0

foreach ($file in $files) {
    $content = Get-Content $file.FullName
    $relativePath = [System.IO.Path]::GetRelativePath($basePathForRelative, $file.FullName) -replace '\\', '/'
    $escapedRelativePath = $relativePath.Replace("'", "''")

    $featureName = ""
    foreach ($line in $content) {
        if ($line -match '^\s*Feature:\s*(.+)$') {
            $featureName = $Matches[1].Trim()
            break
        }
    }

    $lines += "  - file: '$escapedRelativePath'"
    $lines += "    feature: '$($featureName.Replace("'", "''"))'"
    $lines += "    scenarios:"

    $pendingTags = @()
    for ($i = 0; $i -lt $content.Length; $i++) {
        $line = $content[$i].Trim()

        if ($line -match '^(@\S+\s*)+$') {
            $pendingTags += ($line -split '\s+' | Where-Object { $_ })
            continue
        }

        if ($line -match '^(Scenario|Scenario Outline):\s*(.+)$') {
            $name = $Matches[2].Trim().Replace("'", "''")
            $lines += "      - name: '$name'"
            $lines += "        line: $($i + 1)"
            if ($pendingTags.Count -gt 0) {
                $quotedTags = $pendingTags | ForEach-Object { "'$($_.Replace("'", "''"))'" }
                $lines += "        tags: [$($quotedTags -join ', ')]"
            }
            $scenarioCount++
            $pendingTags = @()
            continue
        }

        if ($line -ne "") {
            $pendingTags = @()
        }
    }
}

$lines | Out-File -FilePath $OutputPath -Encoding utf8

Write-Output "Scenario index written to $OutputPath ($($files.Count) feature files scanned, $scenarioCount scenarios found)"
