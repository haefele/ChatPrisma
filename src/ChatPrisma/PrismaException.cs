using System.Runtime.Serialization;

namespace ChatPrisma;

public class PrismaException : Exception
{
    public PrismaException()
    {
    }

    public PrismaException(string message) : base(message)
    {
    }

    public PrismaException(string message, Exception innerException) : base(message, innerException)
    {
    }

#pragma warning disable SYSLIB0051
    protected PrismaException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
#pragma warning restore SYSLIB0051
    {
    }
}