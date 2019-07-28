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
        private readonly IArticleTypeCache _articleTypeCache;
        private readonly IPostCache _postCache;
        private readonly IRouteTypeCache _routeTypeCache;
        private readonly ITaskTypeCache _taskTypeCache;
        private readonly IWorkRoleCache _workRoleCache;


        public TypeProvider(
            ArticleTypeCache articleTypeCache,
            PostCache postCache,
            RouteTypeCache routeTypeCache,
            TaskTypeCache taskTypeCache,
            WorkRoleCache workRoleCache )
        {
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
                case "ArticleType":
                    foreach (ArticleType t in _articleTypeCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
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
                default:
                    break;
            }
            return lst;
        }
    }
}
 