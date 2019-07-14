using System.Collections.Generic;
using Clc.Types.Entities;

namespace Clc.Fields.Cache
{
    public interface IDepotCache
    {
        List<Depot> GetList();

        Depot GetById(int id);
    }
}