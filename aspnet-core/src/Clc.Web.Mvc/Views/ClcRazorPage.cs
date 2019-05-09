using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace Clc.Web.Views
{
    public abstract class ClcRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ClcRazorPage()
        {
            LocalizationSourceName = ClcConsts.LocalizationSourceName;
        }
    }
}
