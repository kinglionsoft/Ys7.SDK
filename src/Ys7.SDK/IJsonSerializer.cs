using System;

namespace Ys7.SDK
{
    public interface IJsonSerializer
    {
        string Serialize(object data);

        T Deserialize<T>(string json);

        T Deserialize<T>(ReadOnlySpan<byte> data);
    }
}