using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using HtmlTags;
using ServiceStack.Html;

namespace BlogEngine.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static HtmlTag TextBox<TModel>(
            this HtmlHelper helper,
            Expression<Func<TModel>> propertySelector)
        {
            var memberExpression = (MemberExpression)propertySelector.Body;
            var name = memberExpression.Member.Name;

            return new HtmlTag("input")
                .Id(name)
                .Attr("name", name)
                .Attr("type", "text");
        }
    }
}