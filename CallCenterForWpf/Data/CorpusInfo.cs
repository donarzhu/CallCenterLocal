using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenterForWpf.Data
{
    public class ResultCorpusInfo
    {
        public string message { get; set; } = "";
        public bool successful { get; set; } = false;
        public Corpus[] data { get; set; } = null;
    }

    public class Corpus
    {
        string corpus_id { get; set; }
        string file { get; set; }
        string flow_title { get; set; }
        string flow_type { get; set; }
        string id { get; set; }
        string input { get; set; }
        string output { get; set; }
        string topic { get; set; }
        string type { get; set; }
    }

    public class CmdGetCorpusData
    {
        public string token { get; set; }
        public string flow_id { get; set; }
    }

    public class CmdSaveEditCorpus
    {
        public string token { get; set; }
        public string sentence { get; set; }
        public string id { get; set; }
    }

    public class CmdUpdateVoiceFile
    {
        public string token { get; set; }
        public string output_type { get; set; } = "path";
        public string output_resource { get; set; }
    }
}
