using System.Collections.Generic;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public interface IWorkRoleCache
    {
        List<WorkRole> GetList();

        WorkRole GetById(int id);

        WorkRole GetByName(string name);
    }
}