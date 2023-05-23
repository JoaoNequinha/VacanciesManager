using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Dashboard.Infrastructure.Repositories;
using System.Threading;
using Dashboard.Domain.Models.Client;
using Microsoft.Extensions.Logging;
using Dashboard.Domain.Seedwork;
using Dashboard.Domain.Logic;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Domain.Tests.Unit;

[TestClass()]
public class ClientServiceTest
{
    private Mock<IClientRepository> _clientRepoMock = new Mock<IClientRepository>();
    private Mock<Task<Client>> _clientTaskMock = new Mock<Task<Client>>();
    private Client _client = new Client("IsValidClient", "Bob", "Description yes", "BestLogoEver",0,0);
    private Mock<ILogger<IClientService>> _loggerMock = new Mock<ILogger<IClientService>>();
    private Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

    
    [TestMethod()]
    public void GetClientAsync_WithValidID_ReturnsClientDTO()
    {
        _clientRepoMock.Setup(c => c.GetAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(_client);

        IClientService clientService = new ClientService(_loggerMock.Object, _clientRepoMock.Object);

        var endResult = clientService.GetClientAsync(It.IsAny<int>(), CancellationToken.None);

        Assert.AreEqual("Bob", endResult.Result.AccountManager);
    }

    [TestMethod()]
    public void GetClientAsync_WithInvalidID_ReturnsKeyNotFoundException()
    {
        _client = null;
        _clientRepoMock.Setup(c => c.GetAsync(It.IsAny<int>(), CancellationToken.None)).Throws(new KeyNotFoundException());

        IClientService clientService = new ClientService(_loggerMock.Object, _clientRepoMock.Object);

        var endResult = clientService.GetClientAsync(It.IsAny<int>(), CancellationToken.None);

        Assert.IsInstanceOfType(endResult.Exception.InnerException, typeof(KeyNotFoundException));
    }


    [TestMethod()]
    public async Task GetClients_ClientsListExists_ReturnsClientsList()
    {
        List<Client> clients = new List<Client>();
        clients.Add((Client)_client);
        clients.Add((Client)_client);
        _clientRepoMock.Setup(c => c.GetAllClients()).ReturnsAsync(clients);

        IClientService clientService = new ClientService(_loggerMock.Object, _clientRepoMock.Object);

        var endResult = await clientService.GetClients();

        Assert.AreEqual(2, endResult.Count);
    }


    [TestMethod()]
    public void AddClient_WithClientDetails_ReturnsOkResult()
    {
        Client clientLocal = new Client("Tester", "Bobby", "Descriptor", "LogoOfAmazingStuff", 0, 0);

        Mock<IClientRepository> clientRepoMock = new Mock<IClientRepository>();
        Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
        clientRepoMock.Setup(c => c.UnitOfWork).Returns(unitOfWorkMock.Object);
        clientRepoMock.Setup(c => c.Add(It.IsAny<Client>())).Returns(_client);

        IClientService clientService = new ClientService(_loggerMock.Object, clientRepoMock.Object);
        
        var endResult = clientService.AddClient(clientLocal);

        Assert.IsInstanceOfType(endResult.Result, typeof(Client));
    }

    [TestMethod()]
    public void AddClient_ExceptionErrorOccurs_ReturnsFaulted()
    {
        Mock<IClientRepository> clientRepoMock = new Mock<IClientRepository>();
        
        clientRepoMock.Setup(c => c.Add(It.IsAny<Client>())).Returns(It.IsAny<Client>());
        clientRepoMock.Setup(c => c.UnitOfWork.SaveChangesAsync(CancellationToken.None)).Throws(new DbUpdateException());

        IClientService clientService = new ClientService(_loggerMock.Object, clientRepoMock.Object);

        Client clientLocal = new Client("Tester", "Bobby", "Descriptor", "LogoOfAmazingStuff",0,0);


        var endResult = clientService.AddClient(clientLocal);
        Assert.IsInstanceOfType(endResult.Exception.InnerException, typeof(DbUpdateException));
    }
    


    [TestMethod()]
    public void ModifyClient_WithValidID_ReturnsOkResult()
    {
        _clientRepoMock.Setup(c => c.Modify(It.IsAny<int>(),It.IsAny<Client>()));
        _clientRepoMock.Setup(c => c.UnitOfWork).Returns(_unitOfWorkMock.Object);

        IClientService clientService = new ClientService(_loggerMock.Object, _clientRepoMock.Object);

        Client clientLocal = new Client("Tester", "Bobby", "Descriptor", "LogoOfAmazingStuff",0,0);
        var endResult = clientService.Modifyclient(It.IsAny<int>(), clientLocal);

        Assert.IsTrue(endResult.IsCompletedSuccessfully);
    }

    [TestMethod()]
    public void ModifyClient_WithInvalidID_ReturnsBadRequestResult()
    {
        _clientRepoMock.Setup(c => c.Modify(It.IsAny<int>(), It.IsAny<Client>())).Throws(new NullReferenceException());
        _clientRepoMock.Setup(c => c.UnitOfWork).Returns(_unitOfWorkMock.Object);

        IClientService clientService = new ClientService(_loggerMock.Object, _clientRepoMock.Object);

        Client clientLocal = new Client("Tester", "Bobby", "Descriptor", "LogoOfAmazingStuff",0,0);
        var endResult = clientService.Modifyclient(-1, clientLocal);

        Assert.IsInstanceOfType(endResult.Exception.InnerException, typeof(NullReferenceException));
    }
}
