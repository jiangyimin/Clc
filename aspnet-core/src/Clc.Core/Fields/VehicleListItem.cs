using Abp.AutoMapper;

namespace Clc.Fields
{
    /// <summary>
    /// VehicleListItem Entity
    /// </summary>
    [AutoMapFrom(typeof(Vehicle))]
    public class VehicleListItem
    {      
        public int Id { get; set; }  
        public string License { get; set; }
    }
}

