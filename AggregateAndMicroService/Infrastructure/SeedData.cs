
using AggregateAndMicroService.Aggregates.Course;

namespace AggregateAndMicroService.Infrastructure;
public class SeedData
{
    public static void Seed(IHost app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<LearningContext>() ?? throw new Exception("Не удалось получить контекст бд");
        context.Database.EnsureCreated();

        if (!context.Courses.Any())
        {
            var courseId = Guid.NewGuid();
            var material1 = Stage.Create(StageId.Of(Guid.NewGuid()),
             "Document 1", StageType.Of(StageTypes.Document),
              StageDuration.Of(TimeSpan.Zero),
               courseId);

        }
        context.SaveChanges();
    }

    private static void InitStages(LearningContext context, Guid courseId)
    {
        var document = Stage.Create(StageId.Of(Guid.NewGuid()),
             "Document 1", StageType.Of(StageTypes.Document),
              StageDuration.Of(TimeSpan.Zero),
               courseId);

        var test = Stage.Create(StageId.Of(Guid.NewGuid()), "Test", StageType.Of(StageTypes.Test), StageDuration.Of(TimeSpan.FromHours(2)), courseId);
    }

}
