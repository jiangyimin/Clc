using System.Linq;
using Clc.Types;
using Clc.Fields;
using Clc.Clients;
using Clc.Authorization.Roles;

namespace Clc.EntityFrameworkCore.Seed.Tenants
{
    public class BaseEntitySeedBuilder
    {
        private readonly ClcDbContext _context;
        private readonly int _tenantId;

        public BaseEntitySeedBuilder(ClcDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            // Types
            CreateArticleTypes();
            CreatePosts();
            CreateRouteTypes();
            CreateTaskTypes();
            CreateWorkRoles();

            // Fields
            //CreateDepots();
            //CreateWorkplaces();
            //CreateWorkers();
            //CreateVehicles();
            //CreateArticles();

            // Clients
            //CreateCustomers();
            //CreateOutlets();
            //CreateBoxes();
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
                    new Post { TenantId = _tenantId, Cn = "01", Name = "司机", DefaultWorkRoleName = "司机", AppName = "App02" },
                    new Post { TenantId = _tenantId, Cn = "02", Name = "车长", DefaultWorkRoleName = "车长", AppName = "App02" },
                    new Post { TenantId = _tenantId, Cn = "03", Name = "持枪员", DefaultWorkRoleName = "持枪员", AppName = "App02" },
                    new Post { TenantId = _tenantId, Cn = "04", Name = "主业务员", DefaultWorkRoleName = "主业务员", AppName = "App02" },
                    new Post { TenantId = _tenantId, Cn = "05", Name = "业务员", DefaultWorkRoleName = "业务员", AppName = "App02" },
                    new Post { TenantId = _tenantId, Cn = "06", Name = "库房管理员", DefaultWorkRoleName = "库房管理员", AppName = "App01" },                     
                    new Post { TenantId = _tenantId, Cn = "07", Name = "金库管理员", DefaultWorkRoleName = "金库管理员", AppName = "App01" },                     
                    new Post { TenantId = _tenantId, Cn = "08", Name = "监控员", DefaultWorkRoleName = "监控员", AppName = "App01" },
                    new Post { TenantId = _tenantId, Cn = "09", Name = "大队长", DefaultWorkRoleName = "队长", AppName = "App03" }, 
                    new Post { TenantId = _tenantId, Cn = "10", Name = "领导", DefaultWorkRoleName = "公司领导", AppName = "App03" }, 
                    new Post { TenantId = _tenantId, Cn = "11", Name = "干部", AppName = "App03" }, 
                    new Post { TenantId = _tenantId, Cn = "12", Name = "职员", AppName = "App03" }
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateRouteTypes()
        {
            if (_context.RouteTypes.Count() == 0)
            {
                _context.RouteTypes.AddRange(new RouteType[] {
                    new RouteType { TenantId = _tenantId, Name = "押运", WorkRoles = "司机|车长|持枪员|主业务员|业务员", LendArticleLead = 60, LendArticleDeadline = 60, ActivateLead = 120 }
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
                    new TaskType { TenantId = _tenantId, Cn = "04", Name = "收费中调", isTemporary = true, BasicPrice = 180 }
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateWorkRoles()
        {
            if (_context.WorkRoles.Count() == 0)
            {
                _context.WorkRoles.AddRange(new WorkRole[] {
                    new WorkRole { TenantId = _tenantId, Cn = "01", Name = "司机", ArticleTypeList = "C", mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "02", Name = "车长", ArticleTypeList = "A|B|D", mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "03", Name = "持枪员", ArticleTypeList = "A|B|D", mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "04", Name = "主业务员", Duties = "尾箱|交接", mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "05", Name = "业务员", Duties = "辅助交接", mustHave = true, MaxNum = 2 },
                    new WorkRole { TenantId = _tenantId, Cn = "06", Name = "库房管理员", mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "07", Name = "金库管理员", Duties = "金库", mustHave = true, MaxNum = 8 }, 
                    new WorkRole { TenantId = _tenantId, Cn = "08", Name = "监控员", mustHave = true, MaxNum = 3 },
                 }); 
                _context.SaveChanges();
            }
        }

        private void CreateDepots()
        {
            if (_context.Depots.Count() == 0)
            {
                _context.Depots.AddRange(new Depot[] {
                    new Depot { TenantId = _tenantId, Cn = "01", Name = "中心", Longitude = 114.022022, Latitude = 22.531167, UnlockScreenPassword = "654321", ReportTo = "90005", AllowCardWhenCheckin = true }, 
                    new Depot { TenantId = _tenantId, Cn = "02", Name = "调度", Longitude = 114.016667, Latitude = 22.534167, UnlockScreenPassword = "654321", ReportTo = "90005", ActiveRouteNeedCheckin = true }, 
                    new Depot { TenantId = _tenantId, Cn = "10", Name = "万荣", Longitude = 114.016667, Latitude = 22.534167, UnlockScreenPassword = "654321"}, 
                    new Depot { TenantId = _tenantId, Cn = "91", Name = "监控", Longitude = 114.016667, Latitude = 22.534167, UnlockScreenPassword = "654321"}, 
                    new Depot { TenantId = _tenantId, Cn = "99", Name = "总部", Longitude = 114.016667, Latitude = 22.534167, UnlockScreenPassword = "654321"}, 
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateWorkplaces()
        {
            if (_context.Workplaces.Count() == 0)
            {
                _context.Workplaces.AddRange(new Workplace[] {
                    new Workplace { TenantId = _tenantId, DepotId = 1, Name = "库房", WorkRoles = "金库管理员", MinDuration = 4, MaxDuration = 12}, 
                    new Workplace { TenantId = _tenantId, DepotId = 2, Name = "库房", WorkRoles = "金库管理员", ShareDepotList="01", ArticleTypeList = "A|B|C|D", MinDuration = 4, MaxDuration = 20, DoorIp = "192.168.1.100,1", CameraIp = "192.168.20.120", AskOpenStyle = "直接", EmergPassword = "654321" },
                    new Workplace { TenantId = _tenantId, DepotId = 2, Name = "金库", WorkRoles = "金库管理员", ShareDepotList="01", MinDuration = 1, MaxDuration = 20, DoorIp = "192.168.2.100,2", AskOpenStyle = "验证", EmergPassword = "654321" }, 
                    new Workplace { TenantId = _tenantId, DepotId = 2, Name = "调度室", WorkRoles = "辅助调度", ShareDepotList="01", MinDuration = 4, MaxDuration = 20}, 
                    new Workplace { TenantId = _tenantId, DepotId = 4, Name = "监控室", WorkRoles = "监控员", MinDuration = 6, MaxDuration = 20 }, 
                }); 
                _context.SaveChanges();
            }
        }
        private void CreateWorkers()
        {
            if (_context.Workers.Count() == 0)
            {
                _context.Workers.AddRange(new Worker[] {
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "99999", Name = "李明", PostId = 1, Password = "123456", LoginRoleNames = "Captain", Rfid = "33336", AdditiveInfo = "142701198001041239", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10001", Name = "高鹏", PostId = 2, Password = "123456", Rfid = "10001", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10002", Name = "许振亚", PostId = 2, Password = "123456", Rfid = "10002", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10003", Name = "陈俊杰", PostId = 3, Password = "123456", Rfid = "10003", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10004", Name = "曲斌", PostId = 3, Password = "123456", Rfid = "10004", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10005", Name = "程涛", PostId = 4, Password = "123456", Rfid = "10005", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10006", Name = "郭杰", PostId = 4, Password = "123456", Rfid = "10006", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10007", Name = "曲斌", PostId = 5, Password = "123456", Rfid = "10007", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10008", Name = "程涛", PostId = 5, Password = "123456", Rfid = "10008", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 1, Cn = "10009", Name = "吴风涛", PostId = 6, Password = "123456", Rfid = "10009", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "10010", Name = "郭杰", PostId = 6, Password = "123456", Rfid = "10010", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "10021", Name = "高鹏二", PostId = 2, Password = "123456", Rfid = "10021", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "10022", Name = "陈俊亚", PostId = 3, Password = "123456", Rfid = "10022", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "10023", Name = "曲三", PostId = 4, Password = "123456", Rfid = "10023", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "10024", Name = "程涛二", PostId = 5, Password = "123456", Rfid = "10024", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "10025", Name = "郭杰二", PostId = 6, Password = "123456", Rfid = "10025", AdditiveInfo = "142701198001041239", IsActive = true }, 
                });
                _context.SaveChanges();
                _context.Workers.AddRange(new Worker[] {
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "90005", Name = "测试", PostId = 1, Password = "123456", Rfid = "90005", LoginRoleNames = "Captain", AdditiveInfo = "142701198001041239", IsActive = true, WorkRoleNames = "车长|司机|库房管理员|金库管理员" }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "20001", Name = "赵帅", PostId = 7, Password = "123456", LoginRoleNames = "Article|Box", Rfid = "20001", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "20002", Name = "裴孟林", PostId = 7, Password = "123456", LoginRoleNames = "Article|Box", Rfid = "20002", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "20003", Name = "滕帅斌", PostId = 8, Password = "123456", LoginRoleNames = "Box", Rfid = "20003", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 2, Cn = "20004", Name = "申晓强", PostId = 8, Password = "123456", LoginRoleNames = "Box", Rfid = "20004", AdditiveInfo = "142701198001041239", IsActive = true }, 
                    new Worker { TenantId = _tenantId, DepotId = 5, Cn = "20009", Name = "辅助调度", PostId = 7, Password = "123456", LoginRoleNames = "Box|Aux", Rfid = "60001", AdditiveInfo = "142701198001041239", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 4, Cn = "99998", Name = "监控队长", PostId = 1, Password = "123456", LoginRoleNames = "Captain", Rfid = "99998", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 4, Cn = "20015", Name = "王宽", PostId = 10, Password = "123456", LoginRoleNames = "Monitor", Rfid = "20015", AdditiveInfo = "142701198001041239", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 4, Cn = "20016", Name = "陈灼", PostId = 10, Password = "123456", LoginRoleNames = "Monitor", Rfid = "20016", AdditiveInfo = "142701198001041239", IsActive = true },

                    new Worker { TenantId = _tenantId, DepotId = 5, Cn = "60001", Name = "系统管理", PostId = 12, Password = "123456", LoginRoleNames = StaticRoleNames.Tenants.Admin, Rfid = "60001", AdditiveInfo = "142701198001041239", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 5, Cn = "60002", Name = "人事编辑", PostId = 12, Password = "123456", LoginRoleNames = StaticRoleNames.Tenants.Hrm, Rfid = "60002", AdditiveInfo = "142701198001041239", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 5, Cn = "60003", Name = "人事查询", PostId = 12, Password = "123456", LoginRoleNames = StaticRoleNames.Tenants.Hrq, Rfid = "60003", AdditiveInfo = "142701198001041239", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 5, Cn = "60010", Name = "领导一", PostId = 11, Password = "123456", IsActive = true },
                    new Worker { TenantId = _tenantId, DepotId = 5, Cn = "60011", Name = "领导一", PostId = 11, Password = "123456", IsActive = true },
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
                    new Vehicle { TenantId = _tenantId, DepotId = 1, Cn = "901", License = "晋MNU245", Remark = "机动" },
                    new Vehicle { TenantId = _tenantId, DepotId = 2, Cn = "103", License = "晋MNU146" },
                }); 
                _context.SaveChanges();
            }
        }

        private void CreateArticles()
        {
            if (_context.Articles.Count() == 0)
            {
                _context.Articles.AddRange(new Article[] {
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01A001", Name = "中心1号枪(枪号03041635)", ArticleTypeId = 1, Rfid = "101" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01A002", Name = "中心2号枪(枪号08041638)", ArticleTypeId = 1, Rfid = "102" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01A003", Name = "中心3号枪(枪号03041436)", ArticleTypeId = 1, Rfid = "103" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01A004", Name = "中心4号枪(枪号08041639)", ArticleTypeId = 1, Rfid = "104" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01B001", Name = "中心1号弹夹", ArticleTypeId = 2, Rfid = "201" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01B002", Name = "中心2号弹夹", ArticleTypeId = 2, Rfid = "202" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01B003", Name = "中心3号弹夹", ArticleTypeId = 2, Rfid = "203" }, 
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01B004", Name = "中心4号弹夹", ArticleTypeId = 2, Rfid = "204" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01C001", Name = "中心01号车", BindInfo = "001", ArticleTypeId = 3, Rfid = "301" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01C002", Name = "中心02号车", BindInfo = "002", ArticleTypeId = 3, Rfid = "302" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01C003", Name = "中心03号车", BindInfo = "003", ArticleTypeId = 3, Rfid = "303" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01D001", Name = "高鹏枪证(142701302A058)", BindInfo = "10001", ArticleTypeId = 4, Rfid = "401" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01D002", Name = "许振亚枪证(142701302A060)", BindInfo = "10002", ArticleTypeId = 4, Rfid = "402" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01D003", Name = "陈俊杰枪证(142701302A064)", BindInfo = "10003", ArticleTypeId = 4, Rfid = "403" },
                    new Article { TenantId = _tenantId, DepotId = 1, Cn = "01D004", Name = "曲斌枪证(142701302A152)", BindInfo = "10004", ArticleTypeId = 4, Rfid = "404" },
                }); 
                _context.SaveChanges();
                _context.Articles.AddRange(new Article[] {
                    new Article { TenantId = _tenantId, DepotId = 2, Cn = "02A001", Name = "调度1号枪(枪号03041635)", ArticleTypeId = 1, Rfid = "501" }, 
                    new Article { TenantId = _tenantId, DepotId = 2, Cn = "02A002", Name = "调度2号枪(枪号08041638)", ArticleTypeId = 1, Rfid = "502" }, 
                    new Article { TenantId = _tenantId, DepotId = 2, Cn = "02B001", Name = "调度1号弹夹", ArticleTypeId = 2, Rfid = "601" }, 
                    new Article { TenantId = _tenantId, DepotId = 2, Cn = "02B002", Name = "调度2号弹夹", ArticleTypeId = 2, Rfid = "602" }, 
                    new Article { TenantId = _tenantId, DepotId = 2, Cn = "02C001", Name = "调度01号车", BindInfo = "103", ArticleTypeId = 3, Rfid = "701" },
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
                    //new Customer { TenantId = _tenantId, Cn = "11", Name = "运城商业银行" },
                });
                _context.SaveChanges();
            }
        }
        private void CreateOutlets()
        {
            if (_context.Outlets.Count() == 0)
            {
                _context.Outlets.AddRange(new Outlet[] {
                    new Outlet { TenantId = _tenantId, CustomerId = 1, Cn = "010102", Name = "工行分行营业部", Password = "123456", Ciphertext = "654321", Weixins = "90005" },
                    new Outlet { TenantId = _tenantId, CustomerId = 1, Cn = "011001", Name = "工行大营支行", Password = "123456", Ciphertext = "654321", Weixins = "90005" },
                    new Outlet { TenantId = _tenantId, CustomerId = 1, Cn = "011003", Name = "工行临汾分理处", Password = "123456", Ciphertext = "654321", Weixins = "90005" },
                    new Outlet { TenantId = _tenantId, CustomerId = 2, Cn = "020101", Name = "农行分行营业部", Password = "123456", Ciphertext = "654321", Weixins = "90005" },
                    new Outlet { TenantId = _tenantId, CustomerId = 2, Cn = "020102", Name = "农行万达支行", Password = "123456", Ciphertext = "654321", Weixins = "90005" },
                    new Outlet { TenantId = _tenantId, CustomerId = 2, Cn = "023110", Name = "农行武大分理处", Password = "123456", Ciphertext = "654321", Weixins = "90005" },
                    new Outlet { TenantId = _tenantId, CustomerId = 3, Cn = "030410", Name = "中行惠凯丽分理处", Password = "123456", Ciphertext = "654321", Weixins = "90005" },
                    new Outlet { TenantId = _tenantId, CustomerId = 3, Cn = "030511", Name = "中行杰拉德支行", Password = "123456", Ciphertext = "654321", Weixins = "90005" },
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
                    new Box { TenantId = _tenantId, OutletId = 3, Cn = "01100302", Name = "2号箱" },
                    new Box { TenantId = _tenantId, OutletId = 3, Cn = "01100304", Name = "4号箱" },
                    new Box { TenantId = _tenantId, OutletId = 4, Cn = "02010105", Name = "5号箱" },
                    new Box { TenantId = _tenantId, OutletId = 4, Cn = "02010106", Name = "6号箱" },
                    new Box { TenantId = _tenantId, OutletId = 5, Cn = "02010207", Name = "7号箱" },
                    new Box { TenantId = _tenantId, OutletId = 5, Cn = "02010208", Name = "8号箱" },
                    new Box { TenantId = _tenantId, OutletId = 6, Cn = "02311015", Name = "15号箱" },
                    new Box { TenantId = _tenantId, OutletId = 6, Cn = "02311016", Name = "16号箱" },
                });
                _context.SaveChanges();
            }

        }
    }
}
