using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.ExcelImport;

public class VacancySelector : IVacancySelector
{
    //TODO in future implementations this method will have to be fed indexes as argument instead of having them hardcoded in the logic  
    public DataTable GetVacancies(ISheet sheet, DataTable allVacanciesTable)
    {

        for (int i = 0; i <= sheet.LastRowNum; i++)
        {

            IRow row = sheet.GetRow(i);

            if (!row.Cells.All(d => d.CellType == CellType.Blank))
            {

                if (row.GetCell(2).StringCellValue.Equals("SGZ UK")
                    && row.GetCell(14).StringCellValue.Equals("UK")
                    && (row.GetCell(33,MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue.Length == 0))
                {

                    DataRow Tablerow = allVacanciesTable.NewRow();
                    Tablerow[0] = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue;
                    Tablerow[1] = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue;
                    Tablerow[2] = row.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue;
                    Tablerow[3] = row.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).DateCellValue;
                    Tablerow[4] = row.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue;
                    Tablerow[5] = row.GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue;

                    allVacanciesTable.Rows.Add(Tablerow);

                }
            }
        }


        for (int i = 0; i < allVacanciesTable.Rows.Count; i++)
        {
            if (allVacanciesTable.Rows[i][5].Equals(""))
            {
                allVacanciesTable.Rows[i][5] = null;
                allVacanciesTable.AcceptChanges();
            }
           
        }
        allVacanciesTable.AcceptChanges();

        return allVacanciesTable;

    }
}
