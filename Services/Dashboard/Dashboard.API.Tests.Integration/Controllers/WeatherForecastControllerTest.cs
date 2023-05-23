using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace Dashboard.API.Tests.Integration.Controllers;

public class WeatherForecastControllerTest
{
   /* [Fact]
    public async Task Get_ReturnSuccessAndCorrectContentType()
    {
        //Arrange
        using var app = new TestApplicationFactory();
        var httpClient = app.CreateClient();

        //Act
        var response = await httpClient.GetAsync("/WeatherForecast");
        //var responseText = await response.Content.ReadAsStringAsync();
        //var result = JsonSerializer.Deserialize<WeatherForecast[]>(responseText);

        //Assert
        response.EnsureSuccessStatusCode();
        response!.Content!.Headers!.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
        //result!.Length.Should().Be(5);
    }*/
}