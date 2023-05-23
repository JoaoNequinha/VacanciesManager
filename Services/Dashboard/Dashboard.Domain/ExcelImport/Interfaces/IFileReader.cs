using NPOI.SS.UserModel;

namespace Dashboard.Domain.ExcelImport;

public interface IFileReader
{
    IWorkbook GetWorkBookFromStream(Stream fs);
    IWorkbook GetWorkBookFromFileStream(string path);
}
