using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Clc.Authorization.Roles;
using Clc.Authorization.Users;
using Clc.MultiTenancy;
using Clc.Types.Entities;
using Clc.Fields;
using Clc.Works;

namespace Clc.EntityFrameworkCore
{
    public class ClcDbContext : AbpZeroDbContext<Tenant, Role, User, ClcDbContext>
    {
        // Types
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RouteType> RouteTypes { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<WorkRole> WorkRoles { get; set; }

        //public DbSet<VaultType> VaultTypes { get; set; }
        // public DbSet<Customer> Customers { get; set; }
        //public DbSet<Outlet> Outlets { get; set; }

        // Fields
        public DbSet<Depot> Depots { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Vault> Vaults { get; set; }

        public DbSet<Worker> Workers { get; set; }

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

            modelBuilder.Entity<Warehouse>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.Name}).IsUnique();
            });
            
            modelBuilder.Entity<Vault>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.Name}).IsUnique();
            });
            
            modelBuilder.Entity<Worker>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });

            modelBuilder.Entity<WarehouseTaskWorker>()
                .HasOne(b=> b.Worker).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
