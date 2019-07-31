using System.Collections.Generic;
using Clc.Fields.Entities;

namespace Clc.Fields.Cache
{
    public interface IDepotCache
    {
        List<Depot> GetList();

        Depot GetById(int id);
    }
}