using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using NPOI.SS.UserModel;


namespace Dashboard.Domain.ExcelImport;
public class ImportValidator : IImportValidator
{

    public bool SheetExists(string name, IWorkbook wb)
    {
        return (wb.GetSheet(name) == null) ? false : true;
    }

    public bool ColumnsExist(Dictionary<string, int> headerMap)
    {
        return ((headerMap.ContainsKey(VacancyHeaders.CandidateName)
            && headerMap.ContainsKey(VacancyHeaders.Source)) == true) ? true : false;
    }
}




