using System.Collections.Generic;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public interface IAffairTypeCache
    {
        List<AffairType> GetList();

        AffairType GetById(int id);

        AffairType GetByCn(string cn);
    }
}