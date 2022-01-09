using WawAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var eventFetcher = new EventFetcher("https://waw4free.pl/rss-dzisiaj");
var events = await eventFetcher.Fetch();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
