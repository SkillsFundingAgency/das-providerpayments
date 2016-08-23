if "%1"=="" ( set "BuildConfig=Debug" ) else ( set "BuildConfig=%1" )

if exist "Deploy\component" ( rd /s /q "Deploy\component" )

if not exist "Deploy\component" ( md "Deploy\component" )

xcopy SFA.DAS.ProviderPayments.Calculator.LevyPayments\bin\%BuildConfig%\*.dll Deploy\component\

exit /b 0