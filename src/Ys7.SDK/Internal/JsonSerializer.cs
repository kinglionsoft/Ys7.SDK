using System;
using System.Text.Json;

namespace Ys7.SDK.Internal
{
    internal class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public string Serialize(object data)
        {
            return System.Text.Json.JsonSerializer.Serialize(data, _options);
        }

        public T Deserialize<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, _options);
        }

        public T Deserialize<T>(ReadOnlySpan<byte> data)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(data, _options);
        }
    }
}