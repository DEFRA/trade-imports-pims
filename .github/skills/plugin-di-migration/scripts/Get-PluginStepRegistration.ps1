<#
.SYNOPSIS
    Prints the Dataverse step registrations of one or more plug-in types, read from the solution source.

.DESCRIPTION
    Reads src/solutions/defra_Imports/src/SdkMessageProcessingSteps/*.xml, the source of truth for step registrations
    ([CrmPluginRegistration] attributes in code may have drifted). For each step registered against a matching plug-in
    type, prints the message, primary entity, stage, mode, rank, filtering attributes, invocation source, images,
    description and the repository-relative XML file. Values are printed as registered, without interpretation.

    Read-only. A type with no step is reported as NOT REGISTERED.

.PARAMETER PluginTypeName
    One or more plug-in class names, short (PopulateAnimalNumber) or full
    (Defra.Imports.Plugins.ParameterSetKeyDataPair.PopulateAnimalNumber). Matching is case-insensitive. Comma-separated
    values in one string are accepted.

.EXAMPLE
    ./Get-PluginStepRegistration.ps1 -PluginTypeName 'PopulateAnimalNumber,ImporterNotificationCountManager'
#>

#requires -Version 7.0
[CmdletBinding(PositionalBinding = $false)]
param(
    [Parameter(Mandatory = $true)]
    [string[]]$PluginTypeName
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$messages = @{
    '9ebdbb1b-ea3e-db11-86a7-000a3a5473e8' = 'Create'
    '20bebb1b-ea3e-db11-86a7-000a3a5473e8' = 'Update'
    'a1bdbb1b-ea3e-db11-86a7-000a3a5473e8' = 'Delete'
    '1cbebb1b-ea3e-db11-86a7-000a3a5473e8' = 'SetState'
}
$stages = @{ '10' = 'PreValidation'; '20' = 'PreOperation'; '40' = 'PostOperation' }
$modes = @{ '0' = 'Synchronous'; '1' = 'Asynchronous' }
$imageTypes = @{ '0' = 'PreImage'; '1' = 'PostImage'; '2' = 'PreImage and PostImage' }
$invocationSources = @{ '-1' = 'Internal'; '0' = 'Parent'; '1' = 'Child' }
$deployments = @{ '0' = 'Server'; '1' = 'Outlook client'; '2' = 'Server and Outlook client' }

function Get-Label {
    param(
        [hashtable]$Map,

        [AllowEmptyString()]
        [string]$Value
    )

    $key = "$Value".Trim()
    if ($Map.ContainsKey($key)) {
        return '{0} ({1})' -f $Map[$key], $key
    }

    return "$key (unrecognised)"
}

function Get-ChildText {
    param(
        [Xml.XmlNode]$Node,

        [string]$Name
    )

    $child = $Node.SelectSingleNode($Name)
    if ($child) {
        return $child.InnerText.Trim()
    }

    return ''
}

$requested = @($PluginTypeName | ForEach-Object { "$_" -split ',' } | ForEach-Object { $_.Trim() } | Where-Object { $_ })

$root = [IO.Path]::GetFullPath("$(git -C $PSScriptRoot rev-parse --show-toplevel)".Trim())
$stepDirectory = Join-Path $root 'src/solutions/defra_Imports/src/SdkMessageProcessingSteps'

$steps = foreach ($file in Get-ChildItem -LiteralPath $stepDirectory -Filter '*.xml' -File) {
    $document = [xml]::new()
    $document.Load($file.FullName)
    $step = $document.SelectSingleNode('/SdkMessageProcessingStep')
    if (-not $step) {
        continue
    }

    $typeName = ((Get-ChildText -Node $step -Name 'PluginTypeName') -split ',')[0].Trim()
    [pscustomobject]@{
        TypeName = $typeName
        Node     = $step
        File     = [IO.Path]::GetRelativePath($root, $file.FullName) -replace '\\', '/'
    }
}

foreach ($name in $requested) {
    $matching = @($steps | Where-Object { $_.TypeName -ieq $name -or ($_.TypeName -split '\.')[-1] -ieq $name })
    if ($matching.Count -eq 0) {
        "NOT REGISTERED  $name has no step in src/solutions/defra_Imports/src/SdkMessageProcessingSteps. It may be unused, or registered by another solution."
        ''
        continue
    }

    foreach ($typeGroup in ($matching | Group-Object TypeName)) {
        'PLUGIN  {0} ({1} step(s))' -f $typeGroup.Name, $typeGroup.Count

        $ordered = $typeGroup.Group | Sort-Object { Get-Label -Map $messages -Value (Get-ChildText -Node $_.Node -Name 'SdkMessageId') }, { [int](Get-ChildText -Node $_.Node -Name 'Stage') }, File
        foreach ($entry in $ordered) {
            $node = $entry.Node
            $mode = Get-ChildText -Node $node -Name 'Mode'
            $filtering = Get-ChildText -Node $node -Name 'FilteringAttributes'

            '  STEP  {0}' -f $node.GetAttribute('Name')
            '    {0} | entity {1} | {2} | {3} | rank {4}' -f (Get-Label -Map $messages -Value (Get-ChildText -Node $node -Name 'SdkMessageId')), (Get-ChildText -Node $node -Name 'PrimaryEntity'), (Get-Label -Map $stages -Value (Get-ChildText -Node $node -Name 'Stage')), (Get-Label -Map $modes -Value $mode), (Get-ChildText -Node $node -Name 'Rank')
            '    filtering attributes: {0}' -f ($filtering ? $filtering : '(none)')
            '    invocation source: {0} | deployment: {1}{2}' -f (Get-Label -Map $invocationSources -Value (Get-ChildText -Node $node -Name 'InvocationSource')), (Get-Label -Map $deployments -Value (Get-ChildText -Node $node -Name 'SupportedDeployment')), ($mode -eq '1' ? " | async auto-delete: $(Get-ChildText -Node $node -Name 'AsyncAutoDelete')" : '')

            $images = @($node.SelectNodes('SdkMessageProcessingStepImages/SdkMessageProcessingStepImage'))
            if ($images.Count -eq 0) {
                '    images: (none)'
            }

            foreach ($image in $images) {
                $attributes = Get-ChildText -Node $image -Name 'Attributes'
                '    image "{0}": {1} on {2} | alias {3} | attributes {4}' -f $image.GetAttribute('Name'), (Get-Label -Map $imageTypes -Value (Get-ChildText -Node $image -Name 'ImageType')), (Get-ChildText -Node $image -Name 'MessagePropertyName'), (Get-ChildText -Node $image -Name 'EntityAlias'), ($attributes ? $attributes : '(all)')
            }

            $description = Get-ChildText -Node $node -Name 'Description'
            if ($description) {
                '    description: {0}' -f ($description -replace '\s+', ' ')
            }

            '    file: {0}' -f $entry.File
        }

        ''
    }
}
