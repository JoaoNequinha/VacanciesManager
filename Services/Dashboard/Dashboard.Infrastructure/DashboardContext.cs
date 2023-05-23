using Dashboard.Domain.Models;
using Dashboard.Domain.Seedwork;
using Dashboard.Infrastructure.EntityConfig;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Infrastructure;

public class DashboardContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "dashboard";
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }

    public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
    {
       
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.ApplyConfiguration(new UserEntityTypeConfiguration());

        model.ApplyConfiguration(new ClientEntityTypeConfiguration());

        model.ApplyConfiguration(new ProjectEntityTypeConfiguration());

        model.ApplyConfiguration(new VacancyEntityTypeConfiguration());

    }
   
}