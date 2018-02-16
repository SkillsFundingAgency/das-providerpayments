if "%1"=="" ( set "BuildConfig=Debug" ) else ( set "BuildConfig=%1" )

if exist DcArtifacts RmDir DcArtifacts /s /q
mkdir "DcArtifacts" 
mkdir "DcArtifacts/PeriodEnd" 
mkdir "DcArtifacts/IlrSubmission" 
mkdir "DcArtifacts/Accounts/Build" 
mkdir "DcArtifacts/Commitments" 


xcopy "Reference\Accounts\SFA.DAS.Payments.Reference.Accounts\bin\%BuildConfig%\*.dll" "DcArtifacts/Accounts/Build" /s /e /y 

..\Tools\IlMerge\IlMerge /lib:DcArtifacts\Accounts\Build /wildcards /closed /out:DcArtifacts\Accounts\DasReference.BinaryTask.dll /targetplatform:v4 *.dll




