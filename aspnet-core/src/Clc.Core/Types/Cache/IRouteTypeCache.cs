using System.Collections.Generic;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public interface IRouteTypeCache
    {
        List<RouteType> GetList();

        RouteType GetById(int id);
    }
}