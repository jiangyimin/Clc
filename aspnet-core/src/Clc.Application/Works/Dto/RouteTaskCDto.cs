using Clc.Routes;

namespace Clc.Works.Dto
{
    /// <summary>
    /// RouteTaskCDto
    /// </summary>
    public class RouteTaskCDto
    {
        public int Id { get; set;}
        public int OutletId { get; set; }

        public int TaskTypeId { get; set; }

        
        public RouteTaskCDto(RouteTask t)
        {
            Id = t.Id;
            OutletId = t.OutletId;
            TaskTypeId = t.TaskTypeId;
        }
    }
}

