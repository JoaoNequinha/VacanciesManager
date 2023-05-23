
namespace Dashboard.Domain.Services
{
    public interface IExcelImportService
    {
        Task RunExcelImport(Stream stream);
    }
}