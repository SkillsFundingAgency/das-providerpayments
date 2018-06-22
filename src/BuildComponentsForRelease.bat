rmdir /S /Q "..\artifacts\components"
mkdir "..\artifacts\components"

xcopy "OpaEarning\Build\Deploy\1718" "..\artifacts\components\EarningsCalculator\1718\" /S /Y
xcopy "OpaEarning\Build\Deploy\1617" "..\artifacts\components\EarningsCalculator\1617\" /S /Y
xcopy "SharedPipelineComponents\DataLock\DeployPeriodEnd" "..\artifacts\components\DataLockPeriodEnd\" /S /Y
xcopy "SharedPipelineComponents\DataLock\Deploy" "..\artifacts\components\DataLockSubmission\" /S /Y
xcopy "SharedPipelineComponents\DataLockEvents\DeployDataLock" "..\artifacts\components\DataLockEvents\" /S /Y
xcopy "SharedPipelineComponents\IlrSubmissionEvents\DeploySubmission" "..\artifacts\components\SubmissionEvents\" /S /Y
xcopy "Reference\Commitments\Deploy" "..\artifacts\components\ReferenceCommitments\" /S /Y
xcopy "Reference\Accounts\Deploy" "..\artifacts\components\ReferenceAccounts\" /S /Y
xcopy "PeriodEnd\DeployCoInvestedPayments" "..\artifacts\components\CoInvestedPaymentsCalculator\" /S /Y
xcopy "PeriodEnd\DeployLevyPayments" "..\artifacts\components\LevyCalculator\" /S /Y
xcopy "PeriodEnd\DeployManualAdjustments" "..\artifacts\components\ManualAdjustmentsCalculator\" /S /Y
xcopy "PeriodEnd\DeployPaymentsDue" "..\artifacts\components\PaymentsDue\" /S /Y
xcopy "PeriodEnd\DeployPeriodEndScripts" "..\artifacts\components\PeriodEndScripts\" /S /Y
xcopy "PeriodEnd\DeployProviderAdjustments" "..\artifacts\components\ProviderAdjustmentsCalculator\" /S /Y
xcopy "PeriodEnd\DeployTransferPayments" "..\artifacts\components\TransferPaymentsCalculator\" /S /Y




