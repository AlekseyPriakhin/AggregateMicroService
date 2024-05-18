using AggregateAndMicroService.Domain.User;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AggregateAndMicroService.Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, dbId => UserId.Of(dbId));
    }
}
