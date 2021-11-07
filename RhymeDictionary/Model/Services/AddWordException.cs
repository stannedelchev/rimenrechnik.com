using System;

namespace RhymeDictionary.Model.Services
{
    internal class AddWordException : Exception
    {
        public AddWordException()
        {
        }

        public AddWordException(string message)
            : base(message)
        {
        }

        public AddWordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AddWordException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
