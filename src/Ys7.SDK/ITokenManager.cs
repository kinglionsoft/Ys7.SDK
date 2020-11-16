using System;
using System.Threading.Tasks;
using Ys7.SDK.Models;

namespace Ys7.SDK
{
    public interface ITokenManager
    {
        /// <summary>
        /// Cache the requested token. Refresh the token if expired.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> GetTokenAsync(Func<Task<IAccessToken>> request);
    }
}