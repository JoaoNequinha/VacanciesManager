using FluentAssertions;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Dashboard.API.Tests.Integration.Controllers;

public class UploadControllerTests : IClassFixture<TestApplicationFixture>
{
    private readonly HttpClient _httpClient;

    public UploadControllerTests(TestApplicationFixture fixture)
    {
        _httpClient = fixture.HttpClient;
    }


    [Fact]
    public async Task UploadSpeadSheetSuccess_ReturnSuccessCode()
    {
        var httpContent = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(File.ReadAllBytes("../../../ExcelSample/sample.xlsx"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        httpContent.Add(fileContent, "file", "sample");
        
        var response = await _httpClient.PostAsync("/api/uploadfile", httpContent);

        response.EnsureSuccessStatusCode();
    }


    [Fact]
    public async Task InvalidUploadSpeadSheet_ReturnBadRequest()
    {
        var httpContent = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(File.ReadAllBytes("../../../ExcelSample/InvalidSample.xlsx"));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        httpContent.Add(fileContent, "file", "sample");

        var response = await _httpClient.PostAsync("/api/uploadfile", httpContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
