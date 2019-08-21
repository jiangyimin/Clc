using System;
using System.Linq;
using Clc.PreRoutes;
using Clc.Affairs;

namespace Clc.EntityFrameworkCore.Seed.Tenants
{
    public class WorkEntitySeedBuilder
    {
        private readonly ClcDbContext _context;
        private readonly int _tenantId;

        public WorkEntitySeedBuilder(ClcDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreatePreRoutes();
            CreatePreRouteWorkers();
            CreatePreRouteTasks();

            CreateAffairs();
            CreateAffairWorkers();

        }

        private void CreatePreRoutes()
        {
            if (_context.PreRoutes.Count() == 0)
            {
                _context.PreRoutes.AddRange(new PreRoute[] {
                    new PreRoute { TenantId = _tenantId, DepotId = 1, RouteName = "早一号线", RouteTypeId = 1, VehicleId = 1, StartTime = "08:15", EndTime = "12:00", Mileage = (float)23.5 }, 
                    new PreRoute { TenantId = _tenantId, DepotId = 1, RouteName = "晚二号线", RouteTypeId = 1, VehicleId = 2, StartTime = "16:50", EndTime = "18:30", Mileage = (float)31.5 }, 
                }); 
                _context.SaveChanges();
            }
        }

        private void CreatePreRouteWorkers()
        {
            if (_context.PreRouteWorkers.Count() == 0)
            {
                _context.PreRouteWorkers.AddRange(new PreRouteWorker[] {
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 1, WorkRoleId = 1, WorkerId = 2 }, 
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 1, WorkRoleId = 2, WorkerId = 4 }, 
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 1, WorkRoleId = 3, WorkerId = 6 }, 
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 1, WorkRoleId = 4, WorkerId = 8 }, 
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 1, WorkRoleId = 5, WorkerId = 10 }, 

                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 2, WorkRoleId = 1, WorkerId = 3 }, 
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 2, WorkRoleId = 2, WorkerId = 5 }, 
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 2, WorkRoleId = 3, WorkerId = 7 }, 
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 2, WorkRoleId = 4, WorkerId = 1 }, 
                    new PreRouteWorker { TenantId = _tenantId, PreRouteId = 2, WorkRoleId = 5, WorkerId = 11 }, 
                }); 
                _context.SaveChanges();
            }
        }
        private void CreatePreRouteTasks()
        {
            if (_context.PreRouteTasks.Count() == 0)
            {
                _context.PreRouteTasks.AddRange(new PreRouteTask[] {
                    new PreRouteTask { TenantId = _tenantId, PreRouteId = 1, ArriveTime = "09:00", OutletId = 1, TaskTypeId = 1 },
                    new PreRouteTask { TenantId = _tenantId, PreRouteId = 1, ArriveTime = "09:20", OutletId = 3, TaskTypeId = 1 },
                    new PreRouteTask { TenantId = _tenantId, PreRouteId = 1, ArriveTime = "09:35", OutletId = 5, TaskTypeId = 1 },

                    new PreRouteTask { TenantId = _tenantId, PreRouteId = 2, ArriveTime = "17:02", OutletId = 4, TaskTypeId = 2 },
                    new PreRouteTask { TenantId = _tenantId, PreRouteId = 2, ArriveTime = "17:16", OutletId = 6, TaskTypeId = 2 },
                    new PreRouteTask { TenantId = _tenantId, PreRouteId = 2, ArriveTime = "17:33", OutletId = 7, TaskTypeId = 2 },
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateAffairs()
        {
            DateTime dd = new DateTime(2019, 8, 8);
            if (_context.Affairs.Count() == 0)
            {
                _context.Affairs.AddRange(new Affair[] {
                    new Affair { TenantId = _tenantId, DepotId = 1, CarryoutDate = dd, Status = "安排", WorkplaceId = 1, StartTime = "06:20", EndTime="12:30", CreateWorkerId = 1, CreateTime = DateTime.Now }, 
                    new Affair { TenantId = _tenantId, DepotId = 1, CarryoutDate = dd, Status = "安排", WorkplaceId = 1, StartTime = "13:30", EndTime="21:00", CreateWorkerId = 1, CreateTime = DateTime.Now }, 
                    new Affair { TenantId = _tenantId, DepotId = 1, CarryoutDate = dd, Status = "安排", WorkplaceId = 2, StartTime = "06:30", EndTime="12:30", CreateWorkerId = 1, CreateTime = DateTime.Now }, 
                    new Affair { TenantId = _tenantId, DepotId = 1, CarryoutDate = dd, Status = "安排", WorkplaceId = 2, StartTime = "15:30", EndTime="20:30", CreateWorkerId = 1, CreateTime = DateTime.Now }, 
                }); 
                _context.SaveChanges();
            }
        }

        private void CreateAffairWorkers()
        {
            if (_context.AffairWorkers.Count() == 0)
            {
                _context.AffairWorkers.AddRange(new AffairWorker[] {
                    new AffairWorker { TenantId = _tenantId, AffairId = 1, WorkRoleId = 6, WorkerId = 12 }, 
                    new AffairWorker { TenantId = _tenantId, AffairId = 1, WorkRoleId = 6, WorkerId = 13 }, 
                    new AffairWorker { TenantId = _tenantId, AffairId = 2, WorkRoleId = 6, WorkerId = 12 }, 
                    new AffairWorker { TenantId = _tenantId, AffairId = 2, WorkRoleId = 6, WorkerId = 13 }, 

                    new AffairWorker { TenantId = _tenantId, AffairId = 3, WorkRoleId = 7, WorkerId = 14}, 
                    new AffairWorker { TenantId = _tenantId, AffairId = 3, WorkRoleId = 7, WorkerId = 15 }, 
                    new AffairWorker { TenantId = _tenantId, AffairId = 4, WorkRoleId = 7, WorkerId = 14 }, 
                    new AffairWorker { TenantId = _tenantId, AffairId = 4, WorkRoleId = 7, WorkerId = 15 }, 
                }); 
                _context.SaveChanges();
            }
        }

    }
}
