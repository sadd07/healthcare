namespace Healthcare.Exceptions;

public class NotFoundException : JsonException
{
    public NotFoundException(string message = "Data not found.") : base(message, 2)
    {
    }
}
