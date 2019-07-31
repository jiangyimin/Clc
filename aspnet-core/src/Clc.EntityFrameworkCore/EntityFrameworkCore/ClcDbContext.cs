using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Clc.Authorization.Roles;
using Clc.Authorization.Users;
using Clc.MultiTenancy;
using Clc.Types.Entities;
using Clc.Fields.Entities;
using Clc.Works;

namespace Clc.EntityFrameworkCore
{
    public class ClcDbContext : AbpZeroDbContext<Tenant, Role, User, ClcDbContext>
    {
        // Types
        public DbSet<AffairType> AffairTypes { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RouteType> RouteTypes { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<WorkRole> WorkRoles { get; set; }

        // Fields
        public DbSet<Depot> Depots { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Article> Articles { get; set; }

        // Customers
        // public DbSet<Customer> Customers { get; set; }
        // public DbSet<Outlet> Outlets { get; set; }

        // Works
        public DbSet<WarehouseTask> WarehouseTasks { get; set; }
        public DbSet<WarehouseTaskEvent> WarehouseEvents { get; set; }
        public DbSet<WarehouseTaskWorker> WarehouseTaskWorkers { get; set; }

        public ClcDbContext(DbContextOptions<ClcDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fields
            modelBuilder.Entity<Depot>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });

            modelBuilder.Entity<Workplace>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.Name}).IsUnique();
            });
                       
            modelBuilder.Entity<Worker>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });

            modelBuilder.Entity<Vehicle>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });
            
            modelBuilder.Entity<Article>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });
            
            modelBuilder.Entity<WarehouseTaskWorker>()
                .HasOne(b=> b.Worker).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
