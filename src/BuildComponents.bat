CD SharedPipelineComponents/Datalock
CALL RunBuild.bat %1 %2
CALL RunPackComponent.bat
CD ../..

CD OpaEarning/Transform
CALL RunBuild.bat
CD ..
powershell -file .\TransformForAcceptanceTesting.ps1 1718
powershell -file .\TransformForAcceptanceTesting.ps1 1617
CD Build
CALL RunBuild.bat %1 %2
CALL RunPackComponent.bat %1 1718
CALL RunPackComponent.bat %1 1617
CD ../..

CD Reference/Accounts
CALL RunBuild.bat %1
CALL PackageComponent.bat %1
CD ..
CD Commitments
CALL RunBuild.bat %1
CALL PackageComponent.bat %1
CD ../..

CD SharedPipelineComponents/DataLockEvents
CALL RunBuild.bat %1 %2
CALL RunPackComponent.bat %1 DataLock
CD ..
CD IlrSubmissionEvents
CALL RunBuild.bat %1 %2
CALL RunPackComponent.bat %1 Submission
CD ../..

CD PeriodEnd
CALL RunBuild.bat %1 %2
CALL RunPackComponent.bat %1 LevyPayments
CALL RunPackComponent.bat %1 PaymentsDue
CALL RunPackComponent.bat %1 CoInvestedPayments
CALL RunPackComponent.bat %1 ProviderAdjustments
CALL RunPackComponent.bat %1 ManualAdjustments
CD ..
