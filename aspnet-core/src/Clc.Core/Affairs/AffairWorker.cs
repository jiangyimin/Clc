using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.Affairs
{
    /// <summary>
    /// AffairWorker Entity
    /// </summary>
    [Description("内务工作人员")]
    public class AffairWorker : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int AffairId { get; set; }
        public Affair Affair { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }   

        [Required]
        public int WorkRoleId { get; set; }
        public WorkRole WorkRole { get; set; }  

        [NotMapped]
        public string WorkerCn { 
            get {
                if (Worker != null)
                    return Worker.Cn;
                else 
                    return null; //string.Empty;
            }
        }

    }
}

