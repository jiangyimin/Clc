using System.Collections.Generic;
using Clc.Fields.Entities;

namespace Clc.Fields.Cache
{
    public interface IVehicleCache
    {
        List<Vehicle> GetList();

        Vehicle GetById(int id);
        Vehicle GetByCn(string cn);
    }
}