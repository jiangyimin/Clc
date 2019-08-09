using Clc.Clients;

namespace Clc.Runtime.Cache
{
    public interface ICustomerCache : IEntityListCache<Customer, Customer, Customer>
    {
    }

    public interface IOutletCache : IEntityListCache<Outlet, Outlet, Outlet>
    {
    }
}