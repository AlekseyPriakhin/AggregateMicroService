using System.Text.Json.Serialization;

using AggregateAndMicroService.Application.Routes;
using AggregateAndMicroService.Infrastructure;
using AggregateAndMicroService.Infrastructure.Kafka;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

if (File.Exists("./config.json")) builder.Configuration.AddJsonFile("./config.json");
else throw new Exception("Файл конфигурации config.json отсутствует!");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSingleton<KafkaService>();


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

app.MapGetRoutes()
    .MapPutRoutes()
    .MapPostRoutes();

app.Seed();
app.Run();

