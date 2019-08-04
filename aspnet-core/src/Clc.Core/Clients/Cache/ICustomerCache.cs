using System.Collections.Generic;
using Clc.Clients.Entities;

namespace Clc.Clients.Cache
{
    public interface ICustomerCache
    {
        List<Customer> GetList();

        Customer GetById(int id);
        Customer GetByCn(string cn);
    }
}