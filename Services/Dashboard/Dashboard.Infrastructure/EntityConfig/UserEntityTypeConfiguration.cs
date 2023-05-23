using Dashboard.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.EntityConfig;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("users", DashboardContext.DEFAULT_SCHEMA);
        entity.HasKey(user => user.Id);
        entity.OwnsOne(user => user.Name, name => name.WithOwner());
        entity.OwnsOne(user => user.Address, address => address.WithOwner());
        entity
            .Property(user => user.Email)
            .HasConversion(email => email.ToString(), address => new Email(address))
            .IsRequired();
    }
}
