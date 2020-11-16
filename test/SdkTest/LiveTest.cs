using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SdkTest
{
    public class LiveTest: TestBase
    {
        public LiveTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetAddress()
        {
            var address = await Client.GetLiveAddressAsync("203751922");
            WriteJson(address);
        }
    }
}
