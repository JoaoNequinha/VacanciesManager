
using Dashboard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.ExcelImport;

public class VacancyParser : IVacancyParser
{
    public List<Vacancy> ParseIntoVacancies(DataTable allVacanciestable)
    {

        List<Vacancy> importList = new List<Vacancy>();

        var query = allVacanciestable.AsEnumerable()
             .GroupBy(row => new
             {
                 client_name = row.Field<string>("client_name"),
                 project_name = row.Field<string>("project_name"),
                 skill = row.Field<string>("skill"),
                 target_start_date = row.Field<DateTime>("target_start_date"),
                 name = row.Field<string>("name"),
                 location = row.Field<string>("location"),
                 is_open = row.Field<string>("is_open")
             })
             .Select(grp => new
             {

                 client_name = grp.Key.client_name,
                 project_name = grp.Key.project_name,
                 skill = grp.Key.skill,
                 target_start_date = grp.Key.target_start_date,
                 name = grp.Key.name,
                 location = grp.Key.location,
                 is_open = grp.Key.is_open,
                 vacancy_count = grp.Count()

             });



        foreach (var item in query)
        {

            Vacancy vacancy = new Vacancy(
                            item.name,
                            item.skill,
                            item.location,
                            item.target_start_date.ToString("dd/M/yyyy"),
                            item.vacancy_count,
                            item.is_open,
                            item.project_name,
                            item.client_name
                                        );

            importList.Add(vacancy);

        }

        return importList;

    }
}
