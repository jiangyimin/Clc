using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Clc.Clients.Cache;
using Clc.Clients.Entities;

namespace Clc.Clients
{
    /// <summary>
    /// Depot manager.
    /// Implements domain logic for depot.
    /// </summary>
    public class ClientProvider : ITransientDependency
    {        
        private readonly ICustomerCache _customerCache;
        private readonly IOutletCache _outletCache;

        public ClientProvider(
            ICustomerCache customerCache,
            IOutletCache outletCache)
        {
            _customerCache = customerCache;
            _outletCache = outletCache;
        }
        
        public List<ComboboxItemDto> GetComboItems(string name)
        {
            var lst = new List<ComboboxItemDto>();
            switch (name) 
            {
               case "CustomerWithCn":
                    foreach (Customer t in _customerCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = string.Format("{0} {1}", t.Cn, t.Name) });
                    break;
               case "OutletWithCn":
                    foreach (Outlet t in _outletCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = string.Format("{0} {1}", t.Cn, t.Name) });
                    break;
                default:
                    break;
            }
            return lst;
        }

    }
}