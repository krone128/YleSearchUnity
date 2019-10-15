using System;
using System.Collections.Generic;

namespace YleSearch
{
    [Serializable]
    public class YleProgramData
    {
        public Dictionary<string,string> title { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string typeMedia { get; set; }
        public string typeCreative { get; set; }
        public string duration { get; set; }
    }
}