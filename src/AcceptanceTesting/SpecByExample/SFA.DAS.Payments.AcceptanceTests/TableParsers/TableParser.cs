using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace SFA.DAS.Payments.AcceptanceTests.TableParsers
{
    class TableParser
    {
        /// <summary>
        /// Given a table with a type columns and then multiple period columns, 
        ///     transpose it into a list of periods that contain all the types
        ///     and their values for that period
        /// </summary>
        public static List<GenericPeriodBasedRow> Transpose(Table table)
        {
            var result = new List<GenericPeriodBasedRow>(table.Header.Count - 1);

            for (var i = 1; i < table.Header.Count; i++)
            {
                var header = table.Header.Skip(i).First();
                var resultRow = new GenericPeriodBasedRow(header);
                result.Add(resultRow);

                foreach (var row in table.Rows)
                {
                    resultRow.Rows.Add(new RowDefinition(row[0], decimal.Parse(row[header])));
                }
            }

            return result;
        }
    }

    public class GenericPeriodBasedRow
    {
        public GenericPeriodBasedRow(string period)
        {
            Period = period;
        }
        public string Period { get; }
        public List<RowDefinition> Rows { get; set; } = new List<RowDefinition>();

    }

    public class RowDefinition
    {
        public RowDefinition(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
        public string Name { get; }
        public decimal Amount { get; }
    }
}
