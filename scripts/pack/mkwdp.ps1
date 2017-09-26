param(
    [string]$SourcePackage,
    [string]$TargetPackage)

#Convert to WDP
$SourceFullPath = Resolve-Path $SourcePackage
echo "ConvertTo-SCModuleWebDeployPackage -Force $SourceFullPath"
$WDP = ConvertTo-SCModuleWebDeployPackage -Force $SourceFullPath
$WDP

#Apply Tranformation
echo "New-SCCargoPayload -Force $PSSCriptRoot\PackageTransform"
$PackageTransform = New-SCCargoPayload -Force $PSSCriptRoot\PackageTransform
echo "New-SCCargoPayload -Force $PSScriptRoot\SiteTransform"
$SiteTransform = New-SCCargoPayload -Force $PSScriptRoot\SiteTransform

echo "Update-SCWebDeployPackage $WDP -CargoPayloadPath $PackageTransform"
Update-SCWebDeployPackage $WDP -CargoPayloadPath $PackageTransform
echo "Update-SCWebDeployPackage $WDP -EmbedCargoPayloadPath $SiteTransform"
Update-SCWebDeployPackage $WDP -EmbedCargoPayloadPath $SiteTransform

Remove-Item -Force $PackageTransform -ea SilentlyContinue
Remove-Item -Force $SiteTransform -ea SilentlyContinue

Move-Item -Force $WDP $TargetPackage

