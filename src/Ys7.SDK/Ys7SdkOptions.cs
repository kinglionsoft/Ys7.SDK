namespace Ys7.SDK
{
    public sealed class Ys7SdkOptions
    {
        public string AppKey { get; set; }

        public string Secret { get; set; }
        
        /// <summary>
        /// Request timeout, in ms.
        /// </summary>
        public int Timeout { get; set; } = 10_000;

        /// <summary>
        /// Retry durations in ms, <see cref="Microsoft.Extensions.Http.Polly"/>
        /// </summary>
        public int[] RetryDurations { get; set; } = { 500, 1000, 2000 };

        public bool DisableServerSslValidation { get; set; }

        public void CloneFrom(Ys7SdkOptions options)
        {
            AppKey = options.AppKey;
            Secret = options.Secret;
            Timeout = options.Timeout;
            RetryDurations = options.RetryDurations;
            DisableServerSslValidation = options.DisableServerSslValidation;
        }
    }
}