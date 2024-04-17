using System.Text.Json.Serialization;

using AggregateAndMicroService.Infrastructure;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

if (File.Exists("./config.json")) builder.Configuration.AddJsonFile("./config.json");
else throw new Exception("Файл конфигурации config.json отсутствует!");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));


builder.Services.AddDbContext<LearningContext>(options =>
{
    var connectionString = builder.Configuration["dbConnectionString"] ?? throw new Exception("Строка подключения отсутствует");
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/materials", async (LearningContext context) =>
{
    //var items = await context.Courses.Take(10).ToListAsync();

    //return Results.Ok(items);
});

app.MapPost("/materials", async (LearningContext context) =>
{

    await context.SaveChangesAsync();
})

.WithName("GetWeatherForecast")
.WithOpenApi();

SeedData.Seed(app);

app.Run();

