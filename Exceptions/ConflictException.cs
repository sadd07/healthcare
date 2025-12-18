namespace Healthcare.Exceptions;

public class ConflictException : JsonException
{
    public ConflictException(string message = "Conflict.") : base(message, 10)
    {
    }
}
