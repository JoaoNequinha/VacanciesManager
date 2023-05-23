using System.Data;

namespace Dashboard.Domain.ExcelImport;

public interface IDataTableSetup
{
    DataTable SetupVacancyDatatable();
}
