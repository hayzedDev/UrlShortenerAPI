using UrlShortenerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register URL Shortener Service
builder.Services.AddSingleton<UrlShortenerService>();

// Add Swagger for API documentation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "URL Shortener API",
        Version = "v1",
        Description = "A fast and efficient URL shortening service with click tracking and analytics",
        Contact = new()
        {
            Name = "Hayzed Dev",
            Email = "hayzeddev@example.com"
        }
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API V1");
    options.RoutePrefix = string.Empty; // Makes Swagger UI the default page
});

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Welcome endpoint
app.MapGet("/api", () => new
{
    message = "Welcome to URL Shortener API",
    version = "1.0",
    documentation = "/swagger",
    endpoints = new
    {
        shorten = "POST /api/urls",
        redirect = "GET /{shortCode}",
        stats = "GET /api/urls/{shortCode}/stats"
    },
    features = new[]
    {
        "Shorten URLs",
        "Custom short codes",
        "Click tracking",
        "Analytics",
        "URL expiration"
    }
}).WithTags("Info");

app.Run();
