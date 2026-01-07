using System.Runtime.Serialization;

namespace Fcg.User.Service.Domain.Exceptions;
    
[Serializable]
public abstract class CustomExceptionBase : Exception
{
    protected CustomExceptionBase() { }

    protected CustomExceptionBase(string message) : base(message) { }

    protected CustomExceptionBase(string message, Exception innerException)
        : base(message, innerException) { }

    protected CustomExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context) { }
}