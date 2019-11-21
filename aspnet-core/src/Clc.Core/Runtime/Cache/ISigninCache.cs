namespace Clc.Runtime.Cache
{
    public interface ISigninCache
    {
        Signin Get(int depotId, int workerId, bool isMorning);
    }
}