using Dashboard.Domain.Models;

namespace Dashboard.Domain.Logic
{
    public interface IVacancyService
    {
        Task<List<Vacancy>> GetAllVacancies();
        Task<Vacancy> GetVacancyAsync(int id, CancellationToken cancellationToken);
        Task<List<Vacancy>> GetAllVacanciesByProject(int project_id);
        Task<Vacancy> AddVacancy(Vacancy vacancy);
    }
}