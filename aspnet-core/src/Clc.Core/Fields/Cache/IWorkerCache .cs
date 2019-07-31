using System.Collections.Generic;
using Clc.Fields.Entities;

namespace Clc.Fields.Cache
{
    public interface IWorkerCache
    {
        List<Worker> GetList();

        Worker GetById(int id);
        Worker GetByCn(string cn);
    }
}