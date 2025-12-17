using Healthcare.Middlewares; 

namespace Microsoft.AspNetCore.Builder; 

public static class RateLimitExtensions
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RateLimitMiddleware>();
    }
}