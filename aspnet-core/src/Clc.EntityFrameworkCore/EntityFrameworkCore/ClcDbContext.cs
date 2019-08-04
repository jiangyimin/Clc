using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Clc.Authorization.Roles;
using Clc.Authorization.Users;
using Clc.MultiTenancy;
using Clc.Types.Entities;
using Clc.Fields.Entities;
using Clc.Clients.Entities;
using Clc.Works.Entities;

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

        // Clients
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<Box> Boxes { get; set; }

        // Works
        public DbSet<Signin> Signins { get; set; }
        public DbSet<Affair> Affairs { get; set; }
        public DbSet<AffairEvent> AffairEvents { get; set; }
        public DbSet<AffairWorker> AffairWorkers { get; set; }

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
            
            // Clients
            modelBuilder.Entity<Customer>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });

            modelBuilder.Entity<Outlet>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });

            modelBuilder.Entity<Box>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });

            //
            // DeleteBehavior
            //
            // Fields
            modelBuilder.Entity<Workplace>()
                .HasOne(b => b.Depot).WithMany().OnDelete(DeleteBehavior.Restrict);

            // Customers
            modelBuilder.Entity<Box>()
                .HasOne(b => b.Outlet).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Outlet>()
                .HasOne(b => b.Customer).WithMany().OnDelete(DeleteBehavior.Restrict);

            // Works
            modelBuilder.Entity<Signin>()
                .HasOne(b => b.Worker).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Affair>()
                .HasOne(b => b.CreateWorker).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AffairWorker>()
                .HasOne(b => b.Worker).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AffairTask>()
                .HasOne(b => b.Workplace).WithMany().OnDelete(DeleteBehavior.Restrict);                
            modelBuilder.Entity<AffairTask>()
                .HasOne(b => b.CreateWorker).WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
