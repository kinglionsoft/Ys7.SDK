using System;
using System.Threading.Tasks;
using Ys7.SDK.Models;

namespace Ys7.SDK.Internal
{
    internal class SimpleTokenManager : ITokenManager
    {
        private DateTime _expireAt;

        private string _token;

        public async Task<string> GetTokenAsync(Func<Task<IAccessToken>> request)
        {
            /*
             * Only for single instance. Otherwise a distribute cache (e.g. Redis) is recommended.
             */

            if (IsExpired())
            {
                var accessToken = await request();
                _token = accessToken.AccessToken;
                _expireAt = DateTime.Now.AddSeconds(accessToken.ExpiresInSeconds - 200);
            }

            return _token;
        }

        private bool IsExpired()
        {
            return string.IsNullOrEmpty(_token)
                   || DateTime.Now >= _expireAt;
        }
    }
}