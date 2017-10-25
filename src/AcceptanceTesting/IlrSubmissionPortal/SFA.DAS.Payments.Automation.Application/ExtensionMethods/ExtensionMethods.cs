using SFA.DAS.Payments.Automation.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SFA.DAS.Payments.Automation.Application
{
    public static class ExtensionMethods
    {
        public static string ToCSV<T>(this IEnumerable<T> list)
        {
            var type = typeof(T);
            var props = from property in typeof(T).GetProperties()
                        let orderAttribute = property.GetCustomAttributes(typeof(OrderAttribute), false).SingleOrDefault() as OrderAttribute
                        orderby (orderAttribute == null ? 0 : orderAttribute.Order)
                        select property;


            //Setup expression constants
            var param = Expression.Parameter(type, "x");
            var doublequote = Expression.Constant("\"");
            var doublequoteescape = Expression.Constant("\"\"");
            var comma = Expression.Constant(",");

            //Convert all properties to strings, escape and enclose in double quotes
            var propq = (from prop in props
                         let tostringcall = Expression.Call(Expression.Property(param, prop), prop.ReflectedType.GetMethod("ToString", new Type[0]))
                         select Expression.Call(typeof(string).GetMethod("Concat", new Type[] { typeof(String)}),  tostringcall)
                         ).ToArray();

            var concatLine = propq[0];
            for (int i = 1; i < propq.Length; i++)
                concatLine = Expression.Call(typeof(string).GetMethod("Concat", new Type[] { typeof(String), typeof(String), typeof(String) }), concatLine, comma, propq[i]);

            var method = Expression.Lambda<Func<T, String>>(concatLine, param).Compile();

            var header = String.Join(",", props.Select(p => p.Name).ToArray());

            return header + Environment.NewLine + String.Join(Environment.NewLine, list.Select(method).ToArray());
        }

        internal static DateTime ToPeriodDateTime(this string name)
        {
            return new DateTime(int.Parse(name.Substring(3, 2)) + 2000, int.Parse(name.Substring(0, 2)), 1);
        }
        internal static string ToPeriodName(this DateTime date)
        {
            return $"{date.Month:00}/{date.Year - 2000:00}";
        }
    }
}