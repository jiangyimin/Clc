using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Clc.Web.Controllers
{
    // App01: 内务终端
    [IgnoreAntiforgeryToken]
    public class WorkApp01Controller : WorkAppControllerBase
    {
        public WorkApp01Controller(IHostingEnvironment env)
            :base(env, "App01")
        {
        }
    }

    [IgnoreAntiforgeryToken]
    public class WorkApp02Controller : WorkAppControllerBase
    {
        public WorkApp02Controller(IHostingEnvironment env)
            :base(env, "App02")
        {
        }
    }

    [IgnoreAntiforgeryToken]
    public class WorkApp03Controller : WorkAppControllerBase
    {
        public WorkApp03Controller(IHostingEnvironment env)
            :base(env, "App03")
        {
        }
    }

    [IgnoreAntiforgeryToken]
    public class WorkApp04Controller : WorkAppControllerBase
    {
        public WorkApp04Controller(IHostingEnvironment env)
            :base(env, "App04")
        {
        }
    }

}