using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.ExcelImport;


//TODO: require the vacancy repository from Scott's branch
public class ExcelImportFlow : IExcelImportFlow
{
    private readonly IFileReader _fileReader;
    private readonly ISheetSelector _sheetSelector;
    private readonly IHeaderReader _headerReader;
    private readonly IDataTableSetup _datatableSetup;
    private readonly IImportValidator _importValidator;
    private readonly IVacancySelector _vacancySelector;
    private readonly IVacancyParser _vacancyParser;
    private readonly IExcelDataBaseImporter _excelDataBaseImporter;

    public ExcelImportFlow(IFileReader fileReader, ISheetSelector sheetSelector, IHeaderReader headerReader, IDataTableSetup datatableSetup,
        IImportValidator importValidator, IVacancySelector vacancySelector, IVacancyParser vacancyParser, IExcelDataBaseImporter excelDataBaseImporter)
    {
        _fileReader = fileReader;
        _sheetSelector = sheetSelector;
        _headerReader = headerReader;
        _datatableSetup = datatableSetup;
        _importValidator = importValidator;
        _vacancySelector = vacancySelector;
        _vacancyParser = vacancyParser;
        _excelDataBaseImporter = excelDataBaseImporter;
    }
    
    public async Task RunImport(Stream stream)
    {
        var wb = _fileReader.GetWorkBookFromStream(stream);

        var vacancyDataTable = _datatableSetup.SetupVacancyDatatable();

        if (_importValidator.SheetExists("Demands Basedata", wb))
        {
            var sheet = _sheetSelector.SelectSheet(wb, "Demands Basedata");

            var headerMap = _headerReader.GetHeaderMap(sheet);

            if (_importValidator.ColumnsExist(headerMap))
            {
                var selectedVacancies = _vacancySelector.GetVacancies(sheet, vacancyDataTable);
                var listOfVacancyEntitites = _vacancyParser.ParseIntoVacancies(selectedVacancies);
                await _excelDataBaseImporter.ImportToDataBase(listOfVacancyEntitites);
            }
        }
    }
}


