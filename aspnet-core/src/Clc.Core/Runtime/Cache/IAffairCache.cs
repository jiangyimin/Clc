using System;
using System.Collections.Generic;
using Clc.Affairs;

namespace Clc.Runtime.Cache
{
    public interface IAffairCache
    {
        void Set(DateTime carryoutDate, int depotId, object value);

        List<AffairCacheItem> Get(DateTime carryoutDate, int depotId);

        AffairCacheItem GetAffair(DateTime carryoutDate, int depotId, int id);
        
        (AffairCacheItem, AffairWorkerCacheItem) GetAffairWorker(DateTime carryoutDate, int depotId, int workerId);
        (AffairCacheItem, AffairWorkerCacheItem) GetAffairWorker(DateTime carryoutDate, int depotId, int affairId, int workerId);
    }

    
}