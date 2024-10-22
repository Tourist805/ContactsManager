using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ContactsApi.Models;
using ContactsApi.Database;  // Ensure this namespace matches your project structure

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Register the DbContext with SQL Server configuration
builder.Services.AddDbContext<ContactsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();

// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Contacts API",
        Description = "A simple API to manage a list of contacts",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger in development mode
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contacts API v1");
    });
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable routing and endpoints
app.UseAuthorization();

app.MapControllers(); // Maps controller routes

// Run the application
app.Run();
