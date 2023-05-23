using Dashboard.API.DTO;
using Dashboard.Domain.Models;
using Dashboard.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Dashboard.API.Tests.Integration;

internal class TestApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Action<IServiceCollection>? _serviceOverride;

    public TestApplicationFactory(Action<IServiceCollection>? serviceOverride = null)
    {
        _serviceOverride = serviceOverride;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DashboardContext>));


            if (descriptor is not null) services.Remove(descriptor);

            services.AddDbContext<DashboardContext>(options =>
            {
                options.UseInMemoryDatabase("test_dashboard");
            });

            services.AddSingleton<TestApplicationFactory>();

            using IServiceScope scope = services.BuildServiceProvider().CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            DashboardContext db = scopedServices.GetRequiredService<DashboardContext>();
            ILogger<TestApplicationFactory> logger = scopedServices.GetRequiredService<ILogger<TestApplicationFactory>>();
            
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
       

            try
            {

                //Utilities.InitializeDbForTests(db);
                db.Clients.Add(new Client("ProjectTestClient", "Bob", "Goldman Description", new byte[1], 2, 13));

                db.Projects.Add(new Project(1, "ProjectTestForVacancy", "Project Description", "Bob"));

                db.Vacancies.Add(new Vacancy("Java Developer", "Java", "Remote", "20/04/2022", 15, "Open", "ProjectTestForVacancy",
                        "ProjectTestClient"));

                db.Vacancies.Add(new Vacancy("Java Developer", "Java", "Remote", "20/04/2022", 15, "Open", "ProjectTestForVacancy",
                        "ProjectTestClient"));          

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {Message}", ex.Message);
            }
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {

        if (_serviceOverride is not null)
        {
            builder.ConfigureServices(_serviceOverride);
        }

        return base.CreateHost(builder);
    }
}