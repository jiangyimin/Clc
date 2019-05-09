using Abp.AspNetCore.Mvc.ViewComponents;

namespace Clc.Web.Views
{
    public abstract class ClcViewComponent : AbpViewComponent
    {
        protected ClcViewComponent()
        {
            LocalizationSourceName = ClcConsts.LocalizationSourceName;
        }
    }
}
