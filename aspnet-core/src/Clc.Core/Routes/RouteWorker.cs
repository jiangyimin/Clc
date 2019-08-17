using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Clc.Fields;
using Clc.Types;

namespace Clc.Routes
{
    /// <summary>
    /// RouteWorker Entity
    /// </summary>
    [Description("线路工作人员")]
    public class RouteWorker : Entity, IMustHaveTenant
    {
        // Impement of IMustHaveTenant
        public int TenantId { get; set; }
        /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }
        public Route Route { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }   

        [Required]
        public int WorkRoleId { get; set; }
        public WorkRole WorkRole { get; set; }  

        [ForeignKey("RouteWorkerId")]
        public virtual List<RouteArticle> RouteArticles { get; set; }
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

