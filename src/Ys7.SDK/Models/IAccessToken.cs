namespace Ys7.SDK.Models
{
    public interface IAccessToken
    {
        string AccessToken { get; }

        int ExpiresInSeconds { get; }
    }
}