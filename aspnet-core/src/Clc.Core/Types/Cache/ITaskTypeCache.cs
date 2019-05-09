using System.Collections.Generic;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public interface ITaskTypeCache
    {
        List<TaskType> GetList();

        TaskType GetById(int id);

        TaskType GetByCn(string cn);
    }
}