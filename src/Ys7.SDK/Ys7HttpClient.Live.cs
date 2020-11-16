using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ys7.SDK.Models;

namespace Ys7.SDK
{
    public partial class Ys7HttpClient
    {
        /// <summary>
        /// 获取指定设备的直播地址
        /// </summary>
        /// <param name="deviceSerial">设备序列号,存在英文字母的设备序列号，字母需为大写</param>
        /// <param name="channelNo">通道号，IPC设备填1</param>
        /// <param name="expireTime">地址过期时间：单位秒数，最大默认62208000（即720天），最小默认300（即5分钟）。非必选参数，为空时返回对应设备和通道的永久地址</param>
        /// <returns></returns>
        public async Task<Ys7DeviceLiveAddress> GetLiveAddressAsync(string deviceSerial,
            int channelNo = 1,
            int expireTime = 7200,
            CancellationToken cancellation = default)
        {
            var result = await PostAsync<ApiResult<Ys7DeviceLiveAddress>>("/api/lapp/live/address/limited",
                new[]
                {
                    new KeyValuePair<string, string>("deviceSerial", deviceSerial),
                    new KeyValuePair<string, string>("channelNo", channelNo.ToString()),
                    new KeyValuePair<string, string>("expireTime", expireTime.ToString()),
                }, cancellation);
            return result.Data;
        }
    }
}