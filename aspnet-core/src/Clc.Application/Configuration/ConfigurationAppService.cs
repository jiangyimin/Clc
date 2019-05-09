using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Clc.Configuration.Dto;

namespace Clc.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ClcAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
