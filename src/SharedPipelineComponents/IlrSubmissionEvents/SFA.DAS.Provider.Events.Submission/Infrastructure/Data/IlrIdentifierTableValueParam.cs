using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.SqlServer.Server;
using SFA.DAS.Provider.Events.Submission.Domain;

namespace SFA.DAS.Provider.Events.Submission.Infrastructure.Data
{
    internal class IlrIdentifierTableValueParam : SqlMapper.IDynamicParameters
    {
        private readonly IEnumerable<IlrDetails> _parameters;

        private static readonly SqlMetaData _ukprnMetaData = new SqlMetaData("UKPRN", SqlDbType.BigInt);
        private static readonly SqlMetaData _learnRefNumberMetaData = new SqlMetaData("LearnRefNumber", SqlDbType.VarChar, 12);
        private static readonly SqlMetaData _priceEpisodeIdMetaData = new SqlMetaData("PriceEpisodeIdentifier", SqlDbType.VarChar, 25);

        public IlrIdentifierTableValueParam(IEnumerable<IlrDetails> parameters)
        {
            _parameters = parameters;
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            var sqlCommand = (SqlCommand) command;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            var items = new List<SqlDataRecord>();
            foreach (var param in _parameters)
            {
                var rec = new SqlDataRecord(_ukprnMetaData, _learnRefNumberMetaData, _priceEpisodeIdMetaData);
                rec.SetInt64(0, param.Ukprn);
                rec.SetString(1, param.LearnRefNumber);
                rec.SetString(2, param.PriceEpisodeIdentifier);
                items.Add(rec);
            }

            var p = sqlCommand.Parameters.Add("@ILRs", SqlDbType.Structured);
            p.Direction = ParameterDirection.Input;
            p.TypeName = "[Submissions].[IlrIdentifier]";
            p.Value = items;
        }
    }
}