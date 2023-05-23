using Dashboard.Domain.Models;
using Dashboard.Domain.Seedwork;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Infrastructure.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly DashboardContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public VacancyRepository(DashboardContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Vacancy Add(Vacancy vacancy) => _context.Vacancies.Add(vacancy).Entity;

        public async Task<Vacancy> GetAsync(int id, CancellationToken cancellationToken)
        => await _context.Vacancies.Include("Project").FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
                ?? _context.Vacancies.Local.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException();

        public async Task<List<Vacancy>> GetAllVacancies()
        {
            return await _context.Vacancies.Include("Project").ToListAsync();
        }

        public async Task<List<Vacancy>> GetAllVacanciesPerProject(int project_id)
        {
            return await _context.Vacancies.Include("Project").Where(v => v.ProjectId == project_id).ToListAsync();
        }

        public async Task ClearAllVacanciesFromDb()
        { 
            _context.Vacancies.RemoveRange(await GetAllVacancies());
        }
    }
}
