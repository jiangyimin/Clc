using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Clc.EntityFrameworkCore
{
    public static class ClcDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ClcDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ClcDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
