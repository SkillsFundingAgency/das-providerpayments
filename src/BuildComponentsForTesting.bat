rmdir /S /Q "C:\Temp\PaymentsAt\Components"
mkdir "C:\Temp\PaymentsAt\Components"
REM call RunBuildComponents.bat "Debug"

xcopy "OpaEarning\Build\Deploy\1718" "C:\Temp\PaymentsAT\Components\EarningsCalculator-1.0.0.0\1718\" /S /Y
xcopy "SharedPipelineComponents\DataLock\DeployPeriodEnd" "C:\Temp\PaymentsAT\Components\DataLockPeriodEnd-1.0.0.0\" /S /Y
xcopy "SharedPipelineComponents\DataLock\Deploy" "C:\Temp\PaymentsAT\Components\DataLockSubmission-1.0.0.0\" /S /Y
xcopy "SharedPipelineComponents\DataLockEvents\DeployDataLock" "C:\Temp\PaymentsAT\Components\DataLockEvents-1.0.0.0\" /S /Y
xcopy "SharedPipelineComponents\IlrSubmissionEvents\DeploySubmission" "C:\Temp\PaymentsAT\Components\SubmissionEvents-1.0.0.0\" /S /Y
xcopy "Reference\Commitments\Deploy" "C:\Temp\PaymentsAT\Components\ReferenceCommitments-1.0.0.0\" /S /Y
xcopy "Reference\Accounts\Deploy" "C:\Temp\PaymentsAT\Components\ReferenceAccounts-1.0.0.0\" /S /Y
xcopy "PeriodEnd\DeployCoInvestedPayments" "C:\Temp\PaymentsAT\Components\CoInvestedPaymentsCalculator-1.0.0.0\" /S /Y
xcopy "PeriodEnd\DeployLevyPayments" "C:\Temp\PaymentsAT\Components\LevyCalculator-1.0.0.0\" /S /Y
xcopy "PeriodEnd\DeployManualAdjustments" "C:\Temp\PaymentsAT\Components\ManualAdjustmentsCalculator-1.0.0.0\" /S /Y
xcopy "PeriodEnd\DeployPaymentsDue" "C:\Temp\PaymentsAT\Components\PaymentsDue-1.0.0.0\" /S /Y
xcopy "PeriodEnd\DeployPeriodEndScripts" "C:\Temp\PaymentsAT\Components\PeriodEndScripts-1.0.0.0\" /S /Y
xcopy "PeriodEnd\DeployProviderAdjustments" "C:\Temp\PaymentsAT\Components\ProviderAdjustmentsCalculator-1.0.0.0\" /S /Y
xcopy "PeriodEnd\DeployTransfers" "C:\Temp\PaymentsAT\Components\TransfersCalculator-1.0.0.0\" /S /Y




