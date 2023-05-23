using Dashboard.Domain.Models;
using Dashboard.Infrastructure.Repositories;

namespace Dashboard.Domain.Logic
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;

        public VacancyService(IVacancyRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        }

        public async Task<Vacancy> GetVacancyAsync(int id, CancellationToken cancellationToken)
        {
            return await _vacancyRepository.GetAsync(id, cancellationToken);
        }

        public async Task<List<Vacancy>> GetAllVacancies()
        {
            return await _vacancyRepository.GetAllVacancies();
        }

        public async Task<List<Vacancy>> GetAllVacanciesByProject(int project_id)
        {
            return await _vacancyRepository.GetAllVacanciesPerProject(project_id);
        }

        public async Task<Vacancy> AddVacancy(Vacancy vacancy)
        {
            _vacancyRepository.Add(vacancy);
            await _vacancyRepository.UnitOfWork.SaveChangesAsync();
            return vacancy;
        }
    }
}
