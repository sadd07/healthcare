namespace Healthcare.Exceptions;

public class BadRequestException : JsonException
{
    public BadRequestException(string message = "Bad Request.") : base(message, 1)
    {
    }
}
