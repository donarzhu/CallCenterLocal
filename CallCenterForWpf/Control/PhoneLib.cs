using CallCenterLocal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CallCenterLocal.Control
{

    public class CallPhoneControl
    {
        private static phoneDll.PHONE phone = new phoneDll.PHONE();
        static string AppPath = System.Windows.Forms.Application.StartupPath;
        public static string  playFilePath { get; } = AppPath + "\\playvoice\\";
        public static string recFullPath { get; } = AppPath + "\\full\\";
        public static string recSinglePath { get; } = AppPath + "\\full\\";
        public static string recordPath { get; } = AppPath + "\\record\\";
        public static bool initVad = false;
        public void  openDevInit()
        {
            try
            {
                phone.openDevInit();
                initVad = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void closeDev()
        {
            if (!initVad)
                return;
            try
            {
                phone.closeDev();
            }
            catch(Exception e)
            {
            }
        }

        public Int32 startDialPstn(DialPhoneInfo[] dialData, string token)
        {
            if (dialData==null||dialData.Length <= 0)
                return -1;
            phoneDll.PHONE.tag_dial_Data[] m_tagdialData1 = new phoneDll.PHONE.tag_dial_Data[dialData.Length];
            for(int i = 0;i<dialData.Length;i++)
            {
                m_tagdialData1[i].channalUuid = dialData[i].channal_uuid;
                m_tagdialData1[i].dialNumber = dialData[i].cust_number;
                m_tagdialData1[i].flowID = dialData[i].flow_id;
                m_tagdialData1[i].ID = dialData[i].id;
                m_tagdialData1[i].playFilePath = CPlayVoicePathManager.GetVoicePath(m_tagdialData1[0].flowID);
            }
            if (!initVad)
                return -1;
            try
            {
                return phone.startDialPstn(m_tagdialData1, token,  recFullPath, recSinglePath);
            }
            catch(Exception e)
            {
                MessageBox.Show("VAD模块调用失败！");
                return -1;
            }
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
        // public const string PHONEDLLNAME = "phoneDll.dll";

        // 打开设备@"C:\MSDEV\Projects\openwind\Debug\openwind.dll", EntryPoint = "OpenWind_Sub", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        //[DllImport("phoneDll.dll", EntryPoint = "openDevInit")]

        public static extern void openDevInit();

        // 关闭设备
        //[DllImport("phoneDll.dll")]
        public static extern void closeDev();

        // dial 
        //[DllImport("phoneDll.dll")]
        public static extern Int32 startDialPstn(DialPhoneInfo[] dialData, string token, string sPlayFilePath, string sRcFullPath, string sRecSinglePath);

    }

}
