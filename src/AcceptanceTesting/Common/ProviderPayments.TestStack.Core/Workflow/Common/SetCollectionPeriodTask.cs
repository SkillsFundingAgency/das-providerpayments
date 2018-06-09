using System;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
using ProviderPayments.TestStack.Core.Context;
using ProviderPayments.TestStack.Core.Domain;

namespace ProviderPayments.TestStack.Core.Workflow.Common
{
    internal class SetCollectionPeriodTask : WorkflowTask
    {
        private readonly ILogger _logger;

        public SetCollectionPeriodTask(ILogger logger)
        {
            _logger = logger;

            Id = "SetCollectionPeriod";
            Description = "Set the current collection period";
        }

        internal override void Execute(TestStackContext context)
        {
            var openPeriod = JsonConvert.DeserializeObject<CollectionPeriod>(context.CollectionPeriod);
            var firstPeriodInYear = GetFirstPeriodInYear(context.CurrentYear);

            using (var dedsConnection = GetOpenDedsConnection(context))
            {
                try
                {
                    dedsConnection.Execute("DELETE FROM [dbo].[Collection_Period_Mapping]");

                    for (var i = 0; i < 12; i++)
                    {
                        var month = firstPeriodInYear.CalendarMonth + i;
                        var year = firstPeriodInYear.CalendarYear;
                        if (month > 12)
                        {
                            month -= 12;
                            year++;
                        }
                        var periodNumber = firstPeriodInYear.PeriodId + i;
                        var open = openPeriod.CalendarMonth == month ? 1 : 0;
                        InsertPeriod(dedsConnection, context.CurrentYear, periodNumber, month, year, firstPeriodInYear.ActualsSchemaPeriod, openPeriod);
                    }

                    InsertPeriod(dedsConnection, context.CurrentYear, 13, 9, firstPeriodInYear.CalendarYear + 1, firstPeriodInYear.ActualsSchemaPeriod, openPeriod);
                    InsertPeriod(dedsConnection, context.CurrentYear, 14, 10, firstPeriodInYear.CalendarYear + 1, firstPeriodInYear.ActualsSchemaPeriod, openPeriod);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dedsConnection.Close();
                }
            }
        }

        private CollectionPeriod GetFirstPeriodInYear(string academicYear)
        {
            var year = int.Parse(academicYear.Substring(0, 2)) + 2000;
            return new CollectionPeriod
            {
                PeriodId = 1,
                Period = "R01",
                CalendarMonth = 8,
                CalendarYear = year,
                CollectionOpen = 0,
                ActualsSchemaPeriod = ""
            };
        }
        private void InsertPeriod(SqlConnection dedsConnection, string academicYear, int periodNumber, int month, int year, string actualsSchemaPeriod, CollectionPeriod openPeriod)
        {
            var open = openPeriod.PeriodId == periodNumber;
            dedsConnection.Execute(
                "INSERT INTO [dbo].[Collection_Period_Mapping] ([Collection_Year],[Period_ID], [Return_Code],[Collection_Period_Name],[Collection_ReturnCode], [Calendar_Month], [Calendar_Year], [Collection_Open], [ActualsSchemaPeriod]) " +
                " VALUES (@CollectionYear,@PeriodId, @Period,@periodName,'', @CalendarMonth, @CalendarYear, @CollectionOpen, @ActualsSchemaPeriod)",
                new
                {
                    CollectionYear = academicYear,
                    PeriodId = periodNumber,
                    Period = "R" + periodNumber.ToString("00"),
                    PeriodName = academicYear + "-R" + periodNumber.ToString("00"),
                    CalendarMonth = month,
                    CalendarYear = year,
                    CollectionOpen = open,
                    ActualsSchemaPeriod = actualsSchemaPeriod
                });
        }
    }
}