using Dashboard.Domain.Models;

namespace Dashboard.Domain.ExcelImport
{
    public interface IExcelDataBaseImporter
    {
         Task ImportToDataBase(List<Vacancy> list);
    }
}