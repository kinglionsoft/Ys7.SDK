using System.Text.Json.Serialization;

namespace Ys7.SDK.Models
{
    public class OpenLiveResult
    {
        public string DeviceSerial { get; set; }
        public int ChannelNo { get; set; }
        public string Ret { get; set; }
        public string Desc { get; set; }

        [JsonIgnore]
        public bool Success => Ret == "200";
    }
}