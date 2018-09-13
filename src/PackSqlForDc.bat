@echo off

rmdir /s /q "DcArtifacts\sql"
mkdir "DcArtifacts\sql"

rem ***************** Collection Migration\MigrationScripts **********************

for /d /r %%f in (Migration?Scripts) do (xcopy "%%f\*.sql" "DcArtifacts\sql\Collection Migration\MigrationScripts" /I /Y)

rem ***************** Process Submission\DataCopyMappings **********************

echo d | xcopy "PeriodEnd\CoInvestedPayments\DeployCoInvestedPayments\copy mappings\DasCoInvestedPaymentsCopyToDedsMapping.xml" "DcArtifacts\sql\Process Submission\DataCopyMappings\CopyCoInvestedPaymentsToDeds"
echo d | xcopy "SharedPipelineComponents\Datalock\DeployPeriodEnd\copy mappings\DasPeriodEndDataLockCopyToDedsMapping.xml" "DcArtifacts\sql\Process Submission\DataCopyMappings\CopyDataLockDataToDeds" /I /Y
echo d | xcopy "SharedPipelineComponents\DataLockEvents\DeployDataLock\copy mappings\DasDataLockEventsCopyToDedsMapping.xml" "DcArtifacts\sql\Process Submission\DataCopyMappings\CopyDataLockEventsToDeds" /I /Y
echo d | xcopy "PeriodEnd\LevyPayments\DeployLevyPayments\copy mappings\DasLevyPaymentsCopyToDedsMapping.xml" "DcArtifacts\sql\Process Submission\DataCopyMappings\CopyLevyPaymentsDataToDeds" /I /Y
echo d | xcopy "PeriodEnd\ManualAdjustments\DeployManualAdjustments\copy mappings\DasManualAdjustmentsCopyToDedsMapping.xml" "DcArtifacts\sql\Process Submission\DataCopyMappings\CopyManualAdjustmentsDataToDeds" /I /Y
echo d | xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\copy mappings\DasPaymentsDueCopyToDedsMapping.xml" "DcArtifacts\sql\Process Submission\DataCopyMappings\CopyPaymentsDueDataToDeds" /I /Y
echo d | xcopy "PeriodEnd\ProviderAdjustments\DeployProviderAdjustments\copy mappings\DasProviderAdjustmentsCopyToDedsMapping.xml" "DcArtifacts\sql\Process Submission\DataCopyMappings\CopyProviderAdjustmentsToDeds" /I /Y

rem ***************** Process Submission\Populate Reference Data **********************

mkdir "DcArtifacts\sql\Process Submission\Populate Reference Data"

for /r %%f in (*PeriodEnd*.Populate.Reference*.dml.sql) do (xcopy "%%f" "DcArtifacts\sql\Process Submission\Populate Reference Data" /I /Y)

rem ***************** Process Submission\Scripts **********************

mkdir "DcArtifacts\sql\Process Submission\Scripts\CleanUp"
mkdir "DcArtifacts\sql\Process Submission\Scripts\PaymentsDue PreRun"

for /r %%f in (*PeriodEnd*Cleanup.Deds.dml.sql) do (xcopy "%%f" "DcArtifacts\sql\Process Submission\Scripts\CleanUp" /I /Y)
for /r %%f in (*PeriodEnd.PaymentsDue.PreRun.Staging.*.sql) do (xcopy "%%f" "DcArtifacts\sql\Process Submission\Scripts\PaymentsDue PreRun" /I /Y)

rem ***************** Common\Schema **********************

echo d | xcopy "PeriodEnd\CoInvestedPayments\DeployCoInvestedPayments\sql\ddl\PeriodEnd.Transient.CoInvestedPayments.ddl.sprocs.sql" "DcArtifacts\sql\Common\Schema\CoInvestedPayments\Stored Procedures" /I /Y
echo d | xcopy "PeriodEnd\CoInvestedPayments\DeployCoInvestedPayments\sql\ddl\PeriodEnd.Transient.CoInvestedPayments.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\CoInvestedPayments\Tables" /I /Y
echo d | xcopy "PeriodEnd\CoInvestedPayments\DeployCoInvestedPayments\sql\ddl\PeriodEnd.Transient.CoInvestedPayments.ddl.views.sql" "DcArtifacts\sql\Common\Schema\CoInvestedPayments\Views" /I /Y

echo d | xcopy "PeriodEnd\CoInvestedPayments\DeployCoInvestedPayments\sql\ddl\PeriodEnd.Transient.DataLock.DDL.Procs.sql" "DcArtifacts\sql\Common\Schema\DataLock\Stored Procedures" /I /Y
echo d | xcopy "PeriodEnd\CoInvestedPayments\DeployCoInvestedPayments\sql\ddl\PeriodEnd.Transient.DataLock.DDL.Tables.sql" "DcArtifacts\sql\Common\Schema\DataLock\Tables" /I /Y
echo d | xcopy "PeriodEnd\CoInvestedPayments\DeployCoInvestedPayments\sql\ddl\PeriodEnd.Transient.DataLock.DDL.Views.sql" "DcArtifacts\sql\Common\Schema\DataLock\Views" /I /Y

echo d | xcopy "SharedPipelineComponents\DataLockEvents\DeployDataLock\sql\ddl\datalockevents.transient.ddl.procedures.sql" "DcArtifacts\sql\Common\Schema\DataLockEvents\Stored Procedures" /I /Y
echo d | xcopy "SharedPipelineComponents\DataLockEvents\DeployDataLock\sql\ddl\datalockevents.transient.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\DataLockEvents\Tables" /I /Y
echo d | xcopy "SharedPipelineComponents\DataLockEvents\DeployDataLock\sql\ddl\datalockevents.transient.ddl.views.periodend.sql" "DcArtifacts\sql\Common\Schema\DataLockEvents\Views" /I /Y

echo d | xcopy "PeriodEnd\LevyPayments\DeployLevyPayments\sql\ddl\PeriodEnd.Transient.LevyPayments.ddl.sprocs.sql" "DcArtifacts\sql\Common\Schema\LevyPaymentsCalculator\Stored Procedures" /I /Y
echo d | xcopy "PeriodEnd\LevyPayments\DeployLevyPayments\sql\ddl\PeriodEnd.Transient.LevyPayments.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\LevyPaymentsCalculator\Tables" /I /Y
echo d | xcopy "PeriodEnd\LevyPayments\DeployLevyPayments\sql\ddl\PeriodEnd.Transient.LevyPayments.ddl.views.sql" "DcArtifacts\sql\Common\Schema\LevyPaymentsCalculator\Views" /I /Y

echo d | xcopy "PeriodEnd\ManualAdjustments\DeployManualAdjustments\sql\ddl\PeriodEnd.Transient.ManualAdjustments.ddl.sprocs.sql" "DcArtifacts\sql\Common\Schema\ManualAdjustments\Stored Procedures" /I /Y
echo d | xcopy "PeriodEnd\ManualAdjustments\DeployManualAdjustments\sql\ddl\PeriodEnd.Transient.ManualAdjustments.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\ManualAdjustments\Tables" /I /Y
echo d | xcopy "PeriodEnd\ManualAdjustments\DeployManualAdjustments\sql\ddl\PeriodEnd.Transient.ManualAdjustments.ddl.views.sql" "DcArtifacts\sql\Common\Schema\ManualAdjustments\Views" /I /Y

echo d | xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.PaymentsDue.DDL.sprocs.sql" "DcArtifacts\sql\Common\Schema\PaymentsDue\Stored Procedures" /I /Y
echo d | xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.PaymentsDue.DDL.tables.sql" "DcArtifacts\sql\Common\Schema\PaymentsDue\Tables" /I /Y
echo d | xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.PaymentsDue.DDL.views.sql" "DcArtifacts\sql\Common\Schema\PaymentsDue\Views" /I /Y

echo d | xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.PaymentsDue.DDL.sprocs.sql" "DcArtifacts\sql\Common\Schema\ProviderAdjustmentsCalculator\Stored Procedures" /I /Y
echo d | xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.PaymentsDue.DDL.tables.sql" "DcArtifacts\sql\Common\Schema\ProviderAdjustmentsCalculator\Tables" /I /Y
echo d | xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.PaymentsDue.DDL.views.sql" "DcArtifacts\sql\Common\Schema\ProviderAdjustmentsCalculator\Views" /I /Y

mkdir "DcArtifacts\sql\Common\Schema\Reference\Tables"

copy "PeriodEnd\CoInvestedPayments\DeployCoInvestedPayments\sql\ddl\PeriodEnd.Transient.PaymentsHistory.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\Reference\Tables\PeriodEnd.Transient.CoInvested.PaymentsHistory.ddl.tables.sql"

xcopy "SharedPipelineComponents\Datalock\DeployPeriodEnd\sql\ddl\PeriodEnd.Transient.DataLock.Reference.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\Reference\Tables" /I /Y

copy "SharedPipelineComponents\DataLockEvents\DeployDataLock\sql\ddl\datalockevents.transient.reference.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\Reference\Tables\PeriodEnd.Transient.DataLockEvents.Reference.ddl.tables.sql"

copy "PeriodEnd\LevyPayments\DeployLevyPayments\sql\ddl\PeriodEnd.Transient.PaymentsHistory.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\Reference\Tables\PeriodEnd.Transient.Levy.PaymentsHistory.ddl.tables.sql"

xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.PaymentsDue.Reference.DDL.tables.sql" "DcArtifacts\sql\Common\Schema\Reference\Tables" /I /Y
xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.Reference.*.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\Reference\Tables" /I /Y

xcopy "PeriodEnd\ProviderAdjustments\DeployProviderAdjustments\sql\ddl\PeriodEnd.Transient.ProviderAdjustments.Reference.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\Reference\Tables" /I /Y

echo d | xcopy "PeriodEnd\PaymentsDue\DeployPaymentsDue\sql\ddl\PeriodEnd.Transient.Staging.ddl.tables.sql" "DcArtifacts\sql\Common\Schema\Staging\Tables" /I /Y


rem ***************** Common\Scripts **********************

echo d | xcopy "PeriodEnd\DeployPeriodEndScripts\sql\dml\01 PeriodEnd.Populate.Payments.Periods.dml.sql" "DcArtifacts\sql\Common\Scripts" /I /Y

rem ***************** Process Reports **********************

echo d | xcopy "SharedPipelineComponents\DataLockEvents\DeployDataLock\copy mappings\DasDataLockEventsCopyToDedsMapping.xml" "DcArtifacts\sql\Process Reports\DataCopyMappings\CopyDataLockEventsToDeds" /I /Y
echo d | xcopy "SharedPipelineComponents\DataLockEvents\DeployDataLock\sql\dml\*datalock.*.sql" "DcArtifacts\sql\Process Reports\Scripts\DataLockEvents PreRun" /I /Y


