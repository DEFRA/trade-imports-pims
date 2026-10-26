<#
.SYNOPSIS
    Builds both plug-in assembly heads in Release and the unit test project in the UnitTest configuration, and saves a
    normalised warning baseline outside the repository.

.DESCRIPTION
    Builds src/pluginassemblies/Defra.Imports.Plugins and src/pluginassemblies/Defra.Imports.Workflows with
    "--configuration Release -v q -nologo --no-incremental -clp:ForceNoAlign", then tests/Defra.Imports.UnitTests with
    the same flags and "-p:Configuration=UnitTest". It stops, without saving, if any build fails; errors from every
    project in a build are listed. Each warning is normalised to "file|code|message" (repository-relative path, no line
    or column numbers) and saved one line per occurrence, so that Test-Change.ps1 can diff the warnings as a multiset
    after a change.

    For the test project, only warnings that MSBuild attributes to the test project itself are recorded. Its build
    repeats the shared-source and head warnings under Defra.Imports.Container and the heads, and those are already
    recorded by the head builds.

    The projects built are recorded in <name>.json, and Test-Change.ps1 reports INCOMPLETE if they differ.

    Run it before changing any code. Prints counts only; the full logs stay in the output directory.

.PARAMETER Name
    The baseline name. Defaults to "baseline".

.PARAMETER OutputDirectory
    Where to save the baseline. Defaults to a per-checkout folder under the system temporary directory. Must be outside
    the repository.

.EXAMPLE
    ./New-WarningBaseline.ps1
#>

#requires -Version 7.0
[CmdletBinding(PositionalBinding = $false)]
param(
    [string]$Name = 'baseline',

    [string]$OutputDirectory
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'BuildVerification.psm1') -Force

$root = Get-RepositoryRoot
$directory = Get-VerificationDirectory -RepositoryRoot $root -OutputDirectory $OutputDirectory
$logDirectory = Join-Path $directory "$Name-logs"
New-Item -ItemType Directory -Force -Path $logDirectory | Out-Null

$lock = Enter-VerificationLock -RepositoryRoot $root
try {
    $builds = @(Invoke-VerificationBuild -RepositoryRoot $root -LogDirectory $logDirectory)
}
finally {
    Exit-VerificationLock -Lock $lock
}

foreach ($build in $builds) {
    'BUILD     {0,-24} {1,-9} {2} warnings' -f $build.Name, ($build.Succeeded ? 'succeeded' : 'FAILED'), $build.Warnings.Count
}

$failed = @($builds | Where-Object { -not $_.Succeeded })
if ($failed.Count -gt 0) {
    foreach ($build in $failed) {
        "ERRORS    $($build.Name) (first 20; full log: $($build.Log))"
        $build.Errors | Select-Object -Unique -First 20 | ForEach-Object { "  $_" }
    }

    'RESULT    FAIL - baseline not saved. Report the red build before changing any code; stale .projitems entries are a common cause, and a test compile error fails the build too.'
    exit 1
}

[string[]]$keys = @($builds | ForEach-Object { $_.Warnings })
[Array]::Sort($keys, [StringComparer]::Ordinal)
$baselinePath = Join-Path $directory "$Name.warnings.txt"
[IO.File]::WriteAllLines($baselinePath, $keys)

$state = Get-RepositoryState -RepositoryRoot $root
[ordered]@{
    name               = $Name
    createdUtc         = [DateTime]::UtcNow.ToString('o')
    commit             = $state.Commit
    uncommittedChanges = $state.HasUncommittedChanges
    sdk                = $state.Sdk
    warnings           = $keys.Count
    projects           = @($builds | ForEach-Object { $_.Path })
} | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $directory "$Name.json") -Encoding utf8

$distinct = (Get-DiagnosticCount -Key $keys).Count
'BASELINE  {0} warnings ({1} distinct) | commit {2}{3} | SDK {4}' -f $keys.Count, $distinct, $state.Commit, ($state.HasUncommittedChanges ? ' + uncommitted changes' : ''), $state.Sdk
"SAVED     $baselinePath"
