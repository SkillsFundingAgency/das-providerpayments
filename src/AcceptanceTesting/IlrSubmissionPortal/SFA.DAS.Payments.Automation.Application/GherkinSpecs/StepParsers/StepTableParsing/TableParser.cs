using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Gherkin.Ast;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.StepParsers.StepTableParsing
{
    internal static class TableParser
    {
        private static readonly string[] ValidDateTimeFormats = { "dd/MM/yy", "dd/MM/yyyy" };
        private static readonly string[] ValidPeriodPropertyNames = { "Period", "PeriodName" };
        private static readonly string ValidPropertyNamesForException = ValidPeriodPropertyNames.Aggregate((x, y) => $"{x} or {y}");

        public static T[] ParseValueTable<T>(string tableName, DataTable table) where T : new()
        {
            var def = ParseReturnTypeDefinition(typeof(T));

            var rows = table.Rows.ToArray();
            var headers = rows[0].Cells.Select(x => x.Value).ToArray();
            EnsureTableStructure(tableName, def, headers);

            var results = new T[rows.Length - 1];
            for (var i = 1; i < rows.Length; i++)
            {
                var row = rows[i];
                var cols = row.Cells.ToArray();
                var result = new T();

                foreach (var prop in def.Where(p => p.DefaultValue != null))
                {
                    prop.SetValue(result, prop.DefaultValue.ToString());
                }

                for (var j = 0; j < headers.Length; j++)
                {
                    var colName = headers[j];
                    var col = cols[j].Value;
                    var propDef = def.Single(p => p.Name.Equals(FixSpecPropName(colName), StringComparison.CurrentCultureIgnoreCase));
                    propDef.SetValue(result, col);
                }

                results[i - 1] = result;
            }
            return results;
        }
        public static T[] ParsePeriodTable<T>(string tableName, DataTable table) where T : new()
        {
            var def = ParseReturnTypeDefinition(typeof(T));
            var periodProperty = def.FirstOrDefault(p => ValidPeriodPropertyNames.Any(pn => pn.Equals(p.Name, StringComparison.CurrentCultureIgnoreCase)));
            if (periodProperty == null)
            {
                throw new Exception($"Type {typeof(T).Name} does not have a valid Period property. It must be of type string and named {ValidPropertyNamesForException}");
            }

            var rows = table.Rows.ToArray();
            var headers = rows[0].Cells.Select(x => x.Value).ToArray();
            EnsureTableStructure(tableName, def, rows.Select(r => r.Cells.ElementAt(0).Value).Skip(1).ToArray());

            var results = new List<T>();
            for (var j = 1; j < headers.Length; j++)
            {
                if (headers[j] == "...")
                {
                    continue;
                }

                var result = new T();
                foreach (var prop in def.Where(p => p.DefaultValue != null))
                {
                    prop.SetValue(result, prop.DefaultValue.ToString());
                }
                periodProperty.SetValue(result, headers[j]);

                for (var i = 1; i < rows.Length; i++)
                {
                    var row = rows[i];
                    var propName = row.Cells.ElementAt(0).Value;
                    var col = row.Cells.ElementAt(j).Value;
                    var propDef = def.Single(p => p.Name.Equals(FixSpecPropName(propName), StringComparison.CurrentCultureIgnoreCase));
                    propDef.SetValue(result, col);
                }

                results.Add(result);
            }

            return results.ToArray(); ;
        }

        private static PropertyDefinition[] ParseReturnTypeDefinition(Type type)
        {
            var props = type.GetProperties();
            var def = new PropertyDefinition[props.Length];
            for (var i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var defaultValue = prop.GetCustomAttribute<DefaultValueAttribute>()?.Value;

                var name = prop.Name;
                var colHeaderAttr = prop.GetCustomAttribute<ColumnHeaderAttribute>();
                if (colHeaderAttr != null)
                {
                    name = colHeaderAttr.Header;
                }

                var isRequired = !(prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                 && defaultValue == null;
                if (prop.GetCustomAttribute<OptionalAttribute>() != null)
                {
                    isRequired = false;
                }

                def[i] = new PropertyDefinition(prop)
                {
                    Name = name,
                    IsRequired = isRequired,
                    DefaultValue = defaultValue
                };
            }
            return def;
        }
        private static void EnsureTableStructure(string tableName, PropertyDefinition[] def, string[] headers)
        {
            var unknownHeaders = headers.Where(h => !def.Any(p => p.Name.Equals(FixSpecPropName(h), StringComparison.CurrentCultureIgnoreCase))).ToArray();
            if (unknownHeaders.Length > 0)
            {
                var list = unknownHeaders.Select(x => $"   {x}").Aggregate((x, y) => $"{x}\n{y}");
                throw new InvalidTableStructureException($"{tableName} table contains unexpected headers:\n{list}");
            }

            var requiredFieldsWithNoColumns = def.Where(p => p.IsRequired && !headers.Any(h => h.Replace(" ", "").Equals(p.Name, StringComparison.CurrentCultureIgnoreCase))).ToArray();
            if (requiredFieldsWithNoColumns.Length > 0)
            {
                var list = requiredFieldsWithNoColumns.Select(x => $"   {x.Name}").Aggregate((x, y) => $"{x}\n{y}");
                throw new InvalidTableStructureException($"{tableName} table is missing requried columns:\n{list}");
            }
        }
        private static string FixSpecPropName(string specPropName)
        {
            return specPropName.Replace(" ", "").Replace("-", "");
        }

        private class PropertyDefinition
        {
            private readonly PropertyInfo _prop;

            public PropertyDefinition(PropertyInfo prop)
            {
                _prop = prop;
            }

            public string Name { get; set; }
            public bool IsRequired { get; set; }
            public object DefaultValue { get; set; }

            public void SetValue(object instance, string value)
            {
                if (IsRequired && string.IsNullOrWhiteSpace(value) && DefaultValue == null)
                {
                    throw new Exception($"{Name} requires a value"); //TODO
                }

                var propertyType = _prop.PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return;
                    }
                    propertyType = propertyType.GenericTypeArguments[0];
                }

                object castValue;
                if (propertyType.IsEnum)
                {
                    castValue = ToEnumByDescription(value, propertyType);
                }
                else if (propertyType == typeof(string))
                {
                    castValue = value;
                }
                else if (propertyType == typeof(int))
                {
                    int x = 0;
                    if (!string.IsNullOrEmpty(value) && !int.TryParse(value, out x))
                    {
                        throw new Exception(); //TODO
                    }
                    castValue = x;
                }
                else if (propertyType == typeof(long))
                {
                    long x = 0;
                    if (!string.IsNullOrEmpty(value) && !long.TryParse(value, out x))
                    {
                        throw new Exception(); //TODO
                    }
                    castValue = x;
                }
                else if (propertyType == typeof(decimal))
                {
                    decimal x = 0;
                    if (!string.IsNullOrEmpty(value) && !decimal.TryParse(value, out x))
                    {
                        throw new Exception(); //TODO
                    }
                    castValue = x;
                }
                else if (propertyType == typeof(DateTime))
                {
                    DateTime x;
                    if (!DateTime.TryParseExact(value, ValidDateTimeFormats, new CultureInfo("en-GB"), DateTimeStyles.None, out x))
                    {
                        throw new Exception(); //TODO
                    }
                    castValue = x;
                }
                else
                {
                    throw new Exception($"Unknown property type {propertyType.Name}"); //TODO
                }

                _prop.SetValue(instance, castValue);
            }
            private static object ToEnumByDescription(string description, Type enumType)
            {
                if (!enumType.IsEnum)
                {
                    throw new ArgumentException("enumType must be an Enum", nameof(enumType));
                }

                foreach (Enum enumValue in Enum.GetValues(enumType))
                {
                    var enumDescription = GetEnumDescription(enumValue).Replace(" ", "");
                    if (enumDescription.Equals(description.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase))
                    {
                        return enumValue;
                    }
                }

                throw new ArgumentException($"Cannot find {enumType.Name} with description {description}");
            }
            private static string GetEnumDescription(Enum value)
            {
                var fi = value.GetType().GetField(value.ToString());

                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                return value.ToString();
            }

            public override string ToString()
            {
                return $"{Name} (required: {IsRequired})";
            }
        }
    }
}
