using Dashboard.Domain.Models;


namespace Dashboard.Domain.Logic
{
    public interface IProjectService
    {     
        //TODO void DeleteProject();
        Task<Project> GetProjectAsync(int id, CancellationToken cancellationToken);
        Task <List<Project>> GetAllProjectsByClient(int id);
        Task ModifyProject(int projectId, Project project);
        Task<Project> AddProject(Project project);
        
    }
}