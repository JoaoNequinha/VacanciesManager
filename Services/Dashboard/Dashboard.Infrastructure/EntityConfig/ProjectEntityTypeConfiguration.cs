using Dashboard.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Infrastructure.EntityConfig
{
    internal class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("projects", DashboardContext.DEFAULT_SCHEMA);
            builder.HasKey(project => project.Id);
            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(project => project.Name).IsRequired();
            builder.Ignore(project => project.ClientName);
            builder.Ignore(project => project.ClientLogo);
            builder.Ignore(project => project.VacancyCount);
        }
    }
}
