namespace Ys7.SDK.Models
{
    internal class Ys7AccessToken: IAccessToken
    {
        public string AccessToken { get; set; }

        public int ExpiresInSeconds { get; set; }
    }
}