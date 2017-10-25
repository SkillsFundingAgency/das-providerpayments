using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace SFA.DAS.Payments.Automation.WebUI
{
    public static class HtmlExtensions
    {
        public static HtmlString ValidationMessageFor1<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var x = ValidationExtensions.ValidationMessageFor<TModel, TProperty>(htmlHelper, expression, (string)null, (IDictionary<string, object>)new RouteValueDictionary());
            var validationMessageHtml = x.ToHtmlString();

            var endOfValidationMessageSpanOpening = validationMessageHtml.IndexOf(">") + 1;
            var validationMessageWrapper = validationMessageHtml.Substring(0, endOfValidationMessageSpanOpening);
            var messageContent = validationMessageHtml.Substring(0, validationMessageHtml.Length - 7).Substring(endOfValidationMessageSpanOpening);

            if (string.IsNullOrWhiteSpace(messageContent))
            {
                return new HtmlString(validationMessageHtml);
            }

            return new HtmlString(validationMessageWrapper + "<ol><li>" + messageContent.Replace("\n", "</li><li>") + "</li></ol></span>");
        }
    }
}