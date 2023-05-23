using Dashboard.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Dashboard.API.Tests.Integration;

public class TestApplicationFixture : IDisposable
{
    private readonly TestApplicationFactory _factory = new();
    public HttpClient HttpClient { get; }

    public TestApplicationFixture()
    {
        HttpClient = _factory.CreateClient();
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        _factory.Dispose();
    }
       
}
