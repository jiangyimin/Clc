using System.Runtime.InteropServices;

namespace Clc.Runtime
{
    public class FingerDll
    {
        [DllImport("F:\\Tbs\\src\\Tbs.Web.Mvc\\bin\\Debug\\netcoreapp1.1\\ARTH_DLL32.dll")]
        public static extern int Match2Fp(byte[] src, byte[] dst);

        //[DllImport("C:\\inetpub\\TbsWeb\\ARTH_DLL.dll")]
        [DllImport("E:\\Projects\\Clc\\aspnet-core\\src\\Clc.Web.Mvc\\bin\\Debug\\netcoreapp2.2\\ARTH_DLL.dll")]
        public static extern int UserMatch(byte[] src, byte[] dst, byte secuLevel, ref int matchScore);
    }
}