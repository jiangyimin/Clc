using System.Collections.Generic;
using Clc.Fields.Entities;

namespace Clc.Fields.Cache
{
    public interface IWorkplaceCache
    {
        List<Workplace> GetList();

        Workplace GetById(int id);
        Workplace GetByName(int depotId, string name);
    }
}