using Clc.Clients;

namespace Clc.Runtime.Cache
{
    public interface ICustomerCache : IEntityListCache<Customer, Customer, Customer>
    {
    }

    public interface IOutletCache : IEntityListCache<Outlet, Outlet, Outlet>
    {
    }

    public interface IBoxCache : IEntityListCache<Box, Box, Box>
    {
    }

    public interface ICustomerTaskTypeCache : IEntityListCache<CustomerTaskType, CustomerTaskType, CustomerTaskType>
    {
    }

    public interface IOutletTaskTypeCache : IEntityListCache<OutletTaskType, OutletTaskType, OutletTaskType>
    {
    }
}