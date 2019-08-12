using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.PreRoutes
{
    /// <summary>
    /// PreRouteWorker Entity
    /// </summary>
    [Description("预排线路人员")]
    public class PreRouteWorker : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int PreRouteId { get; set; }
        public virtual PreRoute PreRoute { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }   

        [Required]
        public int WorkRoleId { get; set; }
        public WorkRole WorkRole { get; set; }  
    }
}

