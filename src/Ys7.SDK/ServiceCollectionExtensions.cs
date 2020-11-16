using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Ys7.SDK.Internal;

namespace Ys7.SDK
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddYs7Sdk(this IServiceCollection services, Action<Ys7SdkOptions> configure)
        {
            Debug.Assert(configure != null);

            var options = new Ys7SdkOptions();

            configure(options);

            services.Configure<Ys7SdkOptions>(opt => opt.CloneFrom(options));

            var httpClientBuilder = services.AddHttpClient<Ys7HttpClient>();

            if (options.DisableServerSslValidation)
            {
                httpClientBuilder.ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                });
            }

            if (options.RetryDurations?.Length > 0)
            {
                httpClientBuilder.AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(options.RetryDurations.Select(x => TimeSpan.FromMilliseconds(x))));
            }

            services.AddSingleton<IJsonSerializer, JsonSerializer>()
                .AddSingleton<ITokenManager, SimpleTokenManager>()
                ;

            return services;
        }
    }
}
