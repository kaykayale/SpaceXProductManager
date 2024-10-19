using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services for EF Core 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("Data Source=localhost\\SpaceSurfer;Initial Catalog=efcoreapp;User Id=sa;Password=kalynn;TrustServerCertificate=True;"); // For SQL Server
});


// builder.Services.AddScoped<ProductService>();  // Register ProductService for DI

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://127.0.0.1:3000", "http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


// Build the application
var app = builder.Build();

// Apply the CORS policy before mapping endpoints
app.UseCors("AllowFrontend");


app.MapGet("/", () => "Welcome to the SpaceX Product Manager App!");


app.Run();