using Dashboard.Domain.Models;
using Dashboard.Domain.Seedwork;

namespace Dashboard.Domain.Models
{
    public interface IProjectRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Task<Project> AddProject(Project project);
        Task<List<Project>> GetAllClientProjects(int id);
        Task<Project> GetProjectAsync(int id, CancellationToken cancellationToken);
        void Modify(Project projectRecord, Project modifiedProject);
        Task<Project> GetAllProjectsWithClientIdAndProjectName(int client_Id, string projectName, CancellationToken cancellation);
        Task<Project> GetProjectWithSameNameAndClient(int clientId, string projectName, int projectID, CancellationToken cancellationToken);
        Task<Project> GetProjectByNameAndClientName(string projectName, string clientName, CancellationToken cancellation);
    }
}