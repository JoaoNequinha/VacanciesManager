
namespace Dashboard.Domain.ExcelImport
{
    public interface IExcelImportFlow
    {
        Task RunImport(Stream stream);
    }
}