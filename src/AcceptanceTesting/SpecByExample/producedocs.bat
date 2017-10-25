SET cfg=%1
IF [%cfg%]==[] SET cfg=Debug

packages\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe SFA.DAS.Payments.AcceptanceTests\bin\%cfg%\SFA.DAS.Payments.AcceptanceTests.dll --result=SFA.DAS.Payments.AcceptanceTests\bin\%cfg%\TestResult.xml;format=nunit2 --where "cat != not_implemented"

SET testresult=%ERRORLEVEL%

rmdir ..\docs /Q /S
mkdir ..\docs
packages\Pickles.CommandLine.2.8.2\tools\pickles.exe --feature-directory ..\features --link-results-file SFA.DAS.Payments.AcceptanceTests\bin\%cfg%\TestResult.xml --test-results-format nunit --output-directory ..\docs

IF %testresult% NEQ 0 (
	exit /b %testresult%
)