using System.Collections.Generic;
using System.Text;
using SFA.DAS.Payments.Automation.Domain.Specifications;
using MediatR;
using SFA.DAS.Payments.Automation.Application.RefefenceData.GetNextUlnCommand;
using System;
using SFA.DAS.Payments.Automation.Application.Entities;

namespace SFA.DAS.Payments.Automation.Application.Submission
{
    public class SqlRefenceDataGenerator : ISqlRefenceDataGenerator
    {
        private const string CommitmentsTableColumns = "CommitmentId,VersionId,Uln,Ukprn,AccountId,StartDate,EndDate,AgreedCost," +
                                    "StandardCode,ProgrammeType,FrameworkCode,PathwayCode,PaymentStatus,PaymentStatusDescription," +
                                    "Priority,EffectiveFromDate,EffectiveToDate,LegalEntityName";

        private const string AccountsTableColumnNames = "AccountId,AccountHashId,AccountName,Balance,VersionId,IsLevyPayer";

        private readonly IMediator _mediator;

        public SqlRefenceDataGenerator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public  string GenerateAccountsReferenceDataScript(List<AccountEntity> accounts)
        {
            var scripts = new StringBuilder();

            foreach (var a in accounts)
            {
                scripts.Append(GetInsertSql(a));
            }

            return scripts.ToString();
        }

        public string GenerateCommitmentsReferenceDataScript(List<CommitmentEntity> commitments)
        {
            var scripts = new StringBuilder();

            foreach (var c in commitments)
            {
                scripts.Append(GetInsertSql(c));
            }

            return scripts.ToString();
        }

        private string GetInsertSql(CommitmentEntity c)
        {
            var script = new StringBuilder();
            script.Append($" Insert Into dbo.DasCommitments ({CommitmentsTableColumns}) ");
            script.Append(" Values (");
            script.Append(c.CommitmentId);
            script.Append(",");
            script.Append(c.VersionId);
            script.Append(",");
            script.Append(c.Uln);
            script.Append(",");
            script.Append(c.Ukprn);
            script.Append(",");
            script.Append(c.AccountId);
            script.Append(",");
            script.Append($"'{c.StartDate:yyyy-MM-dd HH:mm:ss}'");
            script.Append(",");
            script.Append($"'{c.EndDate:yyyy-MM-dd HH:mm:ss}'");
            script.Append(",");
            script.Append(c.AgreedPrice);
            script.Append(",");
            script.Append(c.StandardCode.HasValue ? c.StandardCode.Value.ToString() : "Null");
            script.Append(",");
            script.Append(c.ProgrammeType.HasValue ? c.ProgrammeType.Value.ToString() : "Null");
            script.Append(",");
            script.Append(c.FrameworkCode.HasValue ? c.FrameworkCode.Value.ToString() : "Null");
            script.Append(",");
            script.Append(c.PathwayCode.HasValue ? c.PathwayCode.Value.ToString() : "Null");
            script.Append(",");
            script.Append(c.Status);
            script.Append(",");
            script.Append($"'{c.Status.ToString()}'");
            script.Append(",");
            script.Append(c.Priority);
            script.Append(",");
            script.Append($"'{c.EffectiveFrom:yyyy-MM-dd HH:mm:ss}'");
            script.Append(",");
            script.Append($"'{c.EffectiveTo:yyyy-MM-dd HH:mm:ss}'");
            script.Append(",");
            script.Append($"''");
            script.Append(")");
            script.Append("\n");
            script.Append(" GO ");
            script.Append("\n");

            return script.ToString();
        }


      

        private  string GetInsertSql(AccountEntity a)
        {
            var script = new StringBuilder();
            script.Append($" Insert Into dbo.DasAccounts ({AccountsTableColumnNames}) ");
            script.Append(" Values (");
            script.Append(a.AccountId);
            script.Append(",");
            script.Append($"'{a.AccountHashId}'");
            script.Append(",");
            script.Append( $"'{a.AccountName}'");
            script.Append(",");
            script.Append(a.Balance);
            script.Append(",");
            script.Append(a.VersionId);
            script.Append(",");
            script.Append(a.IsLevyPayer? 1: 0);
            script.Append(")");
            script.Append("\n");
            script.Append(" GO ");
            script.Append("\n");

            return script.ToString();
        }


      
    }

}
