param(
    [string]$websiteRoot,
    [string]$hostName,
    [string]$packagePath)

Remove-Item "$websiteRoot\Habitat.gen.zip" -ea SilentlyContinue

# Copy the XML and endpoint to the "Build" solution
echo "Copy-Item -Force package.xml `"$websiteRoot\package.gen.xml`""
Copy-Item -Force package.xml "$websiteRoot\package.gen.xml"

# Call Installer API to generate a package
echo "Invoke-WebRequest -Method POST -TimeoutSec 3600 `"http://$hostName/devops/packages/build?projectPath=/package.gen.xml&packagePath=/Habitat.gen.zip&apikey=720347853245A094B829C4853204F5A3C487502D9845F1E3C487A509182B873C`""
$R = Invoke-WebRequest -Method POST -TimeoutSec 3600 "http://$hostName/devops/packages/build?projectPath=/package.gen.xml&packagePath=/Habitat.gen.zip&apikey=720347853245A094B829C4853204F5A3C487502D9845F1E3C487A509182B873C"
$R
if (!$R) {
  throw "Package creation failed"
}

# Grab the generated package
echo "Move-Item -Force `"$websiteRoot\Habitat.gen.zip`" $packagePath"
Move-Item -Force "$websiteRoot\Habitat.gen.zip" $packagePath

# Clean up
Remove-Item "$websiteRoot\package.gen.xml" -ea SilentlyContinue
Remove-Item "$websiteRoot\Habitat.gen.zip" -ea SilentlyContinue

