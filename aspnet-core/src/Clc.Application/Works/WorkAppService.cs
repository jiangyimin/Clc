using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Fields;
using Clc.Runtime;
using Clc.Works.Dto;

namespace Clc.Works
{
    [AbpAuthorize]
    public class WorkAppService : ClcAppServiceBase, IWorkAppService
    {
        public WorkManager WorkManager { get; set; }

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<Signin> _signinRepository;
        
        public WorkAppService(IRepository<Signin> signinRepository)
        {
            _signinRepository = signinRepository;
        }

        public bool VerifyUnlockPassword(string password)
        {
            var depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var unlockPassword = WorkManager.GetUnlockPassword(depotId); // Get DepotId and WorkCn
            return unlockPassword == password;
        }

        public string GetTodayString()
        {
            return DateTime.Now.Date.ToString("yyyy-MM-dd");
        }
        public DateTime getNow()
        {
            return DateTime.Now;
        }

        public MyWorkDto GetMyWork()
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result; 
            int depotId = WorkManager.GetWorkerDepotId(workerId);         
            var dto = new MyWorkDto();
            dto.WorkerCn = WorkManager.GetWorkerCn(workerId);

            var aw = WorkManager.GetValidAffairWorker(depotId, workerId);
            if (aw != null)
            {
                dto.AffairId = aw.Affair.Id;
                dto.Status = aw.Affair.Status;
                dto.StartTime = ClcUtils.GetNowDateTime(aw.Affair.StartTime);
                dto.EndTime = ClcUtils.GetNowDateTime(aw.Affair.EndTime, aw.Affair.IsTomorrow);
                dto.Workers = WorkManager.GetWorkersFromAffairId(aw.Affair.Id);
            }
            return dto;
        }

        #region signin

        public async Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate)
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            var query = _signinRepository.GetAllIncluding(x => x.Worker);
            query = query.Where(x => x.DepotId == depotId && x.CarryoutDate == carryoutDate);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            entities.Sort((a, b) => a.Worker.Cn.CompareTo(b.Worker.Cn));
            return ObjectMapper.Map<List<SigninDto>>(entities);
        }

        public string SigninByRfid(string rfid) 
        {
            int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
            Worker worker = WorkManager.GetWorkerByRfid(rfid);

            if (worker == null) return "找不到此人";
            if ( (!worker.LoanDepotId.HasValue && worker.DepotId != depotId) 
                || (worker.LoanDepotId.HasValue) && worker.LoanDepotId != depotId ) return "此人不应在此运作中心签到";
            
            return DoSignin(depotId, worker.Id);
        }

        public string SigninByWx(string cn, float lat, float lon) 
        {
            Worker worker = WorkManager.GetWorkerByCn(cn);
            if (worker == null) return "无此工号";
            // judge distance

            int depotId = worker.LoanDepotId.HasValue ? worker.LoanDepotId.Value : worker.DepotId;
            if (!WorkManager.IsInDepotRadius(depotId, lat, lon)) return "请到中心后再签到";

            return DoSignin(depotId, worker.Id);
        }

        private string DoSignin(int depotId, int workerId)
        {
            // Get Signin 
            var signin = _signinRepository.FirstOrDefault(s => s.DepotId == depotId && s.CarryoutDate == DateTime.Today && s.WorkerId == workerId);
            if (signin == null)
            {
                Signin s = new Signin() { DepotId = depotId, CarryoutDate = DateTime.Today, WorkerId = workerId, SigninTime = DateTime.Now }; 
                _signinRepository.Insert(s);
                return "签到成功";
            }
            else
            {
                return "你已签过到";
            }
        }
        #endregion
    }
}