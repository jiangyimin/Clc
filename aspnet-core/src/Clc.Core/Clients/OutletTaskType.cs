using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.Clients
{
    /// <summary>
    /// OutletTaskType Entity
    /// </summary>
    [Description("押运网点的任务类型")]
    public class OutletTaskType : Entity
    {
        public const int MaxRemarkLength = 50;
        
        [Required]
        public int OutletId { get; set; }
        public virtual Outlet Outlet { get; set; }
        
        [Required]
        public int TaskTypeId { get; set; }
        public virtual TaskType TaskType { get; set; }

        /// <summary>
        /// 大队
        /// </summary>
        public int? DepotId { get; set; }
        public virtual Depot Depot { get; set; }

        [Required]
        public float Price { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(MaxRemarkLength)]
        public string Remark { get; set; }
    }
}

