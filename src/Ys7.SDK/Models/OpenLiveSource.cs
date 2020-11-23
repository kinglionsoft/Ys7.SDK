namespace Ys7.SDK.Models
{
    public class OpenLiveSource
    {
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DeviceSerial { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        public int ChannelNo { get; set; } = 1;

        public OpenLiveSource()
        {
            
        }

        public OpenLiveSource(string deviceSerial, int channelNo = 1)
        {
            DeviceSerial = deviceSerial;
            ChannelNo = channelNo;
        }
    }
}