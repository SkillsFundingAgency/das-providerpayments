if exist "Deploy\component" (
  rd /s /q "Deploy\component"
)

robocopy SFA.DAS.ProviderPayments.Calculator.LevyPayments\bin\Release\ Deploy\component\ *.dll
