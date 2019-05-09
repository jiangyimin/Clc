using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Configuration;
using Abp.Runtime.Session;
using Clc.Configuration;
using Clc.Extensions;

namespace Clc.Web.Views.Shared.Components.TopBarTitle
{
    public class TopBarTitleViewComponent : ClcViewComponent
    {
        private const string Images_DIR = "~/images/";
        private readonly ISettingManager _settingManager;
        private readonly IAbpSession _abpSession;

        public TopBarTitleViewComponent(ISettingManager settingManager, IAbpSession abpSession)
        {
            _settingManager = settingManager;
            _abpSession = abpSession;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var name = await _settingManager.GetSettingValueAsync(AppSettingNames.VI.CompanyImageName);
            var model = new TopBarTitleViewModel
            {
                CompanyImageName = Images_DIR + name,
                CompanyName = await _settingManager.GetSettingValueAsync(AppSettingNames.VI.CompanyName),
                AppName = AppConsts.AppName,
                UserName = _abpSession.GetClaimValue("CN")
            };

            return View(model);
        }
    }
}
