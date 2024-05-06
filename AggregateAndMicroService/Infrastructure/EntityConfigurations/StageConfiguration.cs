using AggregateAndMicroService.Domain.Course;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AggregateAndMicroService.Infrastructure.EntityConfigurations;

public class StageConfiguration : IEntityTypeConfiguration<Stage>
{
    public void Configure(EntityTypeBuilder<Stage> builder)
    {

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever()
        .HasConversion(id => id.Value, dbId => StageId.Of(dbId));

        builder.ComplexProperty(
            x => x.Type,
            a => a.Property(p => p.Value)
                    .HasColumnName("Type")
                    .IsRequired());

        builder.ComplexProperty(
            x => x.Duration,
            a => a.Property(p => p.Value)
                    .HasColumnName("Duration")
                    .IsRequired());

        /* builder.HasOne(e => e.Course)
            .WithMany(e => e.Stages)
            .HasForeignKey(e => e.CourseId); */
    }
}
