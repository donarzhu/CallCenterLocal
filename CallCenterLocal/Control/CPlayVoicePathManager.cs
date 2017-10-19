using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenterLocal.Control
{
    class CPlayVoicePathManager
    {
        string _path;
        public String PlayPath { get { return _path; } }
        
        public CPlayVoicePathManager(String workflowID)
        {
            _path = CallPhoneControl.playFilePath + workflowID + "\\";
        }

        public static string GetVoicePath(String workflowID)
        {
            return CallPhoneControl.playFilePath + workflowID + "\\";
        }
    }
}
