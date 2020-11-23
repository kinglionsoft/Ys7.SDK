namespace Ys7.SDK.Models
{
    public sealed class Ys7DeviceLiveAddress
    {
        /// <summary>
        /// 
        /// </summary>
        public string DeviceSerial { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ChannelNo { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// HLS流畅直播地址
        /// </summary>
        public string LiveAddress { get; set; }
        /// <summary>
        /// HLS高清直播地址
        /// </summary>
        public string HdAddress { get; set; }
        /// <summary>
        /// RTMP流畅直播地址
        /// </summary>
        public string Rtmp { get; set; }
        /// <summary>
        /// RTMP高清直播地址
        /// </summary>
        public string RtmpHd { get; set; }
        /// <summary>
        /// 地址使用状态：0-未使用或直播已关闭，1-使用中，2-已过期，3-直播已暂停，0状态不返回地址，其他返回
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 地址异常状态：0-正常，1-设备不在线，2-设备开启视频加密，3-设备删除，4-失效，5-未绑定，6-账户下流量已超出，7-设备接入限制，0/1/2/6状态返回地址，其他不返回
        /// </summary>
        public int Exception { get; set; }
        /// <summary>
        /// 开始时间，long格式如1472694964067，精确到毫秒。expireTime参数为空时该字段无效
        /// </summary>
        public long BeginTime { get; set; }
        /// <summary>
        /// 过期时间，long格式如1472794964067，精确到毫秒。expireTime参数为空时该字段无效
        /// </summary>
        public long EndTime { get; set; }
    }
}