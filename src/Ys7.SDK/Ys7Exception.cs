using System;
using System.Runtime.Serialization;

namespace Ys7.SDK
{
    [Serializable]
    public class Ys7Exception : Exception
    {
        public Ys7Exception()
        {
        }

        public Ys7Exception(string message) : base(message)
        {
        }

        public Ys7Exception(string message, Exception inner) : base(message, inner)
        {
        }

        protected Ys7Exception(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}