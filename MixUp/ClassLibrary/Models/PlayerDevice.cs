using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ClassLibrary.Models
{
    [Serializable]
    public class PlayerDevice
    {
        [JsonProperty("id")]
        public string DeviceId { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("name")]
        public string DeviceName { get; set; }

        [JsonProperty("type")]
        public string DeviceType { get; set; }
    }
}