using System.Threading.Tasks;
using Clc.Configuration.Dto;

namespace Clc.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
