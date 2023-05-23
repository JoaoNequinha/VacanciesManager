using Dashboard.API.DTO;
using Dashboard.Domain.Models;

namespace Dashboard.API.ExtensionMethods;

public static class ProjectMapper
{
    public static ProjectSummaryDTO ToDTO(this Project project)
    {
        return new ProjectSummaryDTO(project.Id, project.Name, project.VacancyCount, project.Description);
    }

    public static ProjectDTO ToProjectDTO(this Project project)
    {
        return new ProjectDTO(project.Id, project.Name, project.Contact,
            project.Contact, project.ClientName, project.ClientLogo, project.VacancyCount);
    }
}
