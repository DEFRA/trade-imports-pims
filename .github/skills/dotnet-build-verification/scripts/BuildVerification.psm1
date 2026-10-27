#requires -Version 7.0

# Shared helpers for New-WarningBaseline.ps1 and Test-Change.ps1.

Set-StrictMode -Version Latest

# The unit test project in the UnitTest configuration compiles the shared projects through Defra.Imports.Container and
# builds both heads again. MSBuild attributes those repeated warnings to Container and the heads, so only warnings
# attributed to the test project itself (AttributedTo) are recorded for it.
$script:VerificationProjects = @(
    [pscustomobject]@{
        Path         = 'src/pluginassemblies/Defra.Imports.Plugins/Defra.Imports.Plugins.csproj'
        Arguments    = @('--configuration', 'Release')
        AttributedTo = $null
    }
    [pscustomobject]@{
        Path         = 'src/pluginassemblies/Defra.Imports.Workflows/Defra.Imports.Workflows.csproj'
        Arguments    = @('--configuration', 'Release')
        AttributedTo = $null
    }
    [pscustomobject]@{
        Path         = 'tests/Defra.Imports.UnitTests/Defra.Imports.UnitTests.csproj'
        Arguments    = @('-p:Configuration=UnitTest')
        AttributedTo = 'tests/Defra.Imports.UnitTests/Defra.Imports.UnitTests.csproj'
    }
)

function Get-RepositoryRoot {
    $root = git -C $PSScriptRoot rev-parse --show-toplevel 2>$null
    if ($LASTEXITCODE -ne 0 -or -not $root) {
        throw 'Unable to determine the repository root. Run this script from within the trade-imports-pims repository.'
    }

    return [IO.Path]::GetFullPath("$root".Trim())
}

function Get-CheckoutId {
    param(
        [Parameter(Mandatory = $true)]
        [string]$RepositoryRoot
    )

    $bytes = [Text.Encoding]::UTF8.GetBytes($RepositoryRoot.ToLowerInvariant())
    return [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).Substring(0, 8).ToLowerInvariant()
}

function Get-VerificationDirectory {
    param(
        [Parameter(Mandatory = $true)]
        [string]$RepositoryRoot,

        [string]$OutputDirectory
    )

    if (-not $OutputDirectory) {
        # One folder per clone or worktree, so that baselines from different checkouts never mix.
        $OutputDirectory = Join-Path ([IO.Path]::GetTempPath()) "trade-imports-pims-verification/$(Get-CheckoutId -RepositoryRoot $RepositoryRoot)"
    }

    $fullPath = [IO.Path]::GetFullPath($OutputDirectory).TrimEnd('\', '/')
    $rootPrefix = $RepositoryRoot.TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar
    if ($fullPath -ieq $RepositoryRoot.TrimEnd('\', '/') -or $fullPath.StartsWith($rootPrefix, [StringComparison]::OrdinalIgnoreCase)) {
        throw "The output directory must be outside the repository: $fullPath"
    }

    New-Item -ItemType Directory -Force -Path $fullPath | Out-Null
    return $fullPath
}

function Enter-VerificationLock {
    <#
    .SYNOPSIS
        Waits until no other verification run is using this checkout, then takes the lock. Concurrent builds in one
        working tree collide on the obj and bin folders, so runs queue here instead. Release it with
        Exit-VerificationLock in a finally block.
    #>
    param(
        [Parameter(Mandatory = $true)]
        [string]$RepositoryRoot,

        [int]$TimeoutMinutes = 30
    )

    $mutex = [Threading.Mutex]::new($false, "Local\trade-imports-pims-verification-$(Get-CheckoutId -RepositoryRoot $RepositoryRoot)")
    try {
        if (-not $mutex.WaitOne(0)) {
            Write-Host 'WAITING   another verification run is using this checkout'
            if (-not $mutex.WaitOne([TimeSpan]::FromMinutes($TimeoutMinutes))) {
                $mutex.Dispose()
                throw "Timed out after $TimeoutMinutes minutes waiting for another verification run in this checkout."
            }
        }
    }
    catch [Threading.AbandonedMutexException] {
        # The previous holder exited without releasing the lock, so ownership has passed to this run.
    }

    return $mutex
}

function Exit-VerificationLock {
    param(
        [Parameter(Mandatory = $true)]
        [Threading.Mutex]$Lock
    )

    $Lock.ReleaseMutex()
    $Lock.Dispose()
}

function Remove-OldVerificationRun {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Directory,

        [int]$Keep = 10
    )

    Get-ChildItem -LiteralPath $Directory -Directory |
        Where-Object Name -Match '^run-\d{8}-\d{6}-\d+$' |
        Sort-Object Name -Descending |
        Select-Object -Skip $Keep |
        Remove-Item -Recurse -Force
}

function ConvertTo-DiagnosticKey {
    <#
    .SYNOPSIS
        Normalises MSBuild diagnostics to "file|code|message": repository-relative paths with forward slashes, and no
        line numbers, columns, node prefixes or project suffixes. Unparsed lines are kept as "<unparsed>||<line>"
        unless -MatchedOnly is set.

        With -Project (a repository-relative path with forward slashes), only diagnostics that MSBuild attributes to
        that project, through its trailing "[project]" suffix, are returned. Unparsed lines cannot be attributed, so
        they are dropped.
    #>
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$RepositoryRoot,

        [Parameter(Mandatory = $true)]
        [ValidateSet('warning', 'error')]
        [string]$Severity,

        [switch]$MatchedOnly,

        [string]$Project
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        return
    }

    $pattern = '^(?<origin>.*?)(?:\((?<location>[\d,]+)\))?\s*:\s*' + $Severity + '(?:\s+(?<code>[^\s:]+))?\s*:\s*(?<message>.*?)(?:\s+\[(?<project>[^\[\]]+)\])?\s*$'
    $rootPattern = ([regex]::Escape($RepositoryRoot.TrimEnd('\', '/')) -replace '\\\\', '[\\/]') + '[\\/]'

    foreach ($rawLine in [IO.File]::ReadLines($Path)) {
        $line = ($rawLine -replace '^\s*\d+(?::\d+)?>', '') -ireplace $rootPattern, ''
        if ([string]::IsNullOrWhiteSpace($line)) {
            continue
        }

        if ($line -match $pattern) {
            if ($Project) {
                # MSBuild appends "::TargetFramework=..." to the project of a multi-targeted build.
                $attributedTo = ("$($Matches['project'])" -replace '::.*$', '') -replace '\\', '/'
                if ($attributedTo -ine $Project) {
                    continue
                }
            }

            $origin = $Matches['origin'].Trim() -replace '\\', '/'
            '{0}|{1}|{2}' -f $origin, $Matches['code'], $Matches['message'].Trim()
        }
        elseif (-not $MatchedOnly -and -not $Project) {
            '<unparsed>||{0}' -f $line.Trim()
        }
    }
}

function Invoke-VerificationBuild {
    <#
    .SYNOPSIS
        Builds the two plug-in assembly heads in Release and the unit test project in the UnitTest configuration, with
        the repository's agreed flags, and returns, per project, whether it succeeded and its normalised warnings and
        errors. Warnings are those MSBuild attributes to the project itself; errors come from every project in the
        build, because a shared-source compile error is attributed to Defra.Imports.Container.
    #>
    param(
        [Parameter(Mandatory = $true)]
        [string]$RepositoryRoot,

        [Parameter(Mandatory = $true)]
        [string]$LogDirectory
    )

    foreach ($project in $script:VerificationProjects) {
        $name = [IO.Path]::GetFileNameWithoutExtension($project.Path)
        $warningsLog = Join-Path $LogDirectory "$name.warnings.log"
        $errorsLog = Join-Path $LogDirectory "$name.errors.log"
        $buildLog = Join-Path $LogDirectory "$name.build.log"
        $projectArguments = @($project.Arguments)

        # File loggers record each diagnostic once; the console logger repeats them in its summary.
        & dotnet build (Join-Path $RepositoryRoot $project.Path) @projectArguments -v q -nologo --no-incremental '-clp:ForceNoAlign' "-flp1:logfile=$warningsLog;warningsonly;forcenoalign;nosummary" "-flp2:logfile=$errorsLog;errorsonly;forcenoalign;nosummary" *> $buildLog
        $exitCode = $LASTEXITCODE

        [pscustomobject]@{
            Name      = $name
            Path      = $project.Path
            Succeeded = $exitCode -eq 0
            Warnings  = @(ConvertTo-DiagnosticKey -Path $warningsLog -RepositoryRoot $RepositoryRoot -Severity warning -Project $project.AttributedTo)
            Errors    = @(ConvertTo-DiagnosticKey -Path $errorsLog -RepositoryRoot $RepositoryRoot -Severity error)
            Log       = $buildLog
        }
    }
}

function Get-DiagnosticCount {
    param(
        [AllowNull()]
        [AllowEmptyCollection()]
        [string[]]$Key
    )

    $counts = [Collections.Generic.Dictionary[string, int]]::new([StringComparer]::Ordinal)
    foreach ($item in $Key) {
        if ($null -eq $item) {
            continue
        }

        $existing = 0
        [void]$counts.TryGetValue($item, [ref]$existing)
        $counts[$item] = $existing + 1
    }

    # The unary comma stops the pipeline from enumerating the dictionary into key/value pairs.
    return , $counts
}

function Compare-DiagnosticCount {
    <#
    .SYNOPSIS
        Diffs two diagnostic multisets. A key whose count rises is added; a key whose count falls is removed.
    #>
    param(
        [Collections.Generic.Dictionary[string, int]]$Baseline,

        [Collections.Generic.Dictionary[string, int]]$Current
    )

    $added = @(foreach ($entry in $Current.GetEnumerator()) {
            $before = 0
            [void]$Baseline.TryGetValue($entry.Key, [ref]$before)
            if ($entry.Value -gt $before) {
                [pscustomobject]@{ Key = $entry.Key; Count = $entry.Value - $before }
            }
        })

    $removed = @(foreach ($entry in $Baseline.GetEnumerator()) {
            $after = 0
            [void]$Current.TryGetValue($entry.Key, [ref]$after)
            if ($entry.Value -gt $after) {
                [pscustomobject]@{ Key = $entry.Key; Count = $entry.Value - $after }
            }
        })

    [pscustomobject]@{
        Added   = @($added | Sort-Object Key)
        Removed = @($removed | Sort-Object Key)
    }
}

function Get-RepositoryState {
    param(
        [Parameter(Mandatory = $true)]
        [string]$RepositoryRoot
    )

    # --no-optional-locks stops git status from refreshing the index file.
    $commit = git -C $RepositoryRoot rev-parse --short HEAD 2>$null
    $changes = git -C $RepositoryRoot --no-optional-locks status --porcelain 2>$null

    [pscustomobject]@{
        Commit                = "$commit".Trim()
        HasUncommittedChanges = [bool]$changes
        Sdk                   = "$(dotnet --version)".Trim()
    }
}

Export-ModuleMember -Function Get-RepositoryRoot, Get-VerificationDirectory, Enter-VerificationLock, Exit-VerificationLock, Remove-OldVerificationRun, ConvertTo-DiagnosticKey, Invoke-VerificationBuild, Get-DiagnosticCount, Compare-DiagnosticCount, Get-RepositoryState
