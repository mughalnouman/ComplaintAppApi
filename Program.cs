using ComplaintApi.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
//yeah database connection string a rhi ha jo k appsettings.json se a rhi ha 
// ✅ Setup your DB connection here BEFORE app.Build()
builder.Services.AddDbContext<ComplaintdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ComplaintDbConnection")));

// ✅ Build the app AFTER all services are added
var app = builder.Build();



// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


// Add this line to enable CORS
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
