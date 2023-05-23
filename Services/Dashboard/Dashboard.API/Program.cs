using Dashboard.API.Logic;
using Dashboard.Domain.ExcelImport;
using Dashboard.Domain.Logic;
using Dashboard.Domain.Models;
using Dashboard.Domain.Services;
using Dashboard.Infrastructure;
using Dashboard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.AddEnvironmentVariables().Build();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IVacancyService, VacancyService>();
builder.Services.AddTransient<IVacancyRepository, VacancyRepository>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IExcelImportService, ExcelImportService>();
builder.Services.AddTransient<IExcelImportFlow, ExcelImportFlow>();
builder.Services.AddTransient<IFileReader, FileReader>();
builder.Services.AddTransient<IHeaderReader, HeaderReader>();
builder.Services.AddTransient<IDataTableSetup, DataTableSetup>();
builder.Services.AddTransient<IImportValidator, ImportValidator>();
builder.Services.AddTransient<IVacancySelector, VacancySelector>();
builder.Services.AddTransient<IVacancyParser, VacancyParser>();
builder.Services.AddTransient<IExcelDataBaseImporter, ExcelDataBaseImporter>();
builder.Services.AddTransient<ISheetSelector, SheetSelector>();



builder.Services
    .AddDbContext<DashboardContext>(options => options
        .UseNpgsql(config["CONNECTION_STRING"]!)
        .UseSnakeCaseNamingConvention());

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                      });
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DashboardContext>();
    
    if(dataContext.Database.IsRelational())
        dataContext.Database.Migrate();
}


app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

