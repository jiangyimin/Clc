using System;
using System.Collections.Generic;
using Clc.Works.Dto;

namespace Clc.Works
{
    public interface IRouteCache
    {
        List<RouteCDto> Get(DateTime carroutDate, int affairId);
        void Set(DateTime carroutDate, int affairId, List<RouteCDto> routes);
    }
}