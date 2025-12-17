using Microsoft.Extensions.Options;

namespace Healthcare.Middlewares;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RateLimitingOptions _options;
    private static readonly object _lock = new();
    private static readonly Dictionary<string, Queue<DateTime>> _requestTracker = new();

    public RateLimitMiddleware(
        RequestDelegate next,
        IOptions<RateLimitingOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (SkipRateLimiting(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var ipAddress = GetClientIpAddress(context);
        var endpoint = $"{context.Request.Method}:{context.Request.Path}";

        if (!IsRequestAllowed(ipAddress, endpoint))
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                code = 5,
                message = "Too many requests. Please try again later.",
            };
            
            await context.Response.WriteAsJsonAsync(response);
            return;
        }

        await _next(context);
    }

    private bool IsRequestAllowed(string ipAddress, string endpoint)
    {
        var key = $"{ipAddress}:{endpoint}";
        var now = DateTime.UtcNow;
        var timeWindow = TimeSpan.FromSeconds(_options.TimeWindowSeconds);

        lock (_lock)
        {
            if (!_requestTracker.ContainsKey(key))
            {
                _requestTracker[key] = new Queue<DateTime>();
            }

            var queue = _requestTracker[key];

            while (queue.Count > 0 && (now - queue.Peek()) > timeWindow)
            {
                queue.Dequeue();
            }

            if (queue.Count >= _options.MaxRequestsPerWindow)
            {
                return false;
            }

            queue.Enqueue(now);

            CleanupOldEntries();
            
            return true;
        }
    }

    private static void CleanupOldEntries()
    {
        // Cleanup memory leak
        var cutoff = DateTime.UtcNow.AddHours(-1);
        var keysToRemove = new List<string>();

        foreach (var kvp in _requestTracker)
        {
            var queue = kvp.Value;
            
            while (queue.Count > 0 && queue.Peek() < cutoff)
            {
                queue.Dequeue();
            }
            
            if (queue.Count == 0)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            _requestTracker.Remove(key);
        }
    }

    private string GetClientIpAddress(HttpContext context)
    {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].ToString();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    private bool SkipRateLimiting(PathString path)
    {
        var skipPaths = new string []
        {
            // Register path untuk skip rate limitter
            // "/health",
            // "/swagger",
            // "/favicon.ico",
            // "/robots.txt"
        };

        return skipPaths.Any(p => path.StartsWithSegments(p));
    }
}

public class RateLimitingOptions
{
    public int MaxRequestsPerWindow { get; set; } = 10;       // total requests allowed
    public int TimeWindowSeconds { get; set; } = 1;           // duration time
    public bool Enabled { get; set; } = true;
}
