using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Clc.Runtime.Cache;

namespace Clc.Types
{
    [AbpAuthorize]
    public class TypeAppService : ClcAppServiceBase, ITypeAppService
    {
        private readonly DutyProvider _dutyProvider;
        private readonly IArticleTypeCache _articleTypeCache;
        private readonly IPostCache _postCache;
        private readonly IRouteTypeCache _routeTypeCache;
        private readonly ITaskTypeCache _taskTypeCache;
        private readonly IWorkRoleCache _workRoleCache;
        private readonly IWorkplaceCache _workplaceCache;

        private readonly List<string>　_askOpenStyleItems = new List<string>() {"直接", "验证", "线路"};
        private readonly List<string> _bindStyleItems = new List<string>() {"人", "车", "线路" };
        private readonly List<string> _sexItems = new List<string>() {"男", "女" };
        private readonly List<string> _politicalStatusItems = new List<string>() {"党员", "团员", "群众" };
        private readonly List<string> _educationItems = new List<string>() {"本科", "大专", "中专", "高中", "初中", "小学" };
        private readonly List<string> _maritalStatusItems = new List<string>() {"已婚", "未婚", "离异" };
        private readonly List<string> _licenseTypeItems = new List<string>() {"A照", "B照", "C照" };
        private readonly List<string> _statusItems = new List<string>() {"在职", "离职" };
        private readonly List<string> _processStyleItems = new List<string>() {"报送", "应急开门", "值班信息" };

        private readonly List<string> _categoryItems = new List<string>() {"电子设备", "办公设备", "电脑" };

        public TypeAppService(DutyProvider dutyProvider, 
            IArticleTypeCache articleTypeCache,
            IPostCache postCache,
            IRouteTypeCache routeTypeCache,
            ITaskTypeCache taskTypeCache,
            IWorkRoleCache workRoleCache,
            IWorkplaceCache workplaceCache)
        {
            _dutyProvider = dutyProvider;
            _articleTypeCache = articleTypeCache;
            _postCache = postCache;
            _routeTypeCache = routeTypeCache;
            _taskTypeCache = taskTypeCache;
            _workRoleCache = workRoleCache;
            _workplaceCache = workplaceCache;
        }

       
        public Task<List<ComboboxItemDto>> GetDutyCategories()
        {
            List<ComboboxItemDto> lst = new List<ComboboxItemDto>();
            foreach (var c in _dutyProvider.GetDutyCategory()) 
            {
                lst.Add(new ComboboxItemDto() { Value = c, DisplayText = c});
            }
            return Task.FromResult<List<ComboboxItemDto>>(lst);            
        }
        
        public Task<List<ComboboxItemDto>> GetComboItems(string typeName)
        {
            var lst = new List<ComboboxItemDto>();
            switch (typeName) 
            {
                case "ArticleType":
                    foreach (ArticleType t in _articleTypeCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = string.Format("{0} {1}", t.Cn, t.Name) });
                    break;
                case "Post":
                    foreach (Post t in _postCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
                case "RouteType":
                    foreach (RouteType t in _routeTypeCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
                case "TaskType":
                    foreach (TaskType t in _taskTypeCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
                case "WorkRole":
                    foreach (WorkRole t in _workRoleCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
                case "AskOpenStyle":
                    foreach (string t in _askOpenStyleItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                case "BindStyle":
                    foreach (string t in _bindStyleItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                case "Sex":
                    foreach (string t in _sexItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                case "PoliticalStatus":
                    foreach (string t in _politicalStatusItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                case "Education":
                    foreach (string t in _educationItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                case "MaritalStatus":
                    foreach (string t in _maritalStatusItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                 case "LicenseType":
                    foreach (string t in _licenseTypeItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                case "Status":
                    foreach (string t in _statusItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                case "ProcessStyle":
                    foreach (string t in _processStyleItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                case "Category":
                    foreach (string t in _categoryItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                default:
                    break;
            }
             return Task.FromResult<List<ComboboxItemDto>>(lst);
        } 

        public List<WorkRole> GetWorkRoleItems(int workplaceId)        
        {
            List<string> targetRoles = new List<string>(_workplaceCache[workplaceId].WorkRoles.Split('|', ','));
            var all = _workRoleCache.GetList();
            return all.FindAll(x => targetRoles.Contains(x.Name));
        }
       
        public List<WorkRole> GetRouteWorkRoleItems(int routeTypeId)
        {
            List<string> targetRoles = new List<string>(_routeTypeCache[routeTypeId].WorkRoles.Split('|', ','));
            var all = _workRoleCache.GetList();
            return all.FindAll(x => targetRoles.Contains(x.Name));
        }
    }
}