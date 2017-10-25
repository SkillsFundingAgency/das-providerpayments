param (
    [string]$rulebaseYear = "1617"
 )

function GetLatestVersion
{
Param ($folders)
    [long]$maxVersion = 0
    $maxVersionName = ""


    foreach ($folder in $folders) {

        if($folder.Name.Split(".")[0] -ne $rulebaseYear)
        {
            continue
        } 

        [long]$currentVersion = [convert]::ToInt64($folder.Name.replace(".", ""), 10)
        
        if ($currentVersion -gt $maxVersion)
        {
            $maxVersion = $currentVersion
            $maxVersionName = $folder.Name
        }
    }
    
    return $maxVersionName
}

$interfaceVersions = Get-ChildItem -Directory .\Source\Interface
$rulebaseVersions = Get-ChildItem -Directory .\Source\Rulebase

$interfaceVersion = GetLatestVersion $interfaceVersions
$rulebaseVersion = GetLatestVersion $rulebaseVersions

$interfaceSourcePath = ".\Source\Interface\$($interfaceVersion)\"
$interfaceDestinationPath = ".\Deploy\$($rulebaseYear )\Interface $($interfaceVersion)\"

$rulebaseSourcePath = ".\Source\Rulebase\$($rulebaseVersion)\"
$rulebaseDestinationPath = ".\Deploy\$($rulebaseYear)\Rulebase $($rulebaseVersion)\"

# Clean previous deployments
Get-ChildItem -Path .\Deploy\$($rulebaseYear)  -Include * -Exclude Readme.txt | Remove-Item -Recurse

# Copy interface
Copy-Item -Path $interfaceSourcePath -Destination $interfaceDestinationPath -Recurse -Force

# Copy rulebase
Copy-Item -Path $rulebaseSourcePath -Destination $rulebaseDestinationPath -Recurse -Force
