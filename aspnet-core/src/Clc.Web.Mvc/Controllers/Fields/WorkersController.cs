using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Fields;
using Clc.Fields.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Fields)]
    public class WorkersController : ClcCrudController<Worker, WorkerDto>
    {
        public WorkersController(IRepository<Worker> repository)
            :base(repository)
        {
        }

        public ActionResult GetPhoto(int id)
        {
            Worker w = _repository.Get(id);
            if (w != null && w.Photo != null)
                return File(w.Photo, "image/jpg");

            return File(new byte[0], "image/jpg");
        }
	}
}