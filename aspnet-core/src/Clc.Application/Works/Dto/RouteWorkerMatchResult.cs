using System.Collections.Generic;

namespace Clc.Works.Dto
{
    public class RouteWorkerMatchResult {
        public string Message { get; set; }
        public RouteMatchedDto RouteMatched { get; set; }
        public WorkerMatchedDto WorkerMatched { get; set; }

        public WorkerMatchedDto WorkerMatched2 { get; set; }

        public List<RouteArticleCDto> Articles { get; set; }
        public List<RouteArticleCDto> Articles2 { get; set; }
        public List<RouteBoxCDto> Boxes { get; set; }
    }
}
