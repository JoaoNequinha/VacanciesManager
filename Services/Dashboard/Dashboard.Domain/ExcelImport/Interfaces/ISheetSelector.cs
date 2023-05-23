using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Dashboard.Domain.ExcelImport;

public interface ISheetSelector
{
    ISheet SelectSheet(IWorkbook wb, string sheetName);
}
