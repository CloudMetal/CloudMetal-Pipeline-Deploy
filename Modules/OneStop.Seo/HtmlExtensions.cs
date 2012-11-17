using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Onestop.Seo {
    public static class HtmlExtensions {
        public static MvcHtmlString SeoOverrideTextbox(this HtmlHelper htmlHelper, string name, string overriddenValue, string generatedValue, int maxLength) {
            var value = !String.IsNullOrEmpty(overriddenValue) ? overriddenValue : generatedValue;

            return htmlHelper.TextBox(name, value,
                        new {
                                @class = "text onestop-seo-rewrite-value",
                                data_generated_default = generatedValue,
                                data_max_length = maxLength.ToString(),
                                @readonly = "readonly"
                            });
        }
    }
}