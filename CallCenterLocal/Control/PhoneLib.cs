using CallCenterLocal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CallCenterLocal.Control
{

    public class CallPhoneControl
    {
        static string AppPath = System.Windows.Forms.Application.StartupPath;
        public string playFilePath { get; set; } = AppPath + "\\playvioce";
        public string recFullPath { get; set; } = AppPath + "\\full";
        public string recSinglePath { get; set; } = AppPath + "\\single";

        public void  openDevInit()
        {
            PhoneLib.openDevInit();
        }

        public void closeDev()
        {
            PhoneLib.closeDev();
        }

        public Int32 startDialPstn(DialPhoneInfo[] dialData, string token)
        {
            return PhoneLib.startDialPstn(dialData, token, this.playFilePath, this.recFullPath, this.recSinglePath);
        }
    }

    class PhoneLib
    {
        //数据结构
        //public struct tag_dial_Data
        //{
        //    public string channalUuid;
        //    public string flowID;
        //    public string dialNumber;
        //    public int ID;

        //}
        public const string PHONEDLLNAME = "phoneDll.dll";

        // 打开设备@"C:\MSDEV\Projects\openwind\Debug\openwind.dll", EntryPoint = "OpenWind_Sub", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        [DllImport("phoneDll.dll", EntryPoint = "openDevInit")]

        public static extern void openDevInit();

        // 关闭设备
        [DllImport("phoneDll.dll")]
        public static extern void closeDev();

        // dial 
        [DllImport("phoneDll.dll")]
        public static extern Int32 startDialPstn(DialPhoneInfo[] dialData, string token, string sPlayFilePath, string sRcFullPath, string sRecSinglePath);

    }
}
}
