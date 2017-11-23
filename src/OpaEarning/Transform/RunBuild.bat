"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion"
"tools\nuget\nuget.exe" "install" "NUnit.Console" "-OutputDirectory" "tools" "-ExcludeVersion"

tools\FAKE\tools\FAKE.exe Build.fsx publishDirectory="Publish" buildMode="Release" target=%2


xcopy .\SFA.OPA.InterfaceTransform.Console\bin\release\SFA.OPA.InterfaceTransform.Console.exe .\.. /y
