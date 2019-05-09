using System.Collections.Generic;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public interface IWorkerTypeCache
    {
        List<WorkerType> GetList();

        WorkerType GetById(int id);

        WorkerType GetByCn(string cn);
    }
}