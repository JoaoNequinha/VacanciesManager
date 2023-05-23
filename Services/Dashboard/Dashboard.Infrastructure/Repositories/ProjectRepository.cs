using Dashboard.Domain.Models;
using Dashboard.Domain.Seedwork;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dashboard.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DashboardContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public ProjectRepository(DashboardContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Project> AddProject(Project project) => _context.Projects.Add(project).Entity;

        public void Modify(Project projectRecord, Project modifiedProject)
        {
            // TODO are ADMINS suppose to be able to change projects to other clients?? projectEntity.ClientId = project.ClientId;

            projectRecord.Name = modifiedProject.Name;
            projectRecord.Description = modifiedProject.Description;
            projectRecord.Contact = modifiedProject.Contact;
            _context.Entry(projectRecord).State = EntityState.Modified;
        }

        public async Task<Project> GetProjectWithSameNameAndClient(int clientId, string projectName, int projectID, CancellationToken cancellationToken)
        {
            return await _context.Projects.Where(p => p.ClientId == clientId
                && p.Name == projectName
                && p.Id != projectID).FirstOrDefaultAsync();
        }

        public async Task<Project> GetProjectAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Projects.Include("Client").Include("Vacancies").Where(c => c.Id == id).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException();
        }

        public async Task<Project> GetAllProjectsWithClientIdAndProjectName(int client_Id, string projectName, CancellationToken cancellation)
        {
            return await _context.Projects.Where(p => p.ClientId == client_Id && p.Name == projectName).FirstOrDefaultAsync();
        }

        public async Task<Project> GetProjectByNameAndClientName(string projectName, string clientName, CancellationToken cancellation)
        {
            return await _context.Projects.Include("Client").Where(p => p.Client.Name == clientName && p.Name == projectName).FirstOrDefaultAsync() ?? null;
        }

        public async Task<List<Project>> GetAllClientProjects(int id)
        {
            return await _context.Projects.Include("Client").Include("Vacancies").Where(c => c.Client.Id == id).ToListAsync();
        }    

    }
}
