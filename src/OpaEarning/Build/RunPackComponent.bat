@ECHO OFF
if "%1"=="" ( set "BuildConfig=Debug" ) else ( set "BuildConfig=%1" )
if "%2"=="" ( set "RuleBaseVersion=1617" ) else ( set "RuleBaseVersion=%2" )

ECHO Using build config: %BuildConfig%
ECHO Using rulebase version: %RuleBaseVersion%

if exist "Deploy\%RuleBaseVersion%\component" ( rd /s /q "Deploy\%RuleBaseVersion%\component" )
if exist "Deploy\%RuleBaseVersion%\test-results" ( rd /s /q "Deploy\%RuleBaseVersion%\test-results" )

if not exist "Deploy\%RuleBaseVersion%\component" ( md "Deploy\%RuleBaseVersion%\component" )
if not exist "Deploy\%RuleBaseVersion%\test-results" ( md "Deploy\%RuleBaseVersion%\test-results" )

xcopy SFA.DAS.CollectionEarnings.Calculator\bin\%BuildConfig%\*.dll Deploy\%RuleBaseVersion%\component\
xcopy SFA.DAS.CollectionEarnings.Calculator\Resources\%RuleBaseVersion%\*.* Deploy\%RuleBaseVersion%\component\Resources\

exit /b 0
@ECHO ON