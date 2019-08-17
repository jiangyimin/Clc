using System.Collections.Generic;

namespace Clc.Works.Dto
{
    public class RouteWorkerMatchResult {
        public string Message { get; set; }
        public RouteMatchedDto RouteMatched { get; set; }
        public WorkerMatchedDto WorkerMatched { get; set; }

        List<RouteArticleCDto> Articles { get; set; }
    }
}
