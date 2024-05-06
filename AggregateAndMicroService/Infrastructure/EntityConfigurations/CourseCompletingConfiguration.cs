using AggregateAndMicroService.Domain.CourseProgress;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AggregateAndMicroService.Infrastructure.EntityConfigurations;

public class CourseCompletingConfiguration : IEntityTypeConfiguration<CourseCompleting>
{
    public void Configure(EntityTypeBuilder<CourseCompleting> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever()
        .HasConversion(id => id.Value, dbId => CourseCompletingId.Of(dbId));


        builder.ComplexProperty(
            x => x.Status,
            a => a.Property(p => p.Value)
                    .HasColumnName("Status")
                    .IsRequired());

        builder.ComplexProperty(
            x => x.Progress,
            a => a.Property(p => p.Value)
                    .HasColumnName("Progress")
                    .HasDefaultValue(0));

        builder.ComplexProperty(
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
        );

        /* builder.HasOne(e => e.User)
            .WithMany(e => e.CourseCompletings)
            .HasForeignKey(e => e.UserId);

        builder.HasOne(e => e.Course)
            .WithMany(e => e.Completings)
            .HasForeignKey(e => e.CourseId); */
    }
}
