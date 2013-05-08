using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Extensions;
using Nancy.ViewEngines;
using Nancy.ViewEngines.Razor;

namespace BlogEngine.Web
{
    public abstract class RazorView<TModel>
        : NancyRazorViewBase<TModel> where TModel : class
    {
        public FormHelper<TModel> Form { get; private set; }

        public override void Initialize(RazorViewEngine engine, IRenderContext renderContext, object model)
        {
            base.Initialize(engine, renderContext, model);

            Layout = renderContext.Context.Request.IsAjaxRequest()
                         ? "Shared/_AjaxLayout.cshtml"
                         : "Shared/_Layout.cshtml";

            Form = new FormHelper<TModel>(Html);
        }
    }
}