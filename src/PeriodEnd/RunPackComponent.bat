if "%1"=="" ( set "BuildConfig=Debug" ) else ( set "BuildConfig=%1" )
if "%2"=="" ( set "Component=LevyPayments" ) else ( set "Component=%2" )

if exist "Deploy%Component%\component" ( rd /s /q "Deploy%Component%\component" )
if exist "Deploy%Component%\test-results" ( rd /s /q "Deploy%Component%\test-results" )

if exist "%Component%\Deploy%Component%" (
  xcopy "%Component%\Deploy%Component%\*.*" "Deploy%Component%\" /s /e /y
)
if not exist "Deploy%Component%\component" ( md "Deploy%Component%\component" )
if not exist "Deploy%Component%\test-results" ( md "Deploy%Component%\test-results" )

if exist "%Component%\SFA.DAS.ProviderPayments.Calc.%Component%\bin\%BuildConfig%" (
  xcopy %Component%\SFA.DAS.ProviderPayments.Calc.%Component%\bin\%BuildConfig%\*.dll Deploy%Component%\component\
)

if exist "%Component%\SFA.DAS.Payments.Calc.%Component%\bin\%BuildConfig%" (
  xcopy %Component%\SFA.DAS.Payments.Calc.%Component%\bin\%BuildConfig%\*.dll Deploy%Component%\component\
)

xcopy ..\TestResult*.%Component%.xml Deploy%Component%\test-results\
xcopy ..\TestResult*.DCFS*.xml Deploy%Component%\test-results\

exit /b 0