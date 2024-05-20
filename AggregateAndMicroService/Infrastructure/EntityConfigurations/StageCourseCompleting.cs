using AggregateAndMicroService.Domain.CourseProgress;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AggregateAndMicroService.Infrastructure.EntityConfigurations;

public class StageCourseCompletingConfiguration : IEntityTypeConfiguration<StageCourseCompleting>
{
    public void Configure(EntityTypeBuilder<StageCourseCompleting> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever()
        .HasConversion(id => id.Value, dbId => StageCourseCompletingId.Of(dbId));

        builder.ComplexProperty(
            x => x.StageProgress,
            a => a.Property(e => e.Value)
                    .HasColumnName("StageProgress")
                    .HasDefaultValue(0));

        builder.Property(e => e.CourseCompletingId)
                .HasConversion(id => id.Value, cId => CourseCompletingId.Of(cId));

        builder.HasOne(e => e.CourseCompleting)
            .WithMany(e => e.StageCourseCompletings)
            .HasForeignKey(e => e.CourseCompletingId);

        /* builder.HasOne(e => e.Stage)
            .WithMany(e => e.StageCourseCompletings)
            .HasForeignKey(e => e.StageId);

            builder.HasOne(e => e.CourseCompleting)
            .WithMany(e => e.StageCourseCompletings)
            .HasForeignKey(e => e.CourseCompletingId); */
    }
}
