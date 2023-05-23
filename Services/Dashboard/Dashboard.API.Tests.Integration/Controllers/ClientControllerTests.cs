using Dashboard.Domain.Models;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using Dashboard.API.DTO;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Dashboard.API.Controllers;
using Moq;
using System.Collections.Generic;
using System.Security.Policy;

namespace Dashboard.API.Tests.Integration.Controllers;

public class ClientControllerTests: IClassFixture<TestApplicationFixture>
{

    private readonly HttpClient _httpClient;

    public ClientControllerTests(TestApplicationFixture fixture)
    {
        _httpClient = fixture.HttpClient;
    }

    [Fact]
    public async Task GetClients_ReturnSuccessAndCorrectContentType()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/clients");

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");

        var streamContent = await response.Content.ReadAsStreamAsync();
        var clients = await JsonSerializer.DeserializeAsync<ClientCreationDTO[]>(streamContent);

        Assert.IsType<ClientCreationDTO[]>(clients);
    }

    /*[Fact]
    public async Task GetClient_ReturnSuccessAndCorrectContentType()
    {
        var newClient = new ClientCreationDTO("GetClientReturnSuccess", "Joao", "Description Template", new byte[1]);
        var httpContent = new StringContent(JsonSerializer.Serialize(newClient), Encoding.UTF8, "application/json");
        var responseFromPost = await _httpClient.PostAsync("api/clients", httpContent);

        var response = await _httpClient.GetAsync(responseFromPost.Headers.Location.AbsolutePath);

        response.EnsureSuccessStatusCode();
        response!.Content!.Headers!.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
    }*/

    [Fact]
    public async Task GetClient_ReturnFailedAndErrorCode()
    {
        var response = await _httpClient.GetAsync("/api/clients/-1");

        Assert.Matches("NotFound", response.StatusCode.ToString());
    }

   /* [Fact]
    public async Task ModifyClient_ReturnSuccess()
    {
        var newClient = new ClientCreationDTO("ToBeModifiedClient", "Joao", "Description Template", new byte[1]);
        var httpContent = new StringContent(JsonSerializer.Serialize(newClient), Encoding.UTF8, "application/json");
        var responseFromPost = await _httpClient.PostAsync("api/clients", httpContent);

        var modifiedClient = new ModifyClientDTO("ToBeModifiedClient", "Peter Parker", "Description Template", new byte[1]);
        httpContent = new StringContent(JsonSerializer.Serialize(modifiedClient), Encoding.UTF8, "application/json");
        var responseFromModify = await _httpClient.PutAsync(responseFromPost.Headers.Location.AbsolutePath, httpContent);

        var getResponse = await _httpClient.GetAsync(responseFromPost.Headers.Location.AbsolutePath);

        var modifiedClientManager = JObject.Parse(await getResponse.Content.ReadAsStringAsync()).GetValue("account_manager");

        responseFromPost.EnsureSuccessStatusCode();
        responseFromModify.EnsureSuccessStatusCode();
        Assert.Equal("Peter Parker", modifiedClientManager);
    }*/

    /*[Fact]
    public async Task ModifyClient_ReturnClientAlreadyExists()
    {
        var newClient = new ClientCreationDTO("FreshClientForModification", "Joao", "Description Template", new byte[1]);
        var httpContent = new StringContent(JsonSerializer.Serialize(newClient), Encoding.UTF8, "application/json");
        var responseFromPost = await _httpClient.PostAsync("api/clients", httpContent);

        var newClientforAlreadyExisting = new ClientCreationDTO("AlreadyExistingClientIssue", "Joao", "Description Template", new byte[1]);
        var httpContentForNewAlreadyExisting = new StringContent(JsonSerializer.Serialize(newClientforAlreadyExisting), Encoding.UTF8, "application/json");
        var responseFromPostForNewAlreadyExisting = await _httpClient.PostAsync("api/clients", httpContentForNewAlreadyExisting);

        var modifiedClient = new ModifyClientDTO("AlreadyExistingClientIssue", "Peter Parker", "Description Template", new byte[1]);
        httpContent = new StringContent(JsonSerializer.Serialize(modifiedClient), Encoding.UTF8, "application/json");
        var responseFromModify = await _httpClient.PutAsync(responseFromPost.Headers.Location.AbsolutePath, httpContent);

        Assert.Matches("Conflict", responseFromModify.StatusCode.ToString());
    }*/

    /*[Fact]
    public async Task ModifyClient_ReturnNotFoundWhenInvalidID()
    {
        var modifiedClient = new ModifyClientDTO("RandomClientThatDoesNotExist", "Peter Parker", "Description Template", new byte[1]);
        var httpContent = new StringContent(JsonSerializer.Serialize(modifiedClient), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("/api/clients/-1", httpContent);

        Assert.Matches("NotFound", response.StatusCode.ToString());
    }
*/
/*    [Fact]
    public async Task CreateClient_ReturnSuccessCode()
    {
        var newClient = new ClientCreationDTO("CreateFreshClient", "Joao", "Description Template", new byte[1]);
        var httpContent = new StringContent(JsonSerializer.Serialize(newClient), Encoding.UTF8, "application/json");
        var responseFromPost = await _httpClient.PostAsync("api/clients", httpContent);
       
        Assert.Matches("Created", responseFromPost.StatusCode.ToString());
    }*/
}
