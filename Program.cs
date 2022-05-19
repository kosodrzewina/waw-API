using Microsoft.EntityFrameworkCore;
using WawAPI.Models;
using WawAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = configuration.GetConnectionString("WawDb");

builder.Services.AddDbContext<MainDbContext>(
    options =>
    {
        options.UseSqlServer(connectionString);
    }
);
builder.Services.AddHostedService<EventService>();
builder.Services.AddScoped<IDatabaseService, MainDbService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
