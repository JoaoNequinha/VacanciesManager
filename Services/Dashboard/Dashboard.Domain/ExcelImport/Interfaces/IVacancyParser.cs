
using Dashboard.Domain.Models;
using System.Data;

namespace Dashboard.Domain.ExcelImport;

public interface IVacancyParser
{
    List<Vacancy> ParseIntoVacancies(DataTable allVacanciestable);
}
