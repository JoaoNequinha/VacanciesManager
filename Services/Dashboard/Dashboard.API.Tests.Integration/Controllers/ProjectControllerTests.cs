using Dashboard.API.DTO;
using Dashboard.Domain.Models;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Dashboard.API.Tests.Integration.Controllers;

public class ProjectControllerTests : IClassFixture<TestApplicationFixture>
{
    private readonly HttpClient _httpClient;
    private static ModifyProjectDTO CreateLandmarkCreationDTO() => new("Landmark", "Bob", "Description To be changed", 1);

    public ProjectControllerTests(TestApplicationFixture fixture)
    {
        _httpClient = fixture.HttpClient;
    }

    [Fact]
    public async Task GetProjectsByClientID_ReturnSuccessAndCorrectContentType()
    {
        var response = await _httpClient.GetAsync("/api/projects?client_id=1");

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");

        var streamContent = await response.Content.ReadAsStreamAsync();
        var projects = await JsonSerializer.DeserializeAsync<ProjectSummaryDTO[]>(streamContent);

        projects!.Length.Should().Be(projects.Length);
        Assert.IsType<ProjectSummaryDTO[]>(projects);
    }

    [Fact]
    public async Task GetProject_ReturnFailedAndErrorCode()
    {
        var response = await _httpClient.GetAsync("/api/projects/-1");

        Assert.Matches("NotFound", response.StatusCode.ToString());
    }

    [Fact]
    public async Task GetProject_ReturnSuccessAndCorrectContentType()
    {
        var response = await _httpClient.GetAsync("/api/projects/1");

        response.EnsureSuccessStatusCode();
        response!.Content!.Headers!.ContentType!.ToString().Should().Be("application/json; charset=utf-8");

        var streamContent = await response.Content.ReadAsStreamAsync();
        var project = await JsonSerializer.DeserializeAsync<ProjectDTO>(streamContent);

        Assert.True(project.name == "ProjectTestForVacancy");
    }

    [Fact]
    public async Task ModifyProject_ReturnSuccess()
    {
        ProjectCreationDTO projectCreationDTO = new("CreationForModification", "Mr Contact", "Description Template", 1);
        var httpContent = new StringContent(JsonSerializer.Serialize(projectCreationDTO), Encoding.UTF8, "application/json");
        var createResponse = await _httpClient.PostAsync("/api/projects/", httpContent);

        ModifyProjectDTO creationDTO = new("ProActive", "Bob", "Desc", 1);
        httpContent = new StringContent(JsonSerializer.Serialize(creationDTO), Encoding.UTF8, "application/json");
        var modifyResponse = await _httpClient.PutAsync(createResponse.Headers.Location.AbsolutePath, httpContent);

        Assert.Matches("OK", modifyResponse.StatusCode.ToString());
    }

    [Fact]
    public async Task ModifyProject_ReturnsAlreadyExists()
    {
        ProjectCreationDTO projectCreationDTO = new("testProject", "Mr Contact", "Description Template", 1);
        var httpContent = new StringContent(JsonSerializer.Serialize(projectCreationDTO), Encoding.UTF8, "application/json");
        var createResponse = await _httpClient.PostAsync("/api/projects/", httpContent);

        ProjectCreationDTO secondProjectCreationDTO = new("testProject2", "Mr Contact", "Description Template", 1);
        var httpContentSecondProject = new StringContent(JsonSerializer.Serialize(secondProjectCreationDTO), Encoding.UTF8, "application/json");
        var createResponseSecondproject = await _httpClient.PostAsync("/api/projects/", httpContentSecondProject);

        ModifyProjectDTO creationDTO = new("testProject", "Bob", "Desc", 1);
        httpContent = new StringContent(JsonSerializer.Serialize(creationDTO), Encoding.UTF8, "application/json");
        var modifyResponse = await _httpClient.PutAsync(createResponseSecondproject.Headers.Location.AbsolutePath, httpContent);

        Assert.Matches("Conflict", modifyResponse.StatusCode.ToString());
    }

    [Fact]
    public async Task ModifyProject_ReturnNotFoundWhenInvalidID()
    {
        var httpContent = new StringContent(JsonSerializer.Serialize(CreateLandmarkCreationDTO()), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("/api/projects/-1", httpContent);

        Assert.Matches("NotFound", response.StatusCode.ToString());
    }

    [Fact]
    public async Task CreateProject_ReturnConflict()
    {
        ProjectCreationDTO initialProject = new("newTestProject", "Mr Contact", "Description Template", 1);
        var httpContent = new StringContent(JsonSerializer.Serialize(initialProject), Encoding.UTF8, "application/json");
        var createResponse = await _httpClient.PostAsync("/api/projects/", httpContent);

        ProjectCreationDTO conflictingProject = new("newTestProject", "Mr Contact", "Description Template", 1);
        var httpContentSecondProject = new StringContent(JsonSerializer.Serialize(conflictingProject), Encoding.UTF8, "application/json");
        var createResponseSecondproject = await _httpClient.PostAsync("/api/projects/", httpContentSecondProject);

        Assert.Matches("Conflict", createResponseSecondproject.StatusCode.ToString());
    }

    [Fact]
    public async Task CreateProject_ReturnCreated()
    {
        var httpContent = new StringContent(JsonSerializer.Serialize(
            new ProjectCreationDTO("TestProjectReturnCreated", "bobadgad", "Yadgadges", 1)), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/projects/", httpContent);

        Assert.Matches("Created", response.StatusCode.ToString());
    }

    /*[Fact]
    public async Task CreateProjectWithExistingNameForNewClient_ReturnsCreated()
    {
        var newClient = new ClientCreationDTO("NewClientToHoldNewProjects", "Joao", "Description Template", new byte[1]);
        var httpContentClient = new StringContent(JsonSerializer.Serialize(newClient), Encoding.UTF8, "application/json");
        var responseFromPost = await _httpClient.PostAsync("api/clients", httpContentClient);

        var getResponse = await _httpClient.GetAsync(responseFromPost.Headers.Location.AbsolutePath);

        var clientID = JObject.Parse(await getResponse.Content.ReadAsStringAsync()).GetValue("id");
        var id= Int32.Parse(clientID.ToString());

        ProjectCreationDTO projectCreationDTO = new("thisProjectExistsInClient", "Mr Contact", "Description Template", id);
        var httpContent = new StringContent(JsonSerializer.Serialize(projectCreationDTO), Encoding.UTF8, "application/json");
        var createResponse = await _httpClient.PostAsync("/api/projects/", httpContent);

        ProjectCreationDTO secondProjectCreationDTO = new("thisProjectExistsInClient", "Miss Contact", "Description Template", 1);
        var httpContentSecondProject = new StringContent(JsonSerializer.Serialize(secondProjectCreationDTO), Encoding.UTF8, "application/json");
        var createResponseSecondproject = await _httpClient.PostAsync("/api/projects/", httpContentSecondProject);

        Assert.Matches("Created", createResponseSecondproject.StatusCode.ToString());
    }*/
}
