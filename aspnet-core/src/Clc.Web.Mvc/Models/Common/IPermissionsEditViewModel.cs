using System.Collections.Generic;
using Clc.Roles.Dto;

namespace Clc.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}