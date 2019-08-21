using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Clc.Clients;
using Clc.Runtime;

namespace Clc.Routes
{
    /// <summary>
    /// RouteInBox
    /// </summary>
    [Description("线路进箱")]
    public class RouteInBox : Entity, IMustHaveTenant
    {
        public const int MaxRemarkLength = ClcConsts.NormalStringLength;

        // Impement of IMustHaveTenant
        public int TenantId { get; set; }

        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        /// <summary>
        /// RouteTask
        /// </summary>
        public int RouteTaskId { get; set; }

        [Required]
        public int BoxRecordId { get; set; }

        public BoxRecord BoxRecord { get; set; }
    }
}

