
using Dashboard.Domain.ExcelImport;

namespace Dashboard.Domain.Services;

public class ExcelImportService : IExcelImportService
{
    private readonly IExcelImportFlow _excelImportFlow;

    public ExcelImportService(IExcelImportFlow excelImportFlow)
    {
        _excelImportFlow = excelImportFlow;
    }

    public  async Task RunExcelImport(Stream stream)
    {
        await _excelImportFlow.RunImport(stream);

    }
}
