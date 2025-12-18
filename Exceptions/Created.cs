namespace Healthcare.Exceptions;

public class Created : JsonException
{
    public Created(string message = "Created.") : base(message, 0)
    {
    }
}
