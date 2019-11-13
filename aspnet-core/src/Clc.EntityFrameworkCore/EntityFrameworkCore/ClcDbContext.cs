using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Clc.Authorization.Roles;
using Clc.Authorization.Users;
using Clc.MultiTenancy;
using Clc.Types;
using Clc.Fields;
using Clc.Clients;
using Clc.Affairs;
using Clc.Runtime;
using Clc.Routes;
using Clc.PreRoutes;

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

        // Fields
        public DbSet<Depot> Depots { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerFile> WorkerFiles { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTypeBind> ArticleTypeBinds { get; set; }

        // Clients
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Outlet> Outlets { get; set; }
        public DbSet<Box> Boxes { get; set; }

        // Runtimes        
        public DbSet<Signin> Signins { get; set; }
        public DbSet<ArticleRecord> ArticleRecords { get; set; }
        public DbSet<BoxRecord> BoxRecords { get; set; }
        public DbSet<Issue> Issues { get; set; }

        public DbSet<AskDoorRecord> AskDoorRecords { get; set; }
        public DbSet<EmergDoorRecord> EmergDoorRecords { get; set; }

        // Affairs
        public DbSet<Affair> Affairs { get; set; }
        public DbSet<AffairWorker> AffairWorkers { get; set; }
        public DbSet<AffairTask> AffairTasks { get; set; }
        public DbSet<AffairEvent> AffairEvents { get; set; }

        // PreRoutes
        public DbSet<PreRoute> PreRoutes { get; set; }
        public DbSet<PreRouteWorker> PreRouteWorkers { get; set; }
        public DbSet<PreRouteTask> PreRouteTasks { get; set; }
        // Routes
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteWorker> RouteWorkers { get; set; }
        public DbSet<RouteTask> RouteTasks { get; set; }
        public DbSet<RouteEvent> RouteEvents { get; set; }
        public DbSet<RouteArticle> RouteArticles { get; set; }
        public DbSet<RouteInBox> RouteInBoxes { get; set; }
        public DbSet<RouteOutBox> RouteOutBoxes { get; set; }

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

            modelBuilder.Entity<WorkerFile>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.WorkerId}).IsUnique();
            });

            modelBuilder.Entity<Vehicle>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });
            
            modelBuilder.Entity<Article>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.Cn}).IsUnique();
            });

            modelBuilder.Entity<ArticleTypeBind>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.ArticleTypeId}).IsUnique();
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

            // Works
            modelBuilder.Entity<Signin>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.CarryoutDate, e.DepotId, e.WorkerId });
            });

            modelBuilder.Entity<Affair>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.CarryoutDate, e.DepotId });
            });
            modelBuilder.Entity<PreRoute>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.RouteName}).IsUnique();
            });
            modelBuilder.Entity<Route>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.CarryoutDate, e.DepotId, e.RouteName}).IsUnique();
            });

            modelBuilder.Entity<ArticleRecord>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.LendTime });
            });

            modelBuilder.Entity<BoxRecord>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.InTime });
            });

            modelBuilder.Entity<Issue>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.DepotId, e.CreateTime });
            });

            modelBuilder.Entity<AskDoorRecord>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.AskTime });
                b.HasIndex(e => new { e.TenantId, e.WorkplaceId, e.AskTime });
            });

            modelBuilder.Entity<EmergDoorRecord>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.CreateTime });
                b.HasIndex(e => new { e.TenantId, e.WorkplaceId, e.CreateTime });
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

            // Runtimes
            modelBuilder.Entity<Signin>()
                .HasOne(b => b.Worker).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleRecord>()
                .HasOne(b => b.Article).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ArticleRecord>()
                .HasOne(b => b.RouteWorker).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BoxRecord>()
                .HasOne(b => b.Box).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Issue>()
                .HasOne(b => b.CreateWorker).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Issue>()
                .HasOne(b => b.ProcessWorker).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AskDoorRecord>()
                .HasOne(b => b.AskAffair).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AskDoorRecord>()
                .HasOne(b => b.MonitorAffair).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmergDoorRecord>()
                .HasOne(b => b.MonitorAffair).WithMany().OnDelete(DeleteBehavior.Restrict);

            // Affairs
            modelBuilder.Entity<Affair>()
                .HasOne(b => b.CreateWorker).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AffairWorker>()
                .HasOne(b => b.Worker).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AffairTask>()
                .HasOne(b => b.Workplace).WithMany().OnDelete(DeleteBehavior.Restrict);                
            modelBuilder.Entity<AffairTask>()
                .HasOne(b => b.CreateWorker).WithMany().OnDelete(DeleteBehavior.Restrict);

            // PreRoutes
            modelBuilder.Entity<PreRoute>()
                .HasOne(b => b.Vehicle).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PreRouteWorker>()
                .HasOne(b => b.Worker).WithMany().OnDelete(DeleteBehavior.Restrict);
                
             // Routes
            modelBuilder.Entity<Route>()
                .HasOne(b => b.CreateWorker).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Route>()
                .HasOne(b => b.Vehicle).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Route>()
                .HasMany(b => b.Articles).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Route>()
                .HasMany(b => b.InBoxes).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Route>()
                .HasMany(b => b.OutBoxes).WithOne().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RouteWorker>()
                .HasOne(b => b.Worker).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RouteTask>()
                .HasOne(b => b.CreateWorker).WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RouteArticle>()
                .HasOne(b => b.ArticleRecord).WithMany().OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<RouteInBox>()
            //    .HasOne(b => b.BoxRecord).WithMany().OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<RouteOutBox>()
            //    .HasOne(b => b.BoxRecord).WithMany().OnDelete(DeleteBehavior.Restrict);            
        }
    }
}
