using System.Text.Json.Serialization;

using AggregateAndMicroService.Application.Mappers;
using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Mvc;
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

app.MapGet("/courses", async ([FromServices] LearningContext context) =>
{
    return Results.Ok(await context.Courses.Take(10).Select(e => CourseMapper.ToOutCourseDto(e)).ToListAsync());
});

app.MapGet("/courses/{courseId}", async ([FromServices] LearningContext context, [FromServices] IMediator _mediator, [FromRoute] string courseId) =>
{
    var course = await context.Courses.FindAsync(CourseId.Of(Guid.Parse(courseId)));
    if (course is null) return Results.NotFound($"Курса с таким Id - {courseId} не существует");

    var courseStages = await context.Stages.Where(e => e.CourseId == course.Id.Value).ToListAsync();

    course.UpdateStatus(CourseStatus.Of(Statuses.Draft), courseStages);

    await context.SaveEntitiesAsync();

    return Results.Ok(CourseMapper.ToOutCourseDto(course));
});

app.MapPost("/materials", async (LearningContext context) =>
{

    await context.SaveChangesAsync();
})

.WithName("GetWeatherForecast")
.WithOpenApi();

SeedData.Seed(app);

app.Run();

