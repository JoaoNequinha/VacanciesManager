using NPOI.SS.UserModel;
using System.Data;

namespace Dashboard.Domain.ExcelImport;

public interface IVacancySelector
{
    DataTable GetVacancies(ISheet sheet, DataTable allVacanciesTable);
}
