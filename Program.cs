using Healthcare;
using Healthcare.Exceptions;
using Healthcare.Interfaces.Repositories;
using Healthcare.Interfaces.Services;
using Healthcare.Repositories;
using Healthcare.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Dependency Injection for Repositories
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
    
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.First().ErrorMessage
                );
        
        var response = new
        {
            code = 1,
            message = errors.FirstOrDefault().Value ?? "Bad request.",
        };

        return new BadRequestObjectResult(response)
        {
            ContentTypes = { "application/json" }
        };
    };
});

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
            Created => (201, 0),
            BadRequestException => (400, 1),
            NotFoundException => (404, 2),
            ConflictException => (409, 10),
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

static string? ToCamelCase(string? key)
{
    if (string.IsNullOrEmpty(key)) return key;
    
    // Handle nested properties (e.g., "request.Name" -> "request.name")
    if (key.Contains('.'))
    {
        var parts = key.Split('.');
        return string.Join(".", parts.Select(p => 
            char.ToLowerInvariant(p[0]) + p.Substring(1)));
    }
    
    return char.ToLowerInvariant(key[0]) + key.Substring(1);
}