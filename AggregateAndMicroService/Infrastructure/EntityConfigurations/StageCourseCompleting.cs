using AggregateAndMicroService.Domain.CourseProgress;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AggregateAndMicroService.Infrastructure.EntityConfigurations;

public class StageCourseCompletingConfiguration : IEntityTypeConfiguration<StageCourseCompleting>
{
    public void Configure(EntityTypeBuilder<StageCourseCompleting> builder)
    {
        builder.HasKey(e => new { e.CourseCompletingId, e.StageId });

        builder.ComplexProperty(
            x => x.StageProgress,
            a => a.Property(e => e.Value)
                    .HasColumnName("StageProgress")
                    .HasDefaultValue(0));

        /* builder.HasOne(e => e.Stage)
            .WithMany(e => e.StageCourseCompletings)
            .HasForeignKey(e => e.StageId);

            builder.HasOne(e => e.CourseCompleting)
            .WithMany(e => e.StageCourseCompletings)
            .HasForeignKey(e => e.CourseCompletingId); */
    }
}
