using Dashboard.Domain.Models;
using Dashboard.Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.ExcelImport;

public class ExcelDataBaseImporter : IExcelDataBaseImporter
{
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IClientRepository _clientRepository;

    public ExcelDataBaseImporter(IVacancyRepository vacancyRepository, IProjectRepository projectRepository, IClientRepository clientRepository)
    {
        _vacancyRepository = vacancyRepository;
        _projectRepository = projectRepository;
        _clientRepository = clientRepository;
    }

    public async Task ImportToDataBase(List<Vacancy> vacancies)
    {
        await _vacancyRepository.ClearAllVacanciesFromDb();
        await _vacancyRepository.UnitOfWork.SaveChangesAsync();

        foreach (Vacancy vac in vacancies)
        {
            Project? project = await _projectRepository.GetProjectByNameAndClientName(vac.Project_name,
                vac.Client_name, CancellationToken.None);

            if (project != null)
            {
                vac.ProjectId = project.Id;
                _vacancyRepository.Add(vac);
            }
            else
            {
                Client? existingClient = await _clientRepository.GetClientByName(vac.Client_name, CancellationToken.None);

                if (existingClient != null)
                {
                    Project newProject = await _projectRepository.AddProject(new Project(existingClient.Id, vac.Project_name,
                   "More details coming soon", "More details coming soon"));

                    await _projectRepository.UnitOfWork.SaveChangesAsync();

                    vac.ProjectId = newProject.Id;
                    _vacancyRepository.Add(vac);
                }
                else
                {
                    Client newClient = await _clientRepository.Add(new Client(vac.Client_name,
                        "More details coming soon", "More details coming soon", null, 0, 0));
                    await _clientRepository.UnitOfWork.SaveChangesAsync();

                    Project newProject = await _projectRepository.AddProject(new Project(newClient.Id, vac.Project_name,
                   "More details coming soon", "More details coming soon"));
                    await _projectRepository.UnitOfWork.SaveChangesAsync();
                    vac.ProjectId = newProject.Id;
                    _vacancyRepository.Add(vac);
                }
            }

        }
        await _vacancyRepository.UnitOfWork.SaveChangesAsync();

    }





}


