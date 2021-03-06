if "%1"=="" ( set "BuildConfig=Debug" ) else ( set "BuildConfig=%1" )
if "%2"=="" ( set "Component=DataLock" ) else ( set "Component=%2" )

if exist "Deploy%Component%\component" ( rd /s /q "Deploy%Component%\component" )
if exist "Deploy%Component%\test-results" ( rd /s /q "Deploy%Component%\test-results" )

if not exist "Deploy%Component%\component" ( md "Deploy%Component%\component" )
if not exist "Deploy%Component%\test-results" ( md "Deploy%Component%\test-results" )

if exist "SFA.DAS.Provider.Events.%Component%\bin\%BuildConfig%" (
  xcopy SFA.DAS.Provider.Events.%Component%\bin\%BuildConfig%\*.dll Deploy%Component%\component\
  xcopy SFA.DAS.Provider.Events.%Component%\bin\%BuildConfig%\*.pdb Deploy%Component%\component\
)

xcopy ..\..\TestResult*.Events.DataLock.xml DeployDataLock\test-results\
xcopy ..\..\TestResult*.DCFS*.xml Deploy\test-results\

exit /b 0