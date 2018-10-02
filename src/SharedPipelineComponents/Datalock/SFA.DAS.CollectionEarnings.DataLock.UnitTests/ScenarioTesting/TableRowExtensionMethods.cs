using System;
using System.ComponentModel;
using System.Linq;
using Castle.Core.Internal;
using TechTalk.SpecFlow;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.ScenarioTesting
{
    public static class TableRowExtensionMethods
    {
        public static T CastFieldAs<T>(this TableRow row, string fieldName, T defaultValue = default(T))
        {
            var columns = row.Keys.ToList();

            var cellPosition = columns.FindIndex(x => x.ToLower() == fieldName.ToLower());
            if (cellPosition == -1)
            {
                return defaultValue;
            }

            var value = row[cellPosition];
            if (value.IsNullOrEmpty() || value == "NULL")
            {
                return default(T);
            }

            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    return (T) converter.ConvertFrom(value);
                }
            }
            catch (Exception)
            {
                throw new Exception($"Converting the value '{value}' from column '{fieldName}' as {typeof(T)} failed");
            }

            throw new Exception($"No converter found for {typeof(T)}");
        }
    }
}