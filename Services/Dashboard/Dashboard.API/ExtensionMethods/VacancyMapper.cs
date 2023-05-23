using Dashboard.Domain.Models;

namespace Dashboard.API.DTO;
public static class VacancyMapper
{
    public static VacancyDTO ToDTO(this Vacancy vacancy)
    {
        return new VacancyDTO(vacancy.Id, vacancy.Name, vacancy.Skill, vacancy.Location,
                    vacancy.Target_start_date, vacancy.Vacancy_count, vacancy.Is_open,
                    vacancy.Project_name, vacancy.Client_name, vacancy.ProjectId, vacancy.Project.ClientId);
    }
}
