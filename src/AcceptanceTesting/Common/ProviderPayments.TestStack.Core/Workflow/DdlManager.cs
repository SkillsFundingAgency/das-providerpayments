using System.Collections.Generic;
using System.Linq;

namespace ProviderPayments.TestStack.Core.Workflow
{
    public static class DdlManager
    {
        private static readonly List<string> RequiredScripts = new List<string>
        {
            "Ilr.Transient.DataLock.DDL.Views.sql",
            "PeriodEnd.Transient.DataLock.DDL.Views.sql",
            "OPA.Transient.Rulebase.ddl.views.sql",
            "PeriodEnd.Transient.PaymentsDue.DDL.views.sql",
            "PeriodEnd.Transient.LevyPayments.ddl.views.sql",
            "PeriodEnd.Transient.CoInvestedPayments.ddl.views.sql",
            "Ilr.Transient.Reference.CollectionPeriods.ddl.tables.sql",
            "PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql",
            "PeriodEnd.Transient.ProviderAdjustments.ddl.views.sql",
            "datalockevents.transient.ddl.views.periodend.sql",
            "PeriodEnd.Transient.Reference.Providers.ddl.tables.sql",
            "datalockevents.transient.ddl.views.submission.sql",
            "Ilr.Transient.DataLock.DDL.Procs.sql",
            "PeriodEnd.Transient.DataLock.DDL.Procs.sql",
            "Ilr.Transient.DataLock.DDL.Tables.sql",
            "PeriodEnd.Transient.DataLock.DDL.Tables.sql",
            "submissions.transient.ddl.views.sql",
            "PeriodEnd.Transient.Reference.Accounts.ddl.tables.sql",
            "Ilr.Transient.Reference.Accounts.ddl.tables.sql",
            "Ilr.Transient.Reference.Commitments.ddl.tables.sql",
            "PeriodEnd.Transient.Reference.Commitments.ddl.tables.sql",
            "ddl.transient.commitments.tables.sql",
            "ddl.transient.accounts.tables.sql",

            "OPA.Transient.Earnings.ddl.tables.sql",
            "OPA.Transient.Input.ddl.tables.sql",
            "OPA.Transient.Invalid.ddl.tables.sql",
            "OPA.Transient.Reference.ddl.tables.sql",
            "OPA.Transient.Rulebase.ddl.sprocs.sql",
            "OPA.Transient.Rulebase.ddl.tables.sql",
            "OPA.Transient.Rulebase.ddl.views.sql",
            "OPA.Transient.Transform.ddl.procs.sql",
            "OPA.Transient.Valid.ddl.tables.sql",
            "OPA.Transient.Valid.ddl.views.sql",
            "OPA.Transient.ValidLearnerDestinationandProgressions.ddl.views.sql"
        };

        public static List<string> GetScriptsRequiredOnEveryRun()
        {
            return RequiredScripts;
        }

        public static bool IsScriptRequiredOnEveryRun(string sqlFile)
        {
            return RequiredScripts.Any(x => sqlFile.ToLower().EndsWith(x.ToLower()));
        }

        private static List<string> _hasRanFlags = new List<string>();

        public static bool HasRan(string componentDirectory)
        {
            var hasRan = _hasRanFlags.Contains(componentDirectory);

            if(!hasRan)
                _hasRanFlags.Add(componentDirectory);

            return hasRan;
        }

        public static void ResetHasRanFlags()
        {
            _hasRanFlags = new List<string>();
        }
    }
}
