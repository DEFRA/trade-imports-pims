<#
.SYNOPSIS
    Verifies a change: rebuilds both plug-in assembly heads, diffs their warnings against a baseline and runs the unit
    tests, then prints a compact report.

.DESCRIPTION
    1. Builds both Release heads exactly as New-WarningBaseline.ps1 does.
    2. Diffs the normalised warnings ("file|code|message") against the baseline as a multiset, so a second occurrence of
       an existing warning in the same file still counts as added.
    3. Optionally lists every current warning in files matching -PathFilter, whether new or not.
    4. Runs tests/Defra.Imports.UnitTests in the UnitTest configuration and summarises the TRX results.

    Full logs, the TRX file and the complete added and removed warning lists are written to a run folder outside the
    repository, printed on the last line. The ten most recent run folders are kept. Builds and test runs from different
    invocations in the same checkout queue rather than run concurrently.

    Exit codes: 0 PASS; 1 FAIL (a build error, added warnings, failed tests or no test results);
    2 INCOMPLETE (no baseline, or no tests matched -TestFilter).

.PARAMETER BaselineName
    The baseline created by New-WarningBaseline.ps1. Defaults to "baseline".

.PARAMETER PathFilter
    Repository-relative wildcard patterns with forward slashes, for example
    'src/common/Defra.Imports.Application/WatchLists/*'. Prefer exact folders or file names over broad fragments.

.PARAMETER TestFilter
    A dotnet test --filter expression, for example 'FullyQualifiedName~Defra.Imports.UnitTests.Plugins.Itahc'.

.PARAMETER SkipBuild
    Skips the builds and the warning diff.

.PARAMETER SkipTests
    Skips the unit tests.

.PARAMETER OutputDirectory
    The folder that holds the baseline. Defaults to the same per-checkout temporary folder as New-WarningBaseline.ps1.

.EXAMPLE
    ./Test-Change.ps1

.EXAMPLE
    ./Test-Change.ps1 -SkipTests -PathFilter 'src/common/Defra.Imports.Application/WatchLists/*'
#>

#requires -Version 7.0
[CmdletBinding(PositionalBinding = $false)]
param(
    [string]$BaselineName = 'baseline',

    [string[]]$PathFilter,

    [string]$TestFilter,

    [switch]$SkipBuild,

    [switch]$SkipTests,

    [string]$OutputDirectory
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'BuildVerification.psm1') -Force

function Read-TestResult {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    [xml]$trx = [IO.File]::ReadAllText($Path)
    $namespaces = [Xml.XmlNamespaceManager]::new($trx.NameTable)
    $namespaces.AddNamespace('t', 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010')
    $counters = $trx.SelectSingleNode('//t:ResultSummary/t:Counters', $namespaces)

    $failedTests = foreach ($result in $trx.SelectNodes('//t:Results/t:UnitTestResult[@outcome!="Passed" and @outcome!="NotExecuted"]', $namespaces)) {
        $message = $result.SelectSingleNode('t:Output/t:ErrorInfo/t:Message', $namespaces)
        [pscustomobject]@{
            Name    = $result.GetAttribute('testName')
            Message = if ($message) { ($message.InnerText.Trim() -split '\r?\n')[0] } else { '' }
        }
    }

    $total = [int]$counters.GetAttribute('total')
    $failed = 0
    foreach ($counter in 'failed', 'error', 'timeout', 'aborted') {
        $failed += [int]$counters.GetAttribute($counter)
    }

    # xUnit skips are recorded as NotExecuted results, but the notExecuted counter stays at zero.
    [pscustomobject]@{
        Total       = $total
        Passed      = [int]$counters.GetAttribute('passed')
        Failed      = $failed
        Skipped     = $total - [int]$counters.GetAttribute('executed')
        FailedTests = @($failedTests)
    }
}

function Format-CountedKey {
    param(
        [AllowEmptyCollection()]
        [object[]]$Item,

        [string]$Prefix = ''
    )

    foreach ($entry in $Item) {
        '  {0}{1} x {2}' -f $Prefix, $entry.Count, $entry.Key
    }
}

# pwsh -File passes comma-separated values as a single string.
$PathFilter = @($PathFilter | ForEach-Object { "$_" -split ',' } | ForEach-Object { $_.Trim() } | Where-Object { $_ })

$root = Get-RepositoryRoot
$directory = Get-VerificationDirectory -RepositoryRoot $root -OutputDirectory $OutputDirectory
$runDirectory = Join-Path $directory ('run-{0:yyyyMMdd-HHmmss}-{1}' -f (Get-Date), $PID)
New-Item -ItemType Directory -Force -Path $runDirectory | Out-Null
Remove-OldVerificationRun -Directory $directory

$failures = [Collections.Generic.List[string]]::new()
$incomplete = [Collections.Generic.List[string]]::new()
$skipped = [Collections.Generic.List[string]]::new()

if ($SkipBuild) {
    $skipped.Add('build')
    'BUILD     skipped'
}
else {
    $lock = Enter-VerificationLock -RepositoryRoot $root
    try {
        $builds = @(Invoke-HeadBuild -RepositoryRoot $root -LogDirectory $runDirectory)
    }
    finally {
        Exit-VerificationLock -Lock $lock
    }

    'BUILD     ' +  (($builds | ForEach-Object { '{0} {1}' -f $_.Name, ($_.Succeeded ? 'succeeded' : 'FAILED') }) -join ' | ')

    foreach ($build in @($builds | Where-Object { -not $_.Succeeded })) {
        $failures.Add("$($build.Name) build failed")
        "ERRORS    $($build.Name) (first 20; full log: $($build.Log))"
        $build.Errors | Select-Object -Unique -First 20 | ForEach-Object { "  $_" }
    }

    if ($failures.Count -eq 0) {
        [string[]]$currentKeys = @($builds | ForEach-Object { $_.Warnings })
        $current = Get-DiagnosticCount -Key $currentKeys
        $baselinePath = Join-Path $directory "$BaselineName.warnings.txt"

        if (Test-Path -LiteralPath $baselinePath) {
            [string[]]$baselineKeys = [IO.File]::ReadAllLines($baselinePath)
            $diff = Compare-DiagnosticCount -Baseline (Get-DiagnosticCount -Key $baselineKeys) -Current $current
            $addedCount = [int](($diff.Added | Measure-Object -Property Count -Sum).Sum)
            $removedCount = [int](($diff.Removed | Measure-Object -Property Count -Sum).Sum)
            'WARNINGS  baseline {0} -> current {1} | added {2} | removed {3}' -f $baselineKeys.Count, $currentKeys.Count, $addedCount, $removedCount

            if ($addedCount -gt 0) {
                $failures.Add("$addedCount added warning(s)")
                Format-CountedKey -Item $diff.Added -Prefix '+' | Set-Content -LiteralPath (Join-Path $runDirectory 'warnings-added.txt') -Encoding utf8
                Format-CountedKey -Item ($diff.Added | Select-Object -First 30) -Prefix '+'
                if ($diff.Added.Count -gt 30) {
                    "  ... $($diff.Added.Count - 30) more in warnings-added.txt"
                }
            }

            if ($removedCount -gt 0) {
                Format-CountedKey -Item $diff.Removed -Prefix '-' | Set-Content -LiteralPath (Join-Path $runDirectory 'warnings-removed.txt') -Encoding utf8
                $byFile = $diff.Removed |
                    Group-Object { ($_.Key -split '\|', 2)[0] } |
                    ForEach-Object { [pscustomobject]@{ Key = $_.Name; Count = [int](($_.Group | Measure-Object -Property Count -Sum).Sum) } } |
                    Sort-Object Count -Descending
                '  removed, by file (top 10; full list in warnings-removed.txt):'
                Format-CountedKey -Item ($byFile | Select-Object -First 10) -Prefix '-'
            }
        }
        else {
            $incomplete.Add("no baseline named '$BaselineName'")
            "WARNINGS  no baseline at $baselinePath, so no diff ($($currentKeys.Count) current warnings)"
        }

        if ($PathFilter.Count -gt 0) {
            $inScope = @($currentKeys | Where-Object {
                    $file = ($_ -split '\|', 2)[0]
                    @($PathFilter | Where-Object { $file -like $_ }).Count -gt 0
                })
            'IN SCOPE  {0} warning(s) in files matching: {1}' -f $inScope.Count, ($PathFilter -join ', ')
            $grouped = $inScope | Group-Object -CaseSensitive | ForEach-Object { [pscustomobject]@{ Key = $_.Name; Count = $_.Count } } | Sort-Object Key
            Format-CountedKey -Item ($grouped | Select-Object -First 30)
        }
    }
}

if ($SkipTests) {
    $skipped.Add('tests')
    'TESTS     skipped'
}
elseif ($failures.Count -gt 0) {
    'TESTS     not run because a build failed; fix the build first'
}
else {
    $trxName = 'unit-tests.trx'
    $trxPath = Join-Path $runDirectory $trxName
    $testLog = Join-Path $runDirectory 'unit-tests.log'
    $testArguments = @(
        'test'
        (Join-Path $root 'tests/Defra.Imports.UnitTests/Defra.Imports.UnitTests.csproj')
        '-p:Configuration=UnitTest'
        '-nologo'
        '-clp:ForceNoAlign'
        '--logger'
        "trx;LogFileName=$trxName"
        '--results-directory'
        $runDirectory
    )
    if ($TestFilter) {
        $testArguments += @('--filter', $TestFilter)
    }

    $lock = Enter-VerificationLock -RepositoryRoot $root
    try {
        & dotnet @testArguments *> $testLog
    }
    finally {
        Exit-VerificationLock -Lock $lock
    }

    if (Test-Path -LiteralPath $trxPath) {
        $tests = Read-TestResult -Path $trxPath
        'TESTS     total {0} | passed {1} | failed {2} | skipped {3}' -f $tests.Total, $tests.Passed, $tests.Failed, $tests.Skipped

        if ($tests.Failed -gt 0) {
            $failures.Add("$($tests.Failed) failed test(s)")
            foreach ($test in ($tests.FailedTests | Select-Object -First 30)) {
                "  FAILED $($test.Name): $($test.Message)"
            }
        }
        elseif ($tests.Total -eq 0) {
            $incomplete.Add(($TestFilter ? "no tests matched -TestFilter '$TestFilter'" : 'no tests ran'))
        }
    }
    elseif (Select-String -LiteralPath $testLog -SimpleMatch 'No test matches the given testcase filter' -Quiet) {
        $incomplete.Add('no tests matched -TestFilter')
        "TESTS     no tests matched the filter: $TestFilter"
    }
    else {
        $failures.Add('the unit test run produced no results')
        "TESTS     no results (full log: $testLog)"
        @(ConvertTo-DiagnosticKey -Path $testLog -RepositoryRoot $root -Severity error -MatchedOnly) |
            Select-Object -Unique -First 20 |
            ForEach-Object { "  $_" }
    }
}

$skippedNote = if ($skipped.Count -gt 0) { " ($($skipped -join ' and ') skipped)" } else { '' }
if ($SkipBuild -and $SkipTests) {
    $incomplete.Add('nothing was verified')
}

if ($failures.Count -gt 0) {
    "RESULT    FAIL$skippedNote - $($failures -join '; ')"
    $exitCode = 1
}
elseif ($incomplete.Count -gt 0) {
    "RESULT    INCOMPLETE$skippedNote - $($incomplete -join '; ')"
    $exitCode = 2
}
else {
    "RESULT    PASS$skippedNote"
    $exitCode = 0
}

"DETAILS   $runDirectory"
exit $exitCode
