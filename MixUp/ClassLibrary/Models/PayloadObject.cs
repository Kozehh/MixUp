using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class PayloadObject
    {
        [JsonProperty("devices")] 
        public List<PlayerDevice> Devices { get; set; }
    }
}
