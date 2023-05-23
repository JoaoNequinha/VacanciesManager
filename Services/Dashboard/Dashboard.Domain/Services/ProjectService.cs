using Dashboard.Domain.Logic;
using Dashboard.Domain.Models;


namespace Dashboard.API.Logic
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Project> GetProjectAsync(int id, CancellationToken cancellationToken)
        {
            Project? project = await _projectRepository.GetProjectAsync(id, cancellationToken);

            project.ClientName = project.Client.Name;
            project.ClientLogo = project.Client.ClientLogo;
            int vacancyCount = 0;
            foreach (Vacancy v in project.Vacancies)
            {
                vacancyCount += v.Vacancy_count;
            }
            project.VacancyCount = vacancyCount;

            return project;
        }
        public async Task<List<Project>> GetAllProjectsByClient(int clientId)
        {
            List<Project> projects = await _projectRepository.GetAllClientProjects(clientId);

            foreach (Project p in projects)
            {
                int vacancyCount = 0;
                foreach (Vacancy v in p.Vacancies)
                {
                    vacancyCount += v.Vacancy_count;
                }
                p.VacancyCount = vacancyCount;
            }

            return projects;
        }
        public async Task<Project> AddProject(Project project)
        {
            var existingProject = await _projectRepository.GetAllProjectsWithClientIdAndProjectName(project.ClientId, project.Name, CancellationToken.None);
            
            if (existingProject != null)
                throw new ArgumentException(); //TODO: Change to custom exception

            await _projectRepository.AddProject(project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();
            return project;
        }
        public async Task ModifyProject(int projectId, Project project)
        {

            Project projectRecord = await _projectRepository.GetProjectAsync(projectId, CancellationToken.None)
                ?? throw new KeyNotFoundException();

            Project? existingProject = await _projectRepository.GetProjectWithSameNameAndClient(projectRecord.ClientId,
                project.Name, projectId, CancellationToken.None);

            if (existingProject != null)
                throw new ArgumentException();


            _projectRepository.Modify(projectRecord, project);
            await _projectRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}
