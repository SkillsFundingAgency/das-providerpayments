if "%1"=="" ( set "BuildConfig=Debug" ) else ( set "BuildConfig=%1" )

if exist "Deploy\component" ( rd /s /q "Deploy\component" )
if exist "Deploy\test-results" ( rd /s /q "Deploy\test-results" )

if not exist "Deploy\component" ( md "Deploy\component" )
if not exist "Deploy\test-results" ( md "Deploy\test-results" )

xcopy SFA.DAS.CollectionEarnings.DataLock\bin\%BuildConfig%\*.dll Deploy\component\
xcopy SFA.DAS.CollectionEarnings.DataLock\bin\%BuildConfig%\*.pdb Deploy\component\
xcopy ..\..\Shared\SQL\* DeployPeriodEnd\SQL\ /e /y
xcopy "..\..\TestResult*.SFA.DAS.CollectionEarnings.DataLock.xml" Deploy\test-results\

if exist "DeployPeriodEnd\component" ( rd /s /q "DeployPeriodEnd\component" )
if exist "DeployPeriodEnd\test-results" ( rd /s /q "DeployPeriodEnd\test-results" )

if not exist "DeployPeriodEnd\component" ( md "DeployPeriodEnd\component" )
if not exist "DeployPeriodEnd\test-results" ( md "DeployPeriodEnd\test-results" )

xcopy SFA.DAS.CollectionEarnings.DataLock\bin\%BuildConfig%\*.dll DeployPeriodEnd\component\
xcopy SFA.DAS.CollectionEarnings.DataLock\bin\%BuildConfig%\*.pdb DeployPeriodEnd\component\
xcopy ..\..\Shared\SQL\* DeployPeriodEnd\SQL\ /e /y
xcopy ..\..\TestResult*.SFA.DAS.CollectionEarnings.DataLock.xml DeployPeriodEnd\test-results\
xcopy ..\..\TestResult*.DCFS*.xml Deploy\test-results\

exit /b 0