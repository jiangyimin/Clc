using Abp.Application.Services;
using Clc.Weixin.Dto;

namespace Clc.Weixin
{
    public interface IWwApp01AppService : IApplicationService
    {
        void InsertDoorEmerg(int depotId, int workerId, int doorId, string content);
 
    }
}
