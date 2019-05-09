using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Clc.Configuration;
using Clc.Web;

namespace Clc.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ClcDbContextFactory : IDesignTimeDbContextFactory<ClcDbContext>
    {
        public ClcDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ClcDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            ClcDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ClcConsts.ConnectionStringName));

            return new ClcDbContext(builder.Options);
        }
    }
}
