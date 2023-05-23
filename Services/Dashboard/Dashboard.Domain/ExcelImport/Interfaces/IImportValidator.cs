using NPOI.SS.UserModel;

namespace Dashboard.Domain.ExcelImport;

public interface IImportValidator
{
    bool ColumnsExist(Dictionary<string, int> headerMap);
    bool SheetExists(string name, IWorkbook wb);
}
