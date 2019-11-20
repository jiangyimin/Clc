using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Fields;
using Clc.Fields.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Fields)]
    public class VehiclesController : ClcCrudController<Vehicle, VehicleDto>
    {
        public VehiclesController(IRepository<Vehicle> repository)
            :base(repository)
        {
        }

        // public ActionResult GetPhoto(int id)
        // {
        //     Vehicle v = _repository.Get(id);
        //     if (v != null && v.Photo != null)
        //         return File(v.Photo, "image/jpg");

        //     return File(new byte[0], "image/jpg");
        // }
	}
}