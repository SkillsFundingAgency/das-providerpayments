using System;
using System.Collections.Generic;
using FluentAssertions;
using TechTalk.SpecFlow;
using FastMember;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities
{
    internal static class TableParser
    {
        public static List<T> Parse<T>(Table table) where T : new()
        {
            var result = new List<T>();
            var properties = typeof(T).Properties().ThatArePublicOrInternal;
            var accessor = TypeAccessor.Create(typeof(T));

            foreach (var row in table.Rows)
            {
                var record = new T();
                foreach (var property in properties)
                {
                    if (row.ContainsKey(property.Name))
                    {
                        accessor[record, property.Name] = Converted(property.PropertyType, row[property.Name]);
                    }
                }

                result.Add(record);
            }

            return result;
        }

        private static object Converted(Type type, string value)
        {
            if (value == null || value.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                if (type == typeof(int?) || type == typeof(int))
                {
                    return 0;
                }

                if (type == typeof(long?) || type == typeof(long))
                {
                    return 0L;
                }

                return null;
            }
            if (type == typeof(string))
            {
                return value;
            }
            if (type == typeof(long) || type == typeof(long?))
            {
                return long.Parse(value);
            }

            if (type == typeof(int) || type == typeof(int?))
            {
                return int.Parse(value);
            }
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return DateTime.Parse(value);
            }
            if (type == typeof(bool))
            {
                if (value.Equals("1", StringComparison.OrdinalIgnoreCase) || 
                    value.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }

            if (type.IsEnum)
            {
                return Enum.ToObject(type, int.Parse(value));
            }

            if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return decimal.Parse(value);
            }

            return value;
        }
    }
}

