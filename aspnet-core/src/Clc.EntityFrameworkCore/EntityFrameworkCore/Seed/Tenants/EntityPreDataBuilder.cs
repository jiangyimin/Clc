using System.Linq;
using System.Collections.Generic;
using Clc.Types.Entities;

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
            CreateTypes();
            //CreateSources();
        }

        private void CreateTypes()
        {
            // ArticleType
            if (_context.ArticleTypes.Count() == 0)
            {
                _context.ArticleTypes.AddRange(new ArticleType[] {
                    new ArticleType { TenantId = _tenantId, Cn = "A", Name = "枪"}, 
                    new ArticleType { TenantId = _tenantId, Cn = "B", Name = "弹"}, 
                    new ArticleType { TenantId = _tenantId, Cn = "C", Name = "枪证", BindStyle = "人" },
                    new ArticleType { TenantId = _tenantId, Cn = "D", Name = "车钥匙", BindStyle = "车" }                   
                }); 
                _context.SaveChanges();
            }

            // Post
            if (_context.Posts.Count() == 0)
            {
                _context.Posts.AddRange(new Post[] {
                    new Post { TenantId = _tenantId, Cn = "01", Name = "大队长" }, 
                    new Post { TenantId = _tenantId, Cn = "02", Name = "内务" },                     
                    new Post { TenantId = _tenantId, Cn = "03", Name = "司机" },
                    new Post { TenantId = _tenantId, Cn = "04", Name = "持枪员" },
                    new Post { TenantId = _tenantId, Cn = "05", Name = "主业务员" },
                    new Post { TenantId = _tenantId, Cn = "06", Name = "业务员" },
                    new Post { TenantId = _tenantId, Cn = "07", Name = "领导" }, 
                    new Post { TenantId = _tenantId, Cn = "08", Name = "干部" }, 
                    new Post { TenantId = _tenantId, Cn = "09", Name = "职员" }
                }); 
                _context.SaveChanges();
            }
            // RouteType
            if (_context.RouteTypes.Count() == 0)
            {
                _context.RouteTypes.AddRange(new RouteType[] {
                    new RouteType { TenantId = _tenantId, Name = "押运", WorkRoles = "司机|持枪员一|持枪员二|主业务员|业务员" }
                }); 
                _context.SaveChanges();
            }
            // TaskType
            if (_context.TaskTypes.Count() == 0)
            {
                _context.TaskTypes.AddRange(new TaskType[] {
                    new TaskType { TenantId = _tenantId, Cn = "01", Name = "早送" }, 
                    new TaskType { TenantId = _tenantId, Cn = "01", Name = "晚接" }, 
                    new TaskType { TenantId = _tenantId, Cn = "01", Name = "中调" }, 
                    new TaskType { TenantId = _tenantId, Cn = "01", Name = "收费中调", isCharge = true, DefaultPrice = 180 }
                }); 
                _context.SaveChanges();
            }
            // WorkRole
            if (_context.WorkRoles.Count() == 0)
            {
                _context.WorkRoles.AddRange(new WorkRole[] {
                    new WorkRole { TenantId = _tenantId, Name = "监控", mustHave = true, MaxNum = 3 }, 
                    new WorkRole { TenantId = _tenantId, Name = "库房", DefaultPostId = 2, mustHave = true, MaxNum = 2 }, 
                    new WorkRole { TenantId = _tenantId, Name = "金库", DefaultPostId = 2, mustHave = true, MaxNum = 8 }, 
                    new WorkRole { TenantId = _tenantId, Name = "值班", mustHave = true, MaxNum = 4 }, 
                    new WorkRole { TenantId = _tenantId, Name = "司机", DefaultPostId = 3, mustHave = true, MaxNum = 1 }, 
                    new WorkRole { TenantId = _tenantId, Name = "持枪员一", DefaultPostId = 4, mustHave = true, MaxNum = 1 }, 
                    new WorkRole { TenantId = _tenantId, Name = "持枪员二", DefaultPostId = 4, mustHave = true, MaxNum = 1 }, 
                    new WorkRole { TenantId = _tenantId, Name = "主业务员", DefaultPostId = 5, mustHave = true, MaxNum = 1 }, 
                    new WorkRole { TenantId = _tenantId, Name = "业务员", DefaultPostId = 6, mustHave = true, MaxNum = 1 }
                }); 
                _context.SaveChanges();
            }
        }
    }
}
