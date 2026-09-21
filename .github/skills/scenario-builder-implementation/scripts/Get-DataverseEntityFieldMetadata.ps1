<#
.SYNOPSIS
    Reports the generated C# property name/type and Entity.xml constraints (MaxLength, RequiredLevel, range,
    option set values, ValidForCreateApi) for the attributes of a Dataverse entity, so a Faker (or manual
    field-by-field review) can be written against verified values instead of assumptions.

.DESCRIPTION
    Locates the entity's Entity.xml (under src/solutions/**/Entities/*/Entity.xml, matched by schema name) and
    reads, per attribute:
      - Property: the attribute's PhysicalName - this is the exact casing Dataverse Model Builder uses for the
        generated C# property, and cannot be inferred from the LogicalName (schema name).
      - ClrType: derived from the attribute's Type (and, for picklists, its OptionSetName).
      - Type, RequiredLevel, MaxLength, MinValue, MaxValue, Precision, ValidForCreateApi.
      - OptionSet name and OptionValues (value=label pairs, read from the referenced OptionSets/*.xml), where
        applicable.

    Dataverse Model Builder casing is inconsistent per-attribute (e.g. defraimp_ArrivalDate vs
    defraimp_departuredate vs defraimp_CommodityId) and cannot be inferred from the logical (schema) name -
    always verify against this script's output before writing Faker rules or direct field assignments.

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

# Dataverse attribute Type -> generated CLR type. Picklist/state/status are resolved per-attribute below.
$clrTypeByAttributeType = @{
    lookup     = "Microsoft.Xrm.Sdk.EntityReference"
    owner      = "Microsoft.Xrm.Sdk.EntityReference"
    customer   = "Microsoft.Xrm.Sdk.EntityReference"
    partylist  = "System.Collections.Generic.IEnumerable<Microsoft.Xrm.Sdk.Entity>"
    datetime   = "System.Nullable<System.DateTime>"
    int        = "System.Nullable<System.Int32>"
    decimal    = "System.Nullable<System.Decimal>"
    money      = "Microsoft.Xrm.Sdk.Money"
    nvarchar   = "string"
    ntext      = "string"
    bit        = "System.Nullable<System.Boolean>"
    uniqueidentifier = "System.Nullable<System.Guid>"
    primarykey = "System.Nullable<System.Guid>"
}

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

function Get-ClrType {
    param($Attribute)

    $optionSetName = if ($Attribute.OptionSetName) {
        $Attribute.OptionSetName
    } elseif ($Attribute.Type -in @("picklist", "multiselectpicklist")) {
        "$EntityLogicalName" + "_" + "$($Attribute.LogicalName)"
    } else {
        $null
    }

    switch ($Attribute.Type) {
        "picklist" {
            if ($optionSetName) { return "$optionSetName?" }
            return $null
        }
        "multiselectpicklist" {
            if ($optionSetName) { return "System.Collections.Generic.IEnumerable<$optionSetName>" }
            return $null
        }
        "state" { return "$($EntityLogicalName)_statecode?" }
        "status" { return "$($EntityLogicalName)_statuscode?" }
        default { return $clrTypeByAttributeType[$Attribute.Type] }
    }
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
    } elseif ($attribute.Type -in @("picklist", "multiselectpicklist")) {
        "$EntityLogicalName" + "_" + "$($attribute.LogicalName)"
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
        Property          = $attribute.PhysicalName
        ClrType           = Get-ClrType -Attribute $attribute
        Type              = $attribute.Type
        RequiredLevel     = $attribute.RequiredLevel
        MaxLength         = $attribute.MaxLength
        MinValue          = $attribute.MinValue
        MaxValue          = $attribute.MaxValue
        Precision         = $attribute.Precision
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
