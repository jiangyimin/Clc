using System;
using System.Collections.Generic;
using Clc.Works.Dto;

namespace Clc.Works
{
    public interface IRouteCache
    {
        List<RouteCDto> Get(DateTime carroutDate, int affairId, string type);
        void Set(DateTime carroutDate, int affairId, string type, List<RouteCDto> routes);
    }
}