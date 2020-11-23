using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ys7.SDK.Models;

namespace Ys7.SDK
{
    public partial class Ys7HttpClient
    {
        protected readonly HttpClient HttpClient;
        protected readonly Ys7SdkOptions Options;
        protected readonly ILogger Logger;
        protected readonly IJsonSerializer JsonSerializer;
        protected readonly ITokenManager TokenManager;

        public Ys7HttpClient(HttpClient httpClient,
            IOptions<Ys7SdkOptions> options,
            ILogger<Ys7HttpClient> logger,
            IJsonSerializer jsonSerializer,
            ITokenManager tokenManager)
        {
            HttpClient = httpClient;
            Options = options.Value;
            Logger = logger;
            JsonSerializer = jsonSerializer;
            TokenManager = tokenManager;
            HttpClient.BaseAddress = new Uri("https://open.ys7.com");
            HttpClient.Timeout = TimeSpan.FromMilliseconds(Options.Timeout);
        }

        #region Helpers

        protected virtual async Task<T> PostAsync<T>(string url, IEnumerable<KeyValuePair<string, string>> parameters,
            CancellationToken cancellation)
            where T : ApiResult
        {
            using var content = await PrepareContentAsync(parameters);
            using var response = await HttpClient.PostAsync(url, content, cancellation);
            var responseData = await response.Content.ReadAsStringAsync();
            var apiResult = JsonSerializer.Deserialize<T>(responseData);
            return apiResult;
        }

        #endregion

        #region AccessToken

        protected virtual async Task<FormUrlEncodedContent> PrepareContentAsync(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var token = await TokenManager.GetTokenAsync(GetTokenAsync);
            var collection = parameters.Concat(new[]
            {
                new KeyValuePair<string, string>("accessToken", token)
            });
            var content = new FormUrlEncodedContent(collection);
            return content;
        }

        protected virtual async Task<IAccessToken> GetTokenAsync()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("appKey", Options.AppKey),
                new KeyValuePair<string, string>("appSecret", Options.Secret),
            });

            var response = await HttpClient.PostAsync("/api/lapp/token/get", content);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResult<Ys7AccessToken>>(body);
            if (result.Success)
            {
                return result.Data;
            }
            throw new Ys7Exception($"Request token failed: {body}, refer to https://open.ys7.com/doc/zh/book/index/user.html");
        }

        #endregion
    }
}