<#
.SYNOPSIS
    Reports the generated C# property name and exact CLR type for one or more attributes on a Dataverse
    early-bound entity by grepping the generated entity class, instead of inferring either from Entity.xml.

.DESCRIPTION
    Dataverse Model Builder property casing is inconsistent per-attribute (e.g. defraimp_ArrivalDate vs
    defraimp_departuredate vs defraimp_CommodityId) and cannot be inferred from the attribute's logical
    (schema) name. Likewise, mapping an Entity.xml attribute Type (e.g. "picklist", "datetime", "partylist")
    to a CLR type via rules is unreliable: nullable wrapping, the `virtual` modifier, generated enum/option-set
    type names, and collection element types all depend on the actual generated code, not the attribute
    metadata. This script resolves both from the one unambiguous input - the attribute's logical name - by
    grepping the generated partial class for the entity at
    src/common/Defra.Imports.Model/Entities/<EntityLogicalName>.cs.

    For each requested attribute, it finds its
    [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("<logicalName>")] annotation and reads the property name
    and type from the declaration line immediately following it - this both confirms the match is the real
    property (not the same-named constant in the nested Fields class, which has no such preceding annotation)
    and captures the property name/type exactly as written, including System.Nullable<T> wrapping, a trailing
    `?`, and generated enum/option-set/collection type names.

.PARAMETER EntityLogicalName
    The entity's logical (schema) name, e.g. defraimp_importernotification. Used to locate
    src/common/Defra.Imports.Model/Entities/<EntityLogicalName>.cs.

.PARAMETER AttributeName
    One or more attribute logical names to look up (the LogicalName value reported by
    Get-DataverseEntityFieldMetadata.ps1), e.g. defraimp_enddate. Accepts the Web API "_field_value" lookup
    annotation form as well as the plain logical name. Omit to report every attribute found in the file.

.EXAMPLE
    ./Get-DataverseEntityPropertyClrType.ps1 -EntityLogicalName defraimp_watchlist -AttributeName defraimp_enddate, defraimp_watchtype

.EXAMPLE
    ./Get-DataverseEntityFieldMetadata.ps1 -EntityLogicalName defraimp_watchlist |
        ForEach-Object { ./Get-DataverseEntityPropertyClrType.ps1 -EntityLogicalName defraimp_watchlist -AttributeName $_.LogicalName }
#>

#requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$EntityLogicalName,

    [string[]]$AttributeName
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

if (-not $repoRoot) {
    throw "Unable to determine repository root. Run this script from within the trade-imports-pims repository."
}

$entityFileName = "$($EntityLogicalName.ToLowerInvariant()).cs"
$entityCsPath = Join-Path $repoRoot "src/common/Defra.Imports.Model/Entities/$entityFileName"
if (-not (Test-Path $entityCsPath)) {
    throw "Could not find generated entity class at $entityCsPath. Run the 'Generate model' task if the model is out of date."
}

$lines = Get-Content -Path $entityCsPath

# Lookups are exposed via the Web API as "_field_value" - map back to the base attribute name, same as
# Get-DataverseEntityFieldMetadata.ps1.
$requestedAttributeNames = $AttributeName | ForEach-Object { $_ -replace '^_(.*)_value$', '$1' }

# Matches the property declaration line, e.g.:
#   public virtual defraimp_watchtype? defraimp_WatchType
#   public System.Nullable<System.DateTime> defraimp_EndDate
# Deliberately excludes the same-named "public const string X = "...";" entry in the nested Fields class,
# which never follows an AttributeLogicalNameAttribute annotation.
$propertyPattern = '^\s*public\s+(?:virtual\s+)?(?<type>.+?)\s+(?<name>\S+)\s*$'

$results = for ($i = 0; $i -lt $lines.Count; $i++) {
    if ($lines[$i] -notmatch 'AttributeLogicalNameAttribute\("(?<logicalName>[^"]+)"\)') {
        continue
    }
    $logicalName = $Matches.logicalName

    if ($requestedAttributeNames -and $logicalName -notin $requestedAttributeNames) {
        continue
    }

    $declarationLine = $lines[$i + 1]
    if ($declarationLine -notmatch $propertyPattern) {
        continue
    }

    if ($Matches.name -eq "Id" -and $Matches.type -like "override *") {
        continue
    }

    [pscustomobject]@{
        LogicalName = $logicalName
        Property    = $Matches.name
        ClrType     = $Matches.type
    }
}

if ($requestedAttributeNames) {
    $found = @($results | ForEach-Object { $_.LogicalName })
    foreach ($requested in $requestedAttributeNames) {
        if ($requested -notin $found) {
            Write-Warning "Attribute not found on generated class for '$EntityLogicalName': $requested"
        }
    }
}

$results

