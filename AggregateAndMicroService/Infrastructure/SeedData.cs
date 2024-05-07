
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
            var courseId = Guid.NewGuid();
            InitStages(context, courseId);

            var user = User.Create("user1");
            context.Users.Add(user);

            var stages = context.ChangeTracker.Entries<Stage>().Select(e => e.Entity).ToList();
            var courseDto = new CourseDTO("Course 1", "Description", courseId);
            var course = Course.Create(courseDto, stages);
            context.Courses.Add(course);
            context.SaveChanges();
        }

        return app;
    }

    private static void InitStages(LearningContext context, Guid courseId)
    {
        var document = Stage.Create(StageId.Of(Guid.NewGuid()),
                                    "Document 1", StageType.Of(StageTypes.Document),
                                    StageDuration.Of(TimeSpan.Zero),
                                    courseId);

        var test = Stage.Create(StageId.Of(Guid.NewGuid()),
                                          "Test",
                                          StageType.Of(StageTypes.Test),
                                          StageDuration.Of(TimeSpan.FromHours(2)),
                                          courseId,
                                          document.Id.Value);
        context.Stages.AddRange([document, test]);
    }

}
