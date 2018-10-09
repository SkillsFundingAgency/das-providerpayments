
PUSHD OpaEarning
PUSHD Transform
CALL RunBuild.bat
POPD
powershell -file TransformForAcceptanceTesting.ps1 1718
powershell -file TransformForAcceptanceTesting.ps1 1617
powershell -file TransformForAcceptanceTesting.ps1 1819
PUSHD Build
CALL RunPackComponent.bat %1 1718
CALL RunPackComponent.bat %1 1617
CALL RunPackComponent.bat %1 1819
POPD
POPD

PUSHD Reference
PUSHD Accounts
CALL PackageComponent.bat %1
POPD
PUSHD Commitments
CALL PackageComponent.bat %1
POPD
POPD

PUSHD SharedPipelineComponents
PUSHD DataLockEvents
CALL RunPackComponent.bat %1 DataLock
POPD
PUSHD IlrSubmissionEvents
CALL RunPackComponent.bat %1 Submission
POPD
PUSHD Datalock
CALL RunPackComponent.bat %1
POPD
POPD

PUSHD PeriodEnd
CALL RunPackComponent.bat %1 LevyPayments
CALL RunPackComponent.bat %1 PaymentsDue
CALL RunPackComponent.bat %1 CoInvestedPayments
CALL RunPackComponent.bat %1 ProviderAdjustments
CALL RunPackComponent.bat %1 ManualAdjustments
CALL RunPackComponent.bat %1 TransferPayments
CALL RunPackComponent.bat %1 Refunds
POPD
