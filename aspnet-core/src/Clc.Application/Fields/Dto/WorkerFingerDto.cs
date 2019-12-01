using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Worker))]
    public class WorkerFingerDto : EntityDto
    {       
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [StringLength(Worker.MaxCnLength)]
        public string Cn { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(Worker.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 可担任角色列表
        /// </summary>
        [StringLength(Worker.MaxRoleNamesLength)]
        public string WorkRoleNames { get; set; }

        public string Finger { get; set; }

        public string Finger2 { get; set; }
        
    }
}