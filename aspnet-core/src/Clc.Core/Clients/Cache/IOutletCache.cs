using System.Collections.Generic;
using Clc.Clients.Entities;

namespace Clc.Clients.Cache
{
    public interface IOutletCache
    {
        List<Outlet> GetList();

        Outlet GetById(int id);
        Outlet GetByCn(string cn);
    }
}