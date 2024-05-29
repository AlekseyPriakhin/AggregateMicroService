
using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Domain.User;

namespace AggregateAndMicroService.Infrastructure;
public static class SeedData
{
    public static WebApplication Seed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<LearningContext>() ?? throw new Exception("Не удалось получить контекст бд");
        context.Database.EnsureCreated();

        if (!context.Courses.Any())
        {
            InitUsers(context);
            for (int i = 1; i <= 5; i++)
            {
                var courseId = Guid.NewGuid();
                InitStages(context, courseId);
                var stages = context.ChangeTracker.Entries<Stage>()
                                                .Where(e => e.Entity.CourseId == courseId)
                                                .Select(e => e.Entity)
                                                .ToList();
                var courseDto = new CourseDTO($"Course {i}", "Description", courseId);
                var course = Course.Create(courseDto, stages);
                context.Courses.Add(course);
            }

            context.SaveChanges();
        }

        return app;
    }

    private static void InitUsers(LearningContext context)
    {
        List<string> userNames = ["Alex", "Alice", "Alexander"];
        context.Users.AddRange(userNames.Select(User.Create));
    }

    private static void InitStages(LearningContext context, Guid courseId)
    {
        var document = Stage.Create(StageId.Of(Guid.NewGuid()),
                                    "Document 1", StageType.Of(StageTypes.Document),
                                    StageDuration.Of(TimeSpan.Zero),
                                    courseId,
                                    0);

        var test = Stage.Create(StageId.Of(Guid.NewGuid()),
                                          "Test",
                                          StageType.Of(StageTypes.Test),
                                          StageDuration.Of(TimeSpan.FromHours(2)),
                                          courseId,
                                          1,
                                          document.Id.Value);
        context.Stages.AddRange([document, test]);
    }

}
