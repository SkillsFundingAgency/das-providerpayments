if "%1"=="" ( set "BuildConfig=Debug" ) else ( set "BuildConfig=%1" )

if exist "Deploy\component" ( rd /s /q "Deploy\component" )
if exist "Deploy\test-results" ( rd /s /q "Deploy\test-results" )

if not exist "Deploy\component" ( md "Deploy\component" )
if not exist "Deploy\test-results" ( md "Deploy\test-results" )

xcopy SFA.DAS.CollectionEarnings.Calculator\bin\%BuildConfig%\*.dll Deploy\component\
xcopy TestResult*.xml Deploy\test-results\

exit /b 0