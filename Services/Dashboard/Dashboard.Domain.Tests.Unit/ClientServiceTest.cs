using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Dashboard.Infrastructure.Repositories;
using System.Threading;
using Dashboard.Domain.Models;
using Microsoft.Extensions.Logging;
using Dashboard.Domain.Seedwork;
using Dashboard.Domain.Logic;
using System.Data.Entity.Infrastructure;

namespace Dashboard.Domain.Tests.Unit;

[TestClass()]
public class ClientServiceTest
{
    private Mock<IClientRepository> _clientRepoMock = new Mock<IClientRepository>();
    private Mock<Task<Client>> _clientTaskMock = new Mock<Task<Client>>();
    private Client _client = new Client("IsValidClient", "John", "Description Template", new byte[1], 0, 0);
    private Mock<ILogger<IClientService>> _loggerMock = new Mock<ILogger<IClientService>>();
    private Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

    private Project _project = new Project(1, "ValidProject", "Description Template", "Scott");
    private Vacancy _vacancy = new Vacancy(".NET Developer", "ASP.NET/C#", "Remote", "30/09/2049", 1, "Available", "ValidProject", "IsValidClient");

    [TestMethod()]
    public void GetClientAsync_WithValidID_ReturnsClientDTO()
    {
        List<Vacancy> vacancies = new List<Vacancy>() { _vacancy };
        _project.Vacancies = vacancies;

        List<Project> projects = new List<Project>() { _project };
        _client.Projects = projects;

        _clientRepoMock.Setup(c => c.GetAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(_client);

        IClientService clientService = new ClientService(_clientRepoMock.Object);

        var endResult = clientService.GetClientAsync(It.IsAny<int>(), CancellationToken.None);

        Assert.AreEqual("John", endResult.Result.AccountManager);
    }

    [TestMethod()]
    [ExpectedException(typeof(KeyNotFoundException))]
    public async Task GetClientAsync_WithInvalidID_ReturnsKeyNotFoundException()
    {
        _client = null;
        _clientRepoMock.Setup(c => c.GetAsync(It.IsAny<int>(), CancellationToken.None)).Throws(new KeyNotFoundException());

        IClientService clientService = new ClientService(_clientRepoMock.Object);

        var endResult = await clientService.GetClientAsync(It.IsAny<int>(), CancellationToken.None);

    }

    [TestMethod()]
    public async Task GetClients_ClientsListExists_ReturnsClientsList()
    {
        List<Client> clients = new List<Client>();

        List<Vacancy> vacancies = new List<Vacancy>() { _vacancy };
        _project.Vacancies = vacancies;
        
        List<Project> projects = new List<Project>() { _project };
        _client.Projects = projects;
        
        clients.Add(_client);
        clients.Add(_client);
        _clientRepoMock.Setup(c => c.GetAllClients()).ReturnsAsync(clients);

        IClientService clientService = new ClientService(_clientRepoMock.Object);

        var endResult = await clientService.GetClients();

        Assert.AreEqual(2, endResult.Count);
    }

    [TestMethod()]
    public async Task AddClient_WithClientDetails_ReturnsOkResult()
    {
        Client newClient = new Client("Tester", "Bobby", "Descriptor", new byte[1], 0, 0);

        Mock<IClientRepository> clientRepoMock = new Mock<IClientRepository>();
        Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
        clientRepoMock.Setup(c => c.UnitOfWork).Returns(unitOfWorkMock.Object);
        clientRepoMock.Setup(c => c.Add(It.IsAny<Client>())).ReturnsAsync(_client);

        IClientService clientService = new ClientService(clientRepoMock.Object);

        var endResult = await clientService.AddClient(newClient);

        Assert.IsInstanceOfType(endResult, typeof(Client));
    }

    [TestMethod()]
    [ExpectedException(typeof(DbUpdateException))]
    public async Task AddClient_ExceptionErrorOccurs_ReturnsFaulted()
    {
        Mock<IClientRepository> clientRepoMock = new Mock<IClientRepository>();

        clientRepoMock.Setup(c => c.Add(It.IsAny<Client>())).ReturnsAsync(It.IsAny<Client>());
        clientRepoMock.Setup(c => c.UnitOfWork.SaveChangesAsync(CancellationToken.None)).Throws(new DbUpdateException());

        IClientService clientService = new ClientService(clientRepoMock.Object);

        Client clientLocal = new Client("Tester", "Bobby", "Descriptor", new byte[1], 0, 0);


        var endResult = await clientService.AddClient(clientLocal);

    }

    [TestMethod()]
    public void ModifyClient_WithValidID_ReturnsOkResult()
    {
        _clientRepoMock.Setup(c => c.Modify(It.IsAny<Client>(), It.IsAny<Client>()));
        _clientRepoMock.Setup(c => c.UnitOfWork).Returns(_unitOfWorkMock.Object);

        IClientService clientService = new ClientService(_clientRepoMock.Object);

        Client clientLocal = new Client("Tester", "Bobby", "Descriptor", new byte[1], 0, 0);
        var endResult = clientService.Modifyclient(It.IsAny<int>(), clientLocal);

        Assert.IsTrue(endResult.IsCompletedSuccessfully);
    }
}
