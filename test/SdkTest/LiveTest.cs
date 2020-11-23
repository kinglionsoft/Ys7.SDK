using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Ys7.SDK.Models;

namespace SdkTest
{
    public class LiveTest: TestBase
    {
        public LiveTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task AddDevice()
        {
            var result = await Client
                .AddDeviceAsync(Options.DeviceSerial, Options.ValidateCode, "¿ª·¢²âÊÔ");
            WriteJson(result);
        }

        [Fact]
        public async Task OpenLive()
        {
            var result = await Client
                .OpenLiveAsync(new OpenLiveSource(Options.DeviceSerial));
            WriteJson(result);
        }

        [Fact]
        public async Task GetAddress()
        {
            var result = await Client.GetLiveAddressAsync(Options.DeviceSerial);
            WriteJson(result);
        }


        [Fact]
        public async Task Delete()
        {
            var result = await Client.DeleteDeviceAsync(Options.DeviceSerial);
            WriteJson(result);
        }
    }
}
