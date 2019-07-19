using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Configuration;
using Abp.Runtime.Session;
using Clc.Configuration;
using Clc.Extensions;
using Abp.UI;

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
                UserName = GetUserTitile()
            };

            return View(model);
        }

        private string GetUserTitile() 
        {
            var cn = _abpSession.GetClaimValue("CN");
            if (string.IsNullOrEmpty(cn))
                return null;
            else
                return string.Format("{0} {1} [{2}]", 
                    _abpSession.GetClaimValue("CN"), 
                    _abpSession.GetClaimValue("NAME"),
                    _abpSession.GetClaimValue("DEPOTNAME")); 
        }
    }
}
