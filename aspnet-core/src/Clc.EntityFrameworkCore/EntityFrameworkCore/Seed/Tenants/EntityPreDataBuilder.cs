using System.Linq;
using Clc.Types.Entities;
using Clc.Fields.Entities;
using Clc.Clients.Entities;

namespace Clc.EntityFrameworkCore.Seed.Tenants
{
    public class EntityPreDataBuilder
    {
        private readonly ClcDbContext _context;
        private readonly int _tenantId;

        public EntityPreDataBuilder(ClcDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            // Types
            CreateAffairTypes();
            CreateArticleTypes();
            CreatePosts();
            CreateRouteTypes();
            CreateTaskTypes();
            CreateWorkRoles();

            // Fields
            CreateDepots();
            CreateWorkplaces();
            CreateWorkers();
            CreateVehicles();
            CreateArticles();

            // Clients
            CreateCustomers();
            CreateOutlets();
            CreateBoxes();
        }

        private void CreateAffairTypes()
        {
            if (_context.AffairTypes.Count() == 0)
            {
                _context.AffairTypes.AddRange(new AffairType[] {
                    new AffairType { TenantId = _tenantId, Cn = "A", Name = "守库", hasCloudDoor = true, MinDuration = 4, MaxDuration = 12 }, 
                    new AffairType { TenantId = _tenantId, Cn = "B", Name = "出入库", hasCloudDoor = true, MinDuration = 4, MaxDuration = 12  }, 
                    new AffairType { TenantId = _tenantId, Cn = "C", Name = "值班", MinDuration = 4, MaxDuration = 12 },
                    new AffairType { TenantId = _tenantId, Cn = "D", Name = "监控", MinDuration = 4, MaxDuration = 12 },
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateArticleTypes()
        {
            if (_context.ArticleTypes.Count() == 0)
            {
                _context.ArticleTypes.AddRange(new ArticleType[] {
                    new ArticleType { TenantId = _tenantId, Cn = "A", Name = "枪", BindStyle = "车" }, 
                    new ArticleType { TenantId = _tenantId, Cn = "B", Name = "弹"}, 
                    new ArticleType { TenantId = _tenantId, Cn = "C", Name = "车钥匙", BindStyle = "车" },                 
                    new ArticleType { TenantId = _tenantId, Cn = "D", Name = "持枪证", BindStyle = "人" },
                }); 
                _context.SaveChanges();
            }
        }
        private void CreatePosts()
        {
            if (_context.Posts.Count() == 0)
            {
                _context.Posts.AddRange(new Post[] {
                    new Post { TenantId = _tenantId, Cn = "01", Name = "大队长" }, 
                    new Post { TenantId = _tenantId, Cn = "02", Name = "司机" },
                    new Post { TenantId = _tenantId, Cn = "03", Name = "车长" },
                    new Post { TenantId = _tenantId, Cn = "04", Name = "持枪员" },
                    new Post { TenantId = _tenantId, Cn = "05", Name = "主业务员" },
                    new Post { TenantId = _tenantId, Cn = "06", Name = "业务员" },
                    new Post { TenantId = _tenantId, Cn = "07", Name = "库管" },                     
                    new Post { TenantId = _tenantId, Cn = "08", Name = "监控员" },
                    new Post { TenantId = _tenantId, Cn = "11", Name = "领导" }, 
                    new Post { TenantId = _tenantId, Cn = "12", Name = "干部" }, 
                    new Post { TenantId = _tenantId, Cn = "13", Name = "职员" }
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateRouteTypes()
        {
            if (_context.RouteTypes.Count() == 0)
            {
                _context.RouteTypes.AddRange(new RouteType[] {
                    new RouteType { TenantId = _tenantId, Name = "押运", WorkRoles = "司机|车长|持枪员|主业务员|业务员", LendArticleLead = 60, LendArticleDeadline = 30 }
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateTaskTypes()
        {
            if (_context.TaskTypes.Count() == 0)
            {
                _context.TaskTypes.AddRange(new TaskType[] {
                    new TaskType { TenantId = _tenantId, Cn = "01", Name = "早送" }, 
                    new TaskType { TenantId = _tenantId, Cn = "02", Name = "晚接" }, 
                    new TaskType { TenantId = _tenantId, Cn = "03", Name = "中调" }, 
                    new TaskType { TenantId = _tenantId, Cn = "04", Name = "临时中调", isTemporary = true, BasicPrice = 180 }
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateWorkRoles()
        {
            if (_context.WorkRoles.Count() == 0)
            {
                _context.WorkRoles.AddRange(new WorkRole[] {
                    new WorkRole { TenantId = _tenantId, Cn = "01", Name = "司机", DefaultPostId = 2, mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "02", Name = "车长", DefaultPostId = 3, mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "03", Name = "持枪员", DefaultPostId = 4, mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "04", Name = "主业务员", DefaultPostId = 5, mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "05", Name = "业务员", DefaultPostId = 6, mustHave = true, MaxNum = 2 },
                    new WorkRole { TenantId = _tenantId, Cn = "06", Name = "库房", DefaultPostId = 7, mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "07", Name = "金库", DefaultPostId = 7, mustHave = true, MaxNum = 8 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "08", Name = "值班", mustHave = true, MaxNum = 5 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "09", Name = "监控", DefaultPostId = 8, mustHave = true, MaxNum = 3 }
                 }); 
                _context.SaveChanges();
            }
        }

        private void CreateDepots()
        {
            if (_context.Depots.Count() == 0)
            {
                _context.Depots.AddRange(new Depot[] {
                    new Depot { TenantId = _tenantId, Cn = "01", Name = "中心", Longitude = 114.016667, Latitude = 22.534167 }, 
                    new Depot { TenantId = _tenantId, Cn = "02", Name = "调度", Longitude = 114.016667, Latitude = 22.534167 }, 
                    new Depot { TenantId = _tenantId, Cn = "90", Name = "总部", Longitude = 114.016667, Latitude = 22.534167 }, 
                    new Depot { TenantId = _tenantId, Cn = "91", Name = "监控", Longitude = 114.016667, Latitude = 22.534167 }, 
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateWorkplaces()
        {
            if (_context.Workplaces.Count() == 0)
            {
                _context.Workplaces.AddRange(new Workplace[] {
                    new Workplace { TenantId = _tenantId, DepotId = 1, Name = "库房", AffairTypeId = 1, ArticleTypeList = "A|B|C|D", RoleUserName = "PlaceA" },
                    new Workplace { TenantId = _tenantId, DepotId = 1, Name = "金库", AffairTypeId = 2, RoleUserName = "PlaceB" }, 
                    new Workplace { TenantId = _tenantId, DepotId = 1, Name = "值班室", AffairTypeId = 3 }, 
                    new Workplace { TenantId = _tenantId, DepotId = 4, Name = "监控室", AffairTypeId = 4, RoleUserName = "PlaceC" }, 
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateWorkers()
        {
            if (_context.Workers.Count() == 0)
            {
                _context.Workers.AddRange(new Worker[] {
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "11110", Name = "王宽", PostId = 1, Password = "123456", Rfid = "", AdditiveInfo = "", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10001", Name = "高鹏", PostId = 2, Password = "123456", Rfid = "10001", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10002", Name = "许振亚", PostId = 3, Password = "123456", Rfid = "10002", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10003", Name = "陈俊杰", PostId = 4, Password = "123456", Rfid = "10003", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10004", Name = "曲斌", PostId = 5, Password = "123456", Rfid = "10004", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10005", Name = "程涛", PostId = 6, Password = "123456", Rfid = "10004", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10010", Name = "郭杰", PostId = 7, Password = "123456", Rfid = "10010", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10011", Name = "赵帅", PostId = 7, Password = "123456", Rfid = "10011", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10012", Name = "裴孟林", PostId = 8, Password = "123456", Rfid = "10012", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10013", Name = "滕帅斌", PostId = 8, Password = "123456", Rfid = "10013", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10014", Name = "申晓强", PostId = 8, Password = "123456", Rfid = "10014", AdditiveInfo = "142701198001041239", IsActive = true }, 
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateVehicles()
        {
            if (_context.Vehicles.Count() == 0)
            {
                _context.Vehicles.AddRange(new Vehicle[] {
                    new Vehicle { TenantId = _tenantId, DepotId = 1, Cn = "001", License = "晋MNU144" },
                    new Vehicle { TenantId = _tenantId, DepotId = 1, Cn = "002", License = "晋MNU145" },
                    new Vehicle { TenantId = _tenantId, DepotId = 1, Cn = "003", License = "晋MNU146" },
                }); 
                _context.SaveChanges();
            }
        }

        private void CreateArticles()
        {
            if (_context.Articles.Count() == 0)
            {
                _context.Articles.AddRange(new Article[] {
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01A001", Name = "中心1号枪(枪号03041635)", ArticleTypeId = 1, Rfid = "001" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01A002", Name = "中心2号枪(枪号08041635)", ArticleTypeId = 1, Rfid = "002" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01B001", Name = "中心1号弹夹", ArticleTypeId = 2, Rfid = "003" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01B002", Name = "中心2号弹夹", ArticleTypeId = 2, Rfid = "004" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01C001", Name = "中心05号车", ArticleTypeId = 3, Rfid = "005" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01C002", Name = "中心09号车", ArticleTypeId = 3, Rfid = "006" },
                }); 
                _context.SaveChanges();
            }
        }

        // Clients
        private void CreateCustomers()
        {
            if (_context.Customers.Count() == 0)
            {
                _context.Customers.AddRange(new Customer[] {
                    new Customer { TenantId = _tenantId, Cn = "01", Name = "工商银行" },
                    new Customer { TenantId = _tenantId, Cn = "02", Name = "农业银行" },
                    new Customer { TenantId = _tenantId, Cn = "03", Name = "中国银行" },
                });
                _context.SaveChanges();
            }
        }
        private void CreateOutlets()
        {
            if (_context.Outlets.Count() == 0)
            {
                _context.Outlets.AddRange(new Outlet[] {
                    new Outlet { TenantId = _tenantId, CustomerId = 1, Cn = "010102", Name = "工行分行营业部" },
                    new Outlet { TenantId = _tenantId, CustomerId = 1, Cn = "011001", Name = "工行大营支行" },
                    new Outlet { TenantId = _tenantId, CustomerId = 2, Cn = "020101", Name = "农行分行营业部" },
                    new Outlet { TenantId = _tenantId, CustomerId = 2, Cn = "023110", Name = "农行武大分理处" },
                });
                _context.SaveChanges();
            }

        }
        private void CreateBoxes()
        {
            if (_context.Boxes.Count() == 0)
            {
                _context.Boxes.AddRange(new Box[] {
                    new Box { TenantId = _tenantId, OutletId = 1, Cn = "01010202", Name = "2号箱" },
                    new Box { TenantId = _tenantId, OutletId = 1, Cn = "01010212", Name = "12号箱" },
                    new Box { TenantId = _tenantId, OutletId = 2, Cn = "01100101", Name = "1号箱" },
                    new Box { TenantId = _tenantId, OutletId = 2, Cn = "01100103", Name = "3号箱" },
                    new Box { TenantId = _tenantId, OutletId = 3, Cn = "02010105", Name = "5号箱" },
                    new Box { TenantId = _tenantId, OutletId = 3, Cn = "02010106", Name = "6号箱" },
                    new Box { TenantId = _tenantId, OutletId = 4, Cn = "02311015", Name = "15号箱" },
                    new Box { TenantId = _tenantId, OutletId = 4, Cn = "02311016", Name = "16号箱" },
                });
                _context.SaveChanges();
            }

        }

    }
}
