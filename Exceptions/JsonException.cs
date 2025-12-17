using System.Text.Json;

namespace Healthcare.Exceptions
{
    public class JsonException : Exception
    {
        public int StatusCode { get; }
        public int Code { get; }

        public JsonException(string message, int code = 99)
            : base(message)
        {
            Code = code;
        }

        public string ToJson()
        {
            var response = new
            {
                code = Code,
                message = Message,
            };

            return JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });
        }

        // Method untuk write langsung ke HttpResponse
        public async Task WriteToResponseAsync(HttpResponse response)
        {
            var statusCode = Code switch
            {
                01 => 400, 
                _ => 500,
            };
            
            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            
            var json = ToJson();
            await response.WriteAsync(json);
        }
    }
}