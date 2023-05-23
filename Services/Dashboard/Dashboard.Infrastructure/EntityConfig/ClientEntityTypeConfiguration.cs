using Dashboard.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.EntityConfig
{
    internal class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entity)
        {
            entity.ToTable("clients", DashboardContext.DEFAULT_SCHEMA);
            entity.HasKey(client => client.Id);
            entity.Property(c => c.Id).UseIdentityColumn();
            entity.Property(client => client.Name).IsRequired();
            entity.Ignore(client => client.VacancyCount);
            entity.Ignore(client => client.ProjectCount);           
        }
    }
}
