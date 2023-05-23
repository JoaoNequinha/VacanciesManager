using System.Threading.Tasks;
using System.Net.Http;
using Xunit;
using FluentAssertions;
using System.Text.Json;
using Dashboard.API.DTO;
using System.Linq;

namespace Dashboard.API.Tests.Integration.Controllers;

public class VacancyControllerTests: IClassFixture<TestApplicationFixture>
{
    private readonly HttpClient _httpClient;

    public VacancyControllerTests(TestApplicationFixture fixture)
    {
        _httpClient = fixture.HttpClient;
    }

    [Fact]
    public async Task GetVacancies_ReturnSuccessAndAllVacancies()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/vacancies");

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");

        string content = await response.Content.ReadAsStringAsync();

        var vacancyDTOs = JsonSerializer.Deserialize<VacancyDTO[]>(content);

        response.EnsureSuccessStatusCode();
        Assert.IsType<VacancyDTO[]>(vacancyDTOs);
    }

    [Fact]
    public async Task GetVacanciesWithProjectIdQuery_ReturnSuccessAndAllVacancies()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/vacancies?project_id=1");

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");

        string content = await response.Content.ReadAsStringAsync();

        var vacancyDTOs = JsonSerializer.Deserialize<VacancyDTO[]>(content);

        response.EnsureSuccessStatusCode();
        Assert.IsType<VacancyDTO[]>(vacancyDTOs);
    }

    [Fact]
    public async Task GetVacancy_ReturnFailedAndErrorCode()
    {
        var response = await _httpClient.GetAsync("/api/vacancies/-1");
        Assert.Matches("NotFound", response.StatusCode.ToString());
    }

   /* [Fact]
    public async Task GetVacancy_ReturnSuccessAndCorrectContentType()
    {
        var response = await _httpClient.GetAsync("/api/vacancies/1");

        response.EnsureSuccessStatusCode();
        response!.Content!.Headers!.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
    }*/
}
