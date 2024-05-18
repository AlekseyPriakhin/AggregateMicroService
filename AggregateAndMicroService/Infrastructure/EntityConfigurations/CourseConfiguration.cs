using AggregateAndMicroService.Domain.Course;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AggregateAndMicroService.Infrastructure.EntityConfigurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {

        builder.HasKey(p => p.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever()
            .HasConversion(materialId => materialId.Value, dbId => CourseId.Of(dbId));

        builder.ComplexProperty(
            x => x.Status,
            a => a.Property(p => p.Value)
                    .HasColumnName("Status")
                    .IsRequired());

        builder.ComplexProperty(
            x => x.StageCount,
            a => a.Property(p => p.Value)
                    .HasColumnName("StageCount")
                    .HasDefaultValue(0));
    }
}
