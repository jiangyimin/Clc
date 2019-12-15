using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.PreRoutes
{
    /// <summary>
    /// PreVehicleWorker Entity
    /// </summary>
    [Description("预排车组人员")]
    public class PreVehicleWorker : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }   

        [Required]
        public int WorkRoleId { get; set; }
        public WorkRole WorkRole { get; set; }  
    }
}

