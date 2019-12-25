using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Clc.Runtime.Cache;

namespace Clc.Clients
{
    [AbpAuthorize]
    public class ClientAppService : ClcAppServiceBase, IClientAppService
    {
        private readonly ICustomerCache _customerCache;

        private readonly IOutletCache _outletCache;

        private readonly IBoxCache _boxCache;

        public ClientAppService(ICustomerCache customerCache, IOutletCache outletCache, IBoxCache boxCache)
        {
            _customerCache = customerCache;
            _outletCache = outletCache;
            _boxCache = boxCache;
        }

        public List<ComboboxItemDto> GetComboItems(string name)
        {
            var lst = new List<ComboboxItemDto>();
            switch (name) 
            {
               case "Customer":
                    foreach (Customer t in _customerCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
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

        public List<ComboboxItemDto> GetBoxes(int outletId)
        {
            var lst = new List<ComboboxItemDto>();
            foreach (Box b in  _boxCache.GetList().FindAll(x => x.OutletId == outletId))
                lst.Add(new ComboboxItemDto { Value = b.Cn, DisplayText = string.Format("{0} {1}", b.Cn, b.Name) });
            return lst;
        }
        
    }
}