using System.Collections.Generic;

namespace ProviderPayments.TestStack.Core.Workflow
{
    public static class DdlChecker
    {
        private static readonly List<string> RequiredScripts = new List<string>
        {
            "03 PeriodEnd.Populate.Reference.Commitments.dml.sql",
            "1_Ilr.Deds.DataLock.Tables.Change_Column_Types.sql",
            "1_PeriodEnd.Deds.DataLock.Tables.Change_Column_Types.sql",
            "2_Ilr.Deds.DataLock.Tables.Change_version_id_type.sql",
            "2_PeriodEnd.Deds.DataLock.Tables.Change_version_id_type.sql",
            "4_Ilr.Deds.DataLock.Tables.Add_new_column.sql",
            "4_PeriodEnd.Deds.DataLock.Tables.Add_new_column.sql",
            "Ilr.Deds.DataLock.DDL.Tables.sql",
            "Ilr.Transient.DataLock.DDL.Procs.sql",
            "Ilr.Transient.DataLock.DDL.Tables.sql",
            "Ilr.Transient.DataLock.DDL.Views.sql",
            "Ilr.Transient.Reference.Commitments.ddl.tables.sql",
            "PeriodEnd.Deds.DataLock.DDL.Tables.sql",
            "PeriodEnd.Transient.DataLock.DDL.Procs.sql",
            "PeriodEnd.Transient.DataLock.DDL.Tables.sql",
            "PeriodEnd.Transient.DataLock.DDL.Views.sql",
            "PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql",
            "datalockevents.transient.ddl.views.periodend.sql",
            "datalockevents.transient.ddl.views.submission.sql"
        };

        public static List<string> GetScriptsRequiredOnEveryRun()
        {
            return RequiredScripts;
        }
    }
}
