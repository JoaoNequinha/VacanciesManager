using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dashboard.API.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Domain.Models;
using Moq;
using Dashboard.Domain.Logic;
using Microsoft.Extensions.Logging;
using Dashboard.Domain.Seedwork;
using System.Threading;
using System.Data.Entity.Infrastructure;

namespace Dashboard.API.Logic.Tests
{
    [TestClass()]
    public class ProjectServiceTests
    {
        private Mock<IProjectRepository> _ProjectRepoMock = new Mock<IProjectRepository>();
        private Mock<Task<Project>> _ProjectTaskMock = new Mock<Task<Project>>();
        private Project _project = new Project(1, "TestProject", "testProject description", "Bob");
        private Mock<ILogger<IProjectService>> _loggerMock = new Mock<ILogger<IProjectService>>();
        private Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        List<Vacancy> vacancies = new List<Vacancy>() { new Vacancy("Java Developer", "Java", "Remote", "20/03/2034", 5, "Available", "Test Project", "ClientName")
            , new Vacancy("Java Backend Developer", "Java", "Remote", "20/03/2034", 3, "Available", "Test Project", "ClientName") };

        [TestMethod()]
        public async Task GetProjectWithValidID()
        {
            Client client = new Client("Test Client", "Bob", "Description", null, 3, 5);
            _project.Vacancies = vacancies;
            _project.Client = client;

            _ProjectRepoMock.Setup(p => p.GetProjectAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(_project);

            IProjectService projectService = new ProjectService(_ProjectRepoMock.Object);

            var endResult = await projectService.GetProjectAsync(It.IsAny<int>(), CancellationToken.None);

            Assert.AreEqual("TestProject", endResult.Name);

        }


        [TestMethod()]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetProjectWithInvalidIDReturnsKeyNotFoundException()
        {
            _project = null;

            _ProjectRepoMock.Setup(p => p.GetProjectAsync(It.IsAny<int>(), CancellationToken.None)).Throws(new KeyNotFoundException());

            IProjectService projectService = new ProjectService(_ProjectRepoMock.Object);

            var endResult = await projectService.GetProjectAsync(It.IsAny<int>(), CancellationToken.None);

        }


        [TestMethod()]
        public async Task GetListOfProjectsForValidClientId()
        {
            _project.Vacancies = vacancies;

            List<Project> projects = new List<Project>() { _project, _project };

            _ProjectRepoMock.Setup(p => p.GetAllClientProjects(It.IsAny<int>())).ReturnsAsync(projects);

            IProjectService projectService = new ProjectService(_ProjectRepoMock.Object);

            var endResult = await projectService.GetAllProjectsByClient(It.IsAny<int>());

            Assert.AreEqual(2, endResult.Count);

        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetListOfProjectsForInValidClientIdReturnArgumentNullException()
        {
            _ProjectRepoMock.Setup(p => p.GetAllClientProjects(It.IsAny<int>())).Throws(new ArgumentNullException());

            IProjectService projectService = new ProjectService(_ProjectRepoMock.Object);

            var endResult = await projectService.GetAllProjectsByClient(It.IsAny<int>());

        }

        [TestMethod()]
        public async Task SucessfullAddProjectWithValidDetailsReturnsProject()
        {
            Project newProject = new Project(2, "new test project", " new test project description", "new test contact");

            _ProjectRepoMock.Setup(p => p.AddProject(It.IsAny<Project>())).ReturnsAsync(newProject);
            _ProjectRepoMock.Setup(u => u.UnitOfWork).Returns(_unitOfWorkMock.Object);
            //TODO: Again, do we mock something that by default is null? As we expect it to return null
            //_ProjectRepoMock.Setup(p => p.GetAllProjectsWithClientIdAndProjectName(It.IsAny<int>(), It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(new List<Project>());

            IProjectService projectService = new ProjectService(_ProjectRepoMock.Object);

            var endResult = await projectService.AddProject(newProject);

            Assert.AreEqual(2, endResult.ClientId);

        }

        [TestMethod()]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task AddProjectWithInValidDetailsFailsAndReturnsDBUpdateException()
        {
            _ProjectRepoMock.Setup(p => p.AddProject(It.IsAny<Project>()));
            _ProjectRepoMock.Setup(u => u.UnitOfWork.SaveChangesAsync(CancellationToken.None)).Throws(new DbUpdateException());

            IProjectService projectService = new ProjectService(_ProjectRepoMock.Object);

            var endResult = await projectService.AddProject(_project);
        }

        [TestMethod()]
        public async Task ModifyProjectWithValidDetails()
        {

            _ProjectRepoMock.Setup(p => p.GetProjectAsync(It.IsAny<int>(), CancellationToken.None)).
                ReturnsAsync(new Project(1, "Test Project", "Description", "Contact"));
            _ProjectRepoMock.Setup(p => p.Modify(It.IsAny<Project>(), It.IsAny<Project>()));
            _ProjectRepoMock.Setup(u => u.UnitOfWork).Returns(_unitOfWorkMock.Object);

            //TODO: Should we mock the GetProjectWithSameNameAndClient when it returns null? 
            /* Project? projectNull = null;
            _ProjectRepoMock.Setup(p => p.GetProjectWithSameNameAndClient(It.IsAny<int>(), It.IsAny<string>()
                , It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(projectNull);*/

            IProjectService projectService = new ProjectService(_ProjectRepoMock.Object);

            var endResult = projectService.ModifyProject(It.IsAny<int>(), new Project(1, "Test Project", "Description", "Contact"));

            Assert.IsTrue(endResult.IsCompletedSuccessfully);
        }

        [TestMethod()]
        public void ModifyProjectWithInValidID()
        {
            _ProjectRepoMock.Setup(p => p.Modify(It.IsAny<Project>(), It.IsAny<Project>())).Throws(new KeyNotFoundException());
            _ProjectRepoMock.Setup(u => u.UnitOfWork).Returns(_unitOfWorkMock.Object);

            IProjectService projectService = new ProjectService(_ProjectRepoMock.Object);

            Project newprojectDetails = new Project(-1, "project name", "New description string", "new contact string");

            var endResult =  projectService.ModifyProject(1, newprojectDetails);

           Assert.IsInstanceOfType(endResult.Exception.InnerException, typeof(KeyNotFoundException));
        }

    }
}