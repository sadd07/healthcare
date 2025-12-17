using Healthcare;
using Healthcare.Exceptions;
using Healthcare.Interfaces.Repositories;
using Healthcare.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Database connection string is not configured.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: builder.Configuration.GetValue<int>("Database:MaxRetryCount", 3)
        );
        
        // Command timeout
        sqlServerOptions.CommandTimeout(builder.Configuration.GetValue<int>("Database:CommandTimeout", 30));
    })
);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Dependency Injection for Services

// Dependency Injection for Repositories
builder.Services.AddSingleton<IDoctorRepository, DoctorRepository>();

var app = builder.Build();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var path = context.Request.Path.Value ?? "";
        if (path.StartsWith("/swagger") || path.StartsWith("/openapi"))
        {
            return;
        }

        context.Response.ContentType = "application/json";
        
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        
        var (statusCode, code) = exception switch
        {
            NotFoundException => (404, 2),
            UnauthorizedAccessException => (401, 4),
            _ => (500, 99)
        };

        
        context.Response.StatusCode = statusCode;
        
        await context.Response.WriteAsJsonAsync(new
        {
            code,
            message = exception.Message ?? "Internal server error.",
        });
    });
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRateLimiting();

app.MapControllers();

app.Run();