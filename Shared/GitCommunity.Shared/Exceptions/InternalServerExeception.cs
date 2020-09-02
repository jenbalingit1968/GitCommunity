using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GitCommunity.Shared.Exceptions
{
    public class InternalServerExeception : Exception
    {
        public InternalServerExeception()
        {
        }

        public InternalServerExeception(string message) : base(message)
        {
        }

        public InternalServerExeception(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InternalServerExeception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
