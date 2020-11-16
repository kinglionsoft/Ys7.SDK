using System.Text.Json.Serialization;

namespace Ys7.SDK.Models
{
    public class ApiResult
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonIgnore]
        public bool Success => Code == "200";
    }

    public class ApiResult<T>: ApiResult
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}