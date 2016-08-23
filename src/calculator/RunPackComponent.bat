if exist "Deploy\component" (
  rd /s /q "Deploy\component"
)

if not exist "Deploy\component" (
  md "Deploy\component"
)

robocopy SFA.DAS.ProviderPayments.Calculator.LevyPayments\bin\Release\ Deploy\component\ *.dll
