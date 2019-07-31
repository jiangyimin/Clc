using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Clc.Authorization;

namespace Clc
{
    [DependsOn(
        typeof(ClcCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ClcApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ClcAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(ClcApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );

            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                // common
                mapper.CreateMap<string, bool>().ConvertUsing(s => s == "on" ? true : false);
                mapper.CreateMap<bool, string>().ConvertUsing(s => s ? "on" : "off");
            });

        }
    }
}
