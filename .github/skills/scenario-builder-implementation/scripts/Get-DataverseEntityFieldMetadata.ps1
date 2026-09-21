<#
.SYNOPSIS
    Reports Entity.xml constraints (MaxLength, RequiredLevel, range, option set values, ValidForCreateApi) for
    the attributes of a Dataverse entity, so a Faker (or manual field-by-field review) can be written against
    verified values instead of assumptions.

.DESCRIPTION
    Locates the entity's Entity.xml (under src/solutions/**/Entities/*/Entity.xml, matched by schema name) and
    reads, per attribute:
      - Type, RequiredLevel, MaxLength, MinValue, MaxValue, Precision, ValidForCreateApi.
      - Format: for nvarchar distinguishes text vs textarea (multiline); for datetime distinguishes date vs
        datetime.
      - Behavior: datetime attributes only - distinguishes UserLocal/DateOnly/TimeZoneIndependent. A date-only
        behavior means a Faker rule must not generate a time-of-day component.
      - OptionSet name and OptionValues (value=label pairs, read from the referenced OptionSets/*.xml), where
        applicable.

    This script does NOT report the generated C# property name or a CLR type - Dataverse Model Builder property
    casing is inconsistent per-attribute (e.g. defraimp_ArrivalDate vs defraimp_departuredate vs
    defraimp_CommodityId) and cannot be inferred from the LogicalName (schema name), and mapping an Entity.xml
    attribute Type to a CLR type via rules is unreliable (nullable wrapping, `virtual`, generated enum/option-
    set type names, and collection element types all depend on the actual generated code). Verify both by
    grepping the generated entity class under src/common/Defra.Imports.Model/Entities/<EntityLogicalName>.cs
    for the LogicalName reported here - see Get-DataverseEntityPropertyClrType.ps1 and the skill's "Determine
    the property name and CLR type from the generated entity class" section.

    Returns objects to the pipeline; pipe to Format-Table, Export-Csv, or ConvertTo-Json as needed. Only writes
    a file if -OutputPath is supplied - if so, do not commit that file to source control.

.PARAMETER EntityLogicalName
    The entity's logical (schema) name, e.g. defraimp_importernotification.

.PARAMETER Fields
    Optional list of specific attribute logical names to report on (accepts the Web API "_field_value" lookup
    annotation form as well as the plain logical name). Omit to report on every attribute on the entity.

.PARAMETER OutputPath
    Optional path to additionally write the results to as CSV. Do not commit this file to source control.

.EXAMPLE
    ./Get-DataverseEntityFieldMetadata.ps1 -EntityLogicalName defraimp_importernotification | Format-Table -AutoSize

.EXAMPLE
    ./Get-DataverseEntityFieldMetadata.ps1 -EntityLogicalName defraimp_importernotification -Fields defraimp_name, _defraimp_countryoforiginid_value
#>

#requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$EntityLogicalName,

    [string[]]$Fields,

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

if (-not $repoRoot) {
    throw "Unable to determine repository root. Run this script from within the trade-imports-pims repository."
}

# Find the Entity.xml whose schema name matches, regardless of which solution folder it lives under.
$solutionsPath = Join-Path $repoRoot "src/solutions"
$entityXmlPath = $null
$entityXml = $null

foreach ($candidate in Get-ChildItem -Path $solutionsPath -Recurse -Filter "Entity.xml" -File) {
    [xml]$candidateXml = Get-Content $candidate.FullName -Raw
    if ($candidateXml.Entity.EntityInfo.entity.Name -ieq $EntityLogicalName) {
        $entityXmlPath = $candidate.FullName
        $entityXml = $candidateXml
        break
    }
}

if (-not $entityXmlPath) {
    throw "Could not find Entity.xml for '$EntityLogicalName' under src/solutions/**/Entities/*/Entity.xml."
}

# OptionSets sit alongside Entities under the same solution's src folder, e.g.:
#   src/solutions/<solution>/src/Entities/<EntityName>/Entity.xml
#   src/solutions/<solution>/src/OptionSets/<optionSetName>.xml
$solutionSrcRoot = Split-Path (Split-Path (Split-Path $entityXmlPath -Parent) -Parent) -Parent
$optionSetsDir = Join-Path $solutionSrcRoot "OptionSets"
$optionSetCache = @{}

function Get-OptionSetValues {
    param([string]$OptionSetName)

    if ($optionSetCache.ContainsKey($OptionSetName)) {
        return $optionSetCache[$OptionSetName]
    }

    $path = Join-Path $optionSetsDir "$OptionSetName.xml"
    if (-not (Test-Path $path)) {
        $optionSetCache[$OptionSetName] = $null
        return $null
    }

    [xml]$optionSetXml = Get-Content $path -Raw
    $values = $optionSetXml.optionset.options.option | ForEach-Object {
        $label = $_.labels.label | Where-Object { $_.languagecode -eq '1033' } | Select-Object -First 1
        "$($_.value)=$($label.description)"
    }

    $optionSetCache[$OptionSetName] = ($values -join '; ')
    return $optionSetCache[$OptionSetName]
}

$attributes = @($entityXml.Entity.EntityInfo.entity.attributes.attribute)

if ($Fields) {
    # Lookups are exposed via the Web API as "_field_value" - map back to the base attribute name.
    $requestedLogicalNames = $Fields | ForEach-Object { $_ -replace '^_(.*)_value$', '$1' }
    $attributes = $attributes | Where-Object { $_.LogicalName -in $requestedLogicalNames }

    $foundLogicalNames = @($attributes | ForEach-Object { $_.LogicalName })
    foreach ($requested in $requestedLogicalNames) {
        if ($requested -notin $foundLogicalNames) {
            Write-Warning "Field not found on entity '$EntityLogicalName': $requested"
        }
    }
}

$results = foreach ($attribute in $attributes) {
    $optionSetName = if ($attribute.OptionSetName) {
        $attribute.OptionSetName
    } elseif ($attribute.optionset.Name) {
        $attribute.optionset.Name
    } else {
        $null
    }

    $optionValues = if ($attribute.optionset.options.option) {
        @($attribute.optionset.options.option | ForEach-Object {
                $label = $_.labels.label | Where-Object { $_.languagecode -eq '1033' } | Select-Object -First 1
                "$($_.value)=$($label.description)"
            }) -join '; '
    } elseif ($attribute.OptionSetName) {
        Get-OptionSetValues -OptionSetName $attribute.OptionSetName
    } else {
        $null
    }

    [pscustomobject]@{
        LogicalName       = $attribute.LogicalName
        Type              = $attribute.Type
        RequiredLevel     = $attribute.RequiredLevel
        MaxLength         = $attribute.MaxLength
        MinValue          = $attribute.MinValue
        MaxValue          = $attribute.MaxValue
        Precision         = $attribute.Precision
        Format            = switch ($attribute.Format) {
            0 { "DateOnly" }
            1 { "DateAndTime" }
            default { $null }
        }
        ValidForCreateApi = $attribute.ValidForCreateApi
        OptionSet         = $optionSetName
        OptionValues      = $optionValues
    }
}

if ($OutputPath) {
    $results | Export-Csv -Path $OutputPath -NoTypeInformation
    Write-Information "Field metadata for '$EntityLogicalName' ($($results.Count) attributes) also written to $OutputPath"
}

$results
