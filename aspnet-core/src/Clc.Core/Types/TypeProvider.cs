using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Clc.Types.Cache;
using Clc.Types.Entities;

namespace Clc.Types
{
    /// <summary>
    /// Depot manager.
    /// Implements Typs Manager.
    /// </summary>
    public class TypeProvider : ITransientDependency
    {        
        private readonly IAffairTypeCache _affairTypeCache;
        private readonly IArticleTypeCache _articleTypeCache;
        private readonly IPostCache _postCache;
        private readonly IRouteTypeCache _routeTypeCache;
        private readonly ITaskTypeCache _taskTypeCache;
        private readonly IWorkRoleCache _workRoleCache;
        
        private readonly List<string> _bindStyleItems = new List<string>() {"人", "车", "线路" };

        public TypeProvider(
            AffairTypeCache affairTypeCache,
            ArticleTypeCache articleTypeCache,
            PostCache postCache,
            RouteTypeCache routeTypeCache,
            TaskTypeCache taskTypeCache,
            WorkRoleCache workRoleCache )
        {
            _affairTypeCache = affairTypeCache;
            _articleTypeCache = articleTypeCache;
            _postCache = postCache;
            _routeTypeCache = routeTypeCache;
            _taskTypeCache = taskTypeCache;
            _workRoleCache = workRoleCache;
        }
        
        public List<ComboboxItemDto> GetComboItems(string tableName)
        {
            var lst = new List<ComboboxItemDto>();
            switch (tableName) 
            {
                case "AffairType":
                    foreach (AffairType t in _affairTypeCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
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
                case "BindStyle":
                    foreach (string t in _bindStyleItems)
                        lst.Add(new ComboboxItemDto { Value = t, DisplayText = t });
                    break;
                default:
                    break;
            }
            return lst;
        }
    }
}
 