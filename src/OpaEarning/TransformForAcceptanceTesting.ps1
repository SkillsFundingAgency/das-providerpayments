param (
    [string]$rulebaseVersion = "1617"
 )

function GetLatestVersion
{
Param ($folders)
    [long]$maxVersion = 0
    $maxVersionName = ""

    foreach ($folder in $folders) {
        
        if($folder.Name.Split(".")[0] -ne $rulebaseVersion)
        {
            continue
        } 

        [long]$currentVersion = [convert]::ToInt64($folder.Name.replace(".", ""), 10)
       
        if ($currentVersion -gt $maxVersion)
        {
            $maxVersion = $currentVersion
            $maxVersionName = $folder.FullName
        }
    }
    
    return $maxVersionName
}


$interfaceVersions = Get-ChildItem -Directory .\Source\Interface
$rulebaseVersions = Get-ChildItem -Directory .\Source\Rulebase

$interfacePath = GetLatestVersion $interfaceVersions
$rulebasePath = GetLatestVersion $rulebaseVersions

#$rulebaseVersionPath = Split-Path $rulebasePath -Leaf 
#$rulebaseVersion = Split-Path $rulebasePath -Leaf 


$destinationPath = Get-ChildItem -Directory | Where-Object { $_.Name -eq "Build" } | select FullName

$consoleApp = ".\SFA.OPA.InterfaceTransform.Console.exe"
$interfaceArg = "/interface:$($interfacePath)"
$destinationArg = "/destination:$($destinationPath.FullName)"
$rulebaseVersionArg = "/rulebaseVersion:$($rulebaseVersion)"

Write-Host $interfaceArg
Write-Host $destinationArg


# Transform the interface scripts
Write-Host "Transforming the interface from $($interfacePath)"
& $consoleApp $interfaceArg $destinationArg $rulebaseVersionArg


# Copy rulebase
Write-Host "Copying the rulebase from $($rulebasePath)"
Copy-Item -Path "$($rulebasePath)\*.zip" -Destination "$($destinationPath.FullName)\SFA.DAS.CollectionEarnings.Calculator\Resources\$($rulebaseVersion)\" -Force
