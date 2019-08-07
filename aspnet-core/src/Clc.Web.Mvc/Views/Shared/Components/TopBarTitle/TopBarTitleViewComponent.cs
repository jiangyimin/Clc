using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Runtime.Session;
using Clc.Configuration;
using Clc.Authorization.Users;
using Clc.Runtime.Cache;

namespace Clc.Web.Views.Shared.Components.TopBarTitle
{
    public class TopBarTitleViewComponent : ClcViewComponent
    {
        private const string Images_DIR = "~/images/";
        private readonly ISettingManager _settingManager;
        private readonly UserManager _userManager;
        private readonly IAbpSession _abpSession;

        private readonly IWorkerCache _workerCache;
        private readonly IDepotCache _depotCache;

        public TopBarTitleViewComponent(ISettingManager settingManager, UserManager userManager, IAbpSession abpSession, 
            IWorkerCache workerCache, IDepotCache depotCache)
        {
            _settingManager = settingManager;
            _userManager = userManager;
            _abpSession = abpSession;
            _workerCache = workerCache;
            _depotCache = depotCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var name = await _settingManager.GetSettingValueAsync(AppSettingNames.VI.CompanyImageName);
            var model = new TopBarTitleViewModel
            {
                CompanyImageName = Images_DIR + name,
                CompanyName = await _settingManager.GetSettingValueAsync(AppSettingNames.VI.CompanyName),
                AppName = AppConsts.AppName,
                UserName = await GetUserTitile()
            };

            return View(model);
        }

        private async Task<string> GetUserTitile() 
        {
            var user = await _userManager.GetUserByIdAsync(_abpSession.UserId??0);

            if (user.WorkerId.HasValue)
            {
                var worker = _workerCache[user.WorkerId.Value];
                var depot = _depotCache[worker.DepotId];
                var depotTitleName = await _settingManager.GetSettingValueAsync(AppSettingNames.VI.DepotTitleName);
                return  string.Format("{0} {1} ({2}{3})", worker.Cn, worker.Name, depot.Name, depotTitleName); 
            }
            else 
            {
                return user.UserName;
            }
         }
    }
}
