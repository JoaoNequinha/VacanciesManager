using Dashboard.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.EntityConfig
{
    internal class VacancyEntityTypeConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> entity)
        {
            entity.ToTable("vacancies", DashboardContext.DEFAULT_SCHEMA);
            entity.HasKey(vacancy => vacancy.Id);
            entity.Property(vacancy => vacancy.Id).UseIdentityAlwaysColumn();
            entity.Property(vacancy => vacancy.Name).IsRequired();
            entity.Property(vacancy => vacancy.Skill).IsRequired();
            entity.Property(vacancy => vacancy.Location);
            entity.Property(vacancy => vacancy.Target_start_date).IsRequired();
            entity.Property(vacancy => vacancy.Vacancy_count);
            entity.Property(vacancy => vacancy.Is_open).IsRequired();
            entity.Property(vacancy => vacancy.Project_name).IsRequired();
            entity.Property(vacancy => vacancy.Client_name).IsRequired();
            entity.Property(vacancy => vacancy.ProjectId).IsRequired();
        }
    }
}
