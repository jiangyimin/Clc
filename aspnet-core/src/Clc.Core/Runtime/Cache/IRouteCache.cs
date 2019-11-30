using System;
using System.Collections.Generic;
using Clc.Routes;

namespace Clc.Runtime.Cache
{
    public interface IRouteCache
    {
        void Set(DateTime carryoutDate, int depotId, object value);
        List<RouteCacheItem> Get(DateTime carryoutDate, int depotId);
        RouteCacheItem GetRoute(DateTime carryoutDate, int depotId, int routeId);

        (RouteCacheItem, RouteWorkerCacheItem) GetRouteWorker(DateTime carryoutDate, int depotId, int workerId);
    }

    
}