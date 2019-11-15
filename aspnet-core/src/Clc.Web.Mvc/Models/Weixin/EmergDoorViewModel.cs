using System.Collections.Generic;

namespace Clc.Web.Models.Weixin
{
    public class EmergDoorViewModel
    {
        public int WorkplaceId { get; set; }
        public string Content { get; set; }

        public List<ComboItemModel> Workplaces {get; set; }

    }
}