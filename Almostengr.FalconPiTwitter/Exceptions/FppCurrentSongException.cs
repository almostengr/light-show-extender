using System;
using System.Runtime.Serialization;

namespace Almostengr.FalconPiTwitter.Exceptions
{
    [Serializable]
    internal class FppCurrentSongException : Exception
    {
        public FppCurrentSongException()
        {
        }

        public FppCurrentSongException(string message) : base(message)
        {
        }

        public FppCurrentSongException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FppCurrentSongException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}