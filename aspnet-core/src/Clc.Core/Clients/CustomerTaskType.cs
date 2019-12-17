using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.Clients
{
    /// <summary>
    /// CustomerTaskType Entity
    /// </summary>
    [Description("押运客户的任务类型")]
    public class CustomerTaskType : Entity
    {
        public const int MaxRemarkLength = 50;
        
        [Required]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
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

