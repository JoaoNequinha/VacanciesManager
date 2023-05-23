using Dashboard.Domain.Models;
using Dashboard.Domain.Seedwork;

namespace Dashboard.Infrastructure.Repositories
{
    public interface IVacancyRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<List<Vacancy>> GetAllVacancies();
        Vacancy Add(Vacancy vacancy);
        Task<List<Vacancy>> GetAllVacanciesPerProject(int project_id);
        Task<Vacancy> GetAsync(int id, CancellationToken cancellationToken);
        Task ClearAllVacanciesFromDb();
    }
}