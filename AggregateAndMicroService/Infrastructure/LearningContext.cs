
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

        /* modelBuilder.Entity<User>().HasKey(e => e.Id);

        modelBuilder.Entity<User>().Property(e => e.Id).ValueGeneratedNever()
        .HasConversion(id => id.Value, dbId => UserId.Of(dbId));

        modelBuilder.Entity<Course>().HasKey(p => p.Id);

        modelBuilder.Entity<Course>().Property(r => r.Id).ValueGeneratedNever()
            .HasConversion(materialId => materialId.Value, dbId => CourseId.Of(dbId));

        modelBuilder.Entity<Course>().ComplexProperty(
            x => x.Status,
            a => a.Property(p => p.Value)
                    .HasColumnName("Status")
                    .IsRequired());

        modelBuilder.Entity<Course>().ComplexProperty(
            x => x.StageCount,
            a => a.Property(p => p.Value)
                    .HasColumnName("StageCount")
                    .HasDefaultValue(0)); */
        /* 
                modelBuilder.Entity<Stage>().HasKey(e => e.Id);

                modelBuilder.Entity<Stage>().Property(e => e.Id).ValueGeneratedNever()
                .HasConversion(id => id.Value, dbId => StageId.Of(dbId));

                modelBuilder.Entity<Stage>().ComplexProperty(
                    x => x.Type,
                    a => a.Property(p => p.Value)
                            .HasColumnName("Type")
                            .IsRequired());

                modelBuilder.Entity<Stage>().ComplexProperty(
                    x => x.Duration,
                    a => a.Property(p => p.Value)
                            .HasColumnName("Duration")
                            .IsRequired());
        */

        /*  modelBuilder.Entity<CourseCompleting>().HasKey(e => e.Id);

         modelBuilder.Entity<CourseCompleting>().Property(e => e.Id).ValueGeneratedNever()
         .HasConversion(id => id.Value, dbId => CourseCompletingId.Of(dbId));


         modelBuilder.Entity<CourseCompleting>().ComplexProperty(
             x => x.Status,
             a => a.Property(p => p.Value)
                     .HasColumnName("Status")
                     .IsRequired());

         modelBuilder.Entity<CourseCompleting>().ComplexProperty(
             x => x.Progress,
             a => a.Property(p => p.Value)
                     .HasColumnName("Progress")
                     .HasDefaultValue(0));

         modelBuilder.Entity<CourseCompleting>().ComplexProperty(
             x => x.StagesCountData,
             a =>
             {
                 a.Property(p => p.TotalStages)
                     .HasColumnName("TotalStages")
                     .HasDefaultValue(0);

                 a.Property(p => p.CompletedStages)
                     .HasColumnName("CompletedStages")
                     .HasDefaultValue(0);
             }
         ); */

        /* modelBuilder.Entity<StageCourseCompleting>().HasKey(e => new { e.CourseCompletingId, e.StageId });

        modelBuilder.Entity<StageCourseCompleting>().ComplexProperty(
            x => x.StageProgress,
            a => a.Property(e => e.Value)
                    .HasColumnName("StageProgress")
                    .HasDefaultValue(0)); */

    }

    public async Task SaveEntitiesAsync()
    {
        await _mediator.DispatchDomainEventsAsync(this);
        await base.SaveChangesAsync();
    }


}
