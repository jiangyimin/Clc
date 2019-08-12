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

        public int DepotId { get; set; }
        public string Cn { get; set; }
        public string License { get; set; }

        public string CnLicense { 
            get {
                return string.Format("{0} {1}", Cn, License);
            }
        }

    }
}

