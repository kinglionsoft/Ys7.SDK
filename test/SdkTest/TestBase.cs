using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;
using Ys7.SDK;

namespace SdkTest
{
    public class TestBase
    {
        protected readonly ITestOutputHelper Output;
        protected readonly IJsonSerializer JsonSerializer;
        protected readonly Ys7HttpClient Client;
        protected readonly TestOptions Options;

        public TestBase(ITestOutputHelper output)
        {
            Output = output;
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();

            var sp = new ServiceCollection()
                .AddYs7Sdk(configuration.GetSection("SDK").Bind)
                .Configure<TestOptions>(configuration.GetSection("Test").Bind)
                .BuildServiceProvider();

            Client = sp.GetRequiredService<Ys7HttpClient>();
            JsonSerializer = sp.GetRequiredService<IJsonSerializer>();
            Options = sp.GetService<IOptions<TestOptions>>().Value;
        }

        protected void WriteJson(object data) => Output.WriteLine(JsonSerializer.Serialize(data));
    }
}