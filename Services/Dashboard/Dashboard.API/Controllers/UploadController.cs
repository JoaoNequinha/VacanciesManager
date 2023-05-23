using Dashboard.Domain.Logic;
using Dashboard.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net;

namespace Dashboard.API.Controllers;

[ApiController]
[Route("api/uploadfile")]
public class UploadController : ControllerBase
{
    private readonly ILogger<UploadController> _logger;
    private readonly IExcelImportService _excelImportService;

    public UploadController(ILogger<UploadController> logger, IExcelImportService excelImportService)
    {
        _logger = logger;
        _excelImportService = excelImportService;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UploadFile()
    {
        Stream stream = null;
        
        try
        {
            stream = Request.Form.Files[0].OpenReadStream();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to read uploaded file " + ex.Message);
            return BadRequest("Unable to read file" + ex.Message);
        }

        try
        {
            await _excelImportService.RunExcelImport(stream);
        }
        catch (Exception ex)
        {

            _logger.LogError("Failed to import excel file " + ex.Message);
            return BadRequest("Failed to import excel file" + ex.Message);
        }

        stream.Dispose();

        return Ok($"File was uploaded.");
    }
}
