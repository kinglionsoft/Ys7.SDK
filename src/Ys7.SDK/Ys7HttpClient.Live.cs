using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task<ApiResult<Ys7DeviceLiveAddress>> GetLiveAddressAsync(string deviceSerial,
            int channelNo = 1,
            int expireTime = 7200,
            bool https = true,
            CancellationToken cancellation = default)
        {
            var result = await PostAsync<ApiResult<Ys7DeviceLiveAddress>>("/api/lapp/live/address/limited",
                new[]
                {
                    new KeyValuePair<string, string>("deviceSerial", deviceSerial),
                    new KeyValuePair<string, string>("channelNo", channelNo.ToString()),
                    new KeyValuePair<string, string>("expireTime", expireTime.ToString()),
                }, cancellation);
            if (result.Success && https)
            {
                if (result.Data.HdAddress.StartsWith("http://"))
                {
                    result.Data.HdAddress = result.Data.HdAddress.Insert(4, "s");
                }

                if (result.Data.LiveAddress.StartsWith("http://"))
                {
                    result.Data.LiveAddress = result.Data.LiveAddress.Insert(4, "s");
                }
            }
            return result;
        }

        /// <summary>
        /// 添加设备到账号下。需先将设备连线。
        /// </summary>
        /// <param name="deviceSerial">设备序列号,存在英文字母的设备序列号，字母需为大写</param>
        /// <param name="validateCode">设备验证码，设备机身上的六位大写字母</param>
        /// <param name="deviceName">设备名称，长度不大于50字节，不能包含特殊字符</param>
        /// <param name="open">开通直播</param>
        /// <param name="encrypt">是否加密视频。直播时不能加密</param>
        /// <returns></returns>
        public async Task<ApiResult> AddDeviceAsync(string deviceSerial, 
            string validateCode,
            string deviceName = null, 
            bool open = true,
            bool encrypt = false,
            CancellationToken cancellation = default)
        {
            var result = await PostAsync<ApiResult>("/api/lapp/device/add",
                new[]
                {
                    new KeyValuePair<string, string>("deviceSerial", deviceSerial),
                    new KeyValuePair<string, string>("validateCode", validateCode),
                }, cancellation);
            if (result.Code == "20017")
            {
                // 设备已被自己添加
                result.Code = "200";
            }

            if (result.Success && !string.IsNullOrEmpty(deviceName))
            {
                result = await UpdateDeviceNameAsync(deviceSerial, deviceName, cancellation);
            }

            if (result.Success && open)
            {
                result = await OpenLiveAsync(new OpenLiveSource(deviceSerial), cancellation);
            }

            if (!encrypt)
            {
                result = await EncryptOffAsync(deviceSerial, validateCode, cancellation);
            }

            return result;
        }

        /// <summary>
        /// 删除账号下设备
        /// </summary>
        /// <param name="deviceSerial">设备序列号,存在英文字母的设备序列号，字母需为大写</param>
        /// <returns></returns>
        public async Task<ApiResult> DeleteDeviceAsync(string deviceSerial, CancellationToken cancellation = default)
        {
            var result = await PostAsync<ApiResult>("/api/lapp/device/delete",
                new[]
                {
                    new KeyValuePair<string, string>("deviceSerial", deviceSerial),
                }, cancellation);
            return result;
        }

        /// <summary>
        /// 修改设备名称
        /// </summary>
        /// <param name="deviceSerial">设备序列号,存在英文字母的设备序列号，字母需为大写</param>
        /// <param name="deviceName">设备名称，长度不大于50字节，不能包含特殊字符</param>
        /// <returns></returns>
        public async Task<ApiResult> UpdateDeviceNameAsync(string deviceSerial, string deviceName, CancellationToken cancellation = default)
        {
            var result = await PostAsync<ApiResult>("/api/lapp/device/name/update",
                new[]
                {
                    new KeyValuePair<string, string>("deviceSerial", deviceSerial),
                    new KeyValuePair<string, string>("deviceName", deviceName),
                }, cancellation);

            if (result.Code == "20002" /* 设备不存在 */
                || result.Code == "20018" /* 该用户不拥有该设备 */)
            {
                result.Code = "200";
            }

            return result;
        }

        /// <summary>
        /// 该接口用于根据序列号和通道号批量开通直播功能（只支持可观看视频的设备）。
        /// </summary>
        /// <param name="sources">直播源，[设备序列号]:[通道号],[设备序列号]:[通道号]的形式，例如427734222:1,423344555:3，均采用英文符号，限制50个</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<ApiResult<OpenLiveResult[]>> OpenLiveAsync(IReadOnlyList<OpenLiveSource> sources, CancellationToken cancellation = default)
        {
            if (sources == null) throw new ArgumentNullException(nameof(sources));
            if (sources.Count > 50) throw new ArgumentOutOfRangeException(nameof(sources), "直播源限制50个");

            var result = await PostAsync<ApiResult<OpenLiveResult[]>>("/api/lapp/live/video/open",
                new[]
                {
                    new KeyValuePair<string, string>("source", string.Join(",", sources.Select(x=> $"{x.DeviceSerial}:{x.ChannelNo}"))),
                }, cancellation);
            return result;
        }

        /// <summary>
        /// 该接口用于根据序列号和通道量开通直播功能
        /// </summary>
        public async Task<ApiResult> OpenLiveAsync(OpenLiveSource source, CancellationToken cancellation = default)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var result = await PostAsync<ApiResult<OpenLiveResult[]>>("/api/lapp/live/video/open",
                new[]
                {
                    new KeyValuePair<string, string>("source", $"{source.DeviceSerial}:{source.ChannelNo}")
                }, cancellation);

            if (!result.Success)
            {
                return result;
            }

            var liveRet = result.Data[0];

            if (!liveRet.Success)
            {
                if (liveRet.Ret == "60062") // 该通道直播已开通
                {
                    return result;
                }

                result.Code = liveRet.Ret;
                result.Msg = liveRet.Desc;
            }

            return result;
        }

        /// <summary>
        /// 关闭设备视频加密
        /// </summary>
        public async Task<ApiResult<OpenLiveResult[]>> EncryptOffAsync(string deviceSerial, string validateCode, CancellationToken cancellation = default)
        {
            // https://open.ys7.com/doc/zh/book/index/device_switch.html#device_switch-api2

            var result = await PostAsync<ApiResult<OpenLiveResult[]>>("/api/lapp/device/encrypt/off",
                new[]
                {
                    new KeyValuePair<string, string>("deviceSerial", deviceSerial),
                    new KeyValuePair<string, string>("validateCode", validateCode),
                }, cancellation);
            if (result.Code == "60016") result.Code = "200";
            return result;
        }

        /// <summary>
        /// 开启设备视频加密
        /// </summary>
        public async Task<ApiResult<OpenLiveResult[]>> EncryptOnAsync(string deviceSerial, CancellationToken cancellation = default)
        {
            // https://open.ys7.com/doc/zh/book/index/device_switch.html#device_switch-api2

            var result = await PostAsync<ApiResult<OpenLiveResult[]>>("/api/lapp/device/encrypt/off",
                new[]
                {
                    new KeyValuePair<string, string>("deviceSerial", deviceSerial),
                }, cancellation);
            if (result.Code == "60019") result.Code = "200";
            return result;
        }
    }
}