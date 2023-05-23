using Dashboard.API.Controllers;
using Dashboard.API.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dashboard.API.Tests.Unit;

public class WeatherForecastControllerTest
{
    private readonly ILogger<WeatherForecastController> _logger = Substitute.For<ILogger<WeatherForecastController>>();

    [Fact]
    public void Get_Return5Forecasts()
    {
        // Arrange
        var controller = new WeatherForecastController(_logger);

        // Act
        List<WeatherForecast> forecasts = controller.Get().ToList();

        // Assert
        forecasts.Count.Should().Be(5);
        //TODO: verify _logger
    }
}