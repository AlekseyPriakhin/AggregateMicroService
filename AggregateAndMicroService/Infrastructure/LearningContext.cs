
using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Domain.CourseProgress;
using AggregateAndMicroService.Domain.User;
using AggregateAndMicroService.Infrastructure.EntityConfigurations;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Ordering.Infrastructure;

namespace AggregateAndMicroService.Infrastructure;


// TODO Добавить отложенную обработку доменных событий
public class LearningContext : DbContext
{
    private readonly IMediator _mediator;

    public DbSet<Course> Courses { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Stage> Stages { get; set; }

    public DbSet<StageCourseCompleting> StageCourseCompletings { get; set; }

    public DbSet<CourseCompleting> CourseCompleting { get; set; }

    public LearningContext(DbContextOptions<LearningContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
        //Database.EnsureCreated();
    }

    public LearningContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new StageConfiguration());
        modelBuilder.ApplyConfiguration(new CourseCompletingConfiguration());
        modelBuilder.ApplyConfiguration(new StageCourseCompletingConfiguration());
    }

    public async Task SaveEntitiesAsync()
    {
        await _mediator.DispatchDomainEventsAsync(this);
        await base.SaveChangesAsync();
    }


}
