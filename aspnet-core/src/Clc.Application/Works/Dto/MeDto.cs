using System;
using System.Collections.Generic;
using Clc.Affairs;

namespace Clc.Works.Dto
{
    /// <summary>
    /// MyAffairWork Model
    /// </summary>
    public class MeDto
    {
        public string WorkerCn { get; set; }
        public int DepotId { get; set; }
        public string LoginRoleNames { get; set; }

        public MeDto() {}
        public MeDto(string roles, string cn, int id)
        {
            LoginRoleNames = roles;
            WorkerCn = cn;
            DepotId = id;
        }
    }
}

