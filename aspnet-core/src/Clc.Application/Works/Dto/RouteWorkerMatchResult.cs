using System.Collections.Generic;

namespace Clc.Works.Dto
{
    public class RouteWorkerMatchResult {
        public string Message { get; set; }
        public MatchedRouteDto RouteMatched { get; set; }
        public MatchedWorkerDto WorkerMatched { get; set; }

        public MatchedWorkerDto WorkerMatched2 { get; set; }

        public List<RouteArticleCDto> Articles { get; set; }
        public List<RouteArticleCDto> Articles2 { get; set; }
        public List<RouteBoxCDto> Boxes { get; set; }
    }
}
