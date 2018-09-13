if "%1"=="" ( set "BuildConfig=Debug" ) else ( set "BuildConfig=%1" )

rmdir "DcArtifacts/bin"  /s /q
mkdir "DcArtifacts/bin" 
mkdir "DcArtifacts/bin/PeriodEnd" 
mkdir "DcArtifacts/bin/IlrSubmission" 
mkdir "DcArtifacts/bin/Accounts/Build" 
mkdir "DcArtifacts/bin/Commitments" 


xcopy "Reference\Accounts\SFA.DAS.Payments.Reference.Accounts\bin\%BuildConfig%\*.dll" "DcArtifacts/bin/Accounts/Build" /s /e /y 

..\Tools\IlMerge\IlMerge /lib:DcArtifacts\bin\Accounts\Build /wildcards /closed /out:DcArtifacts\bin\Accounts\DasReference.BinaryTask.dll /targetplatform:v4.5 *.dll




