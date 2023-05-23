using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.ExcelImport;

public class SheetSelector : ISheetSelector
{

    public ISheet SelectSheet(IWorkbook wb,string sheetName)
    {

        ISheet sheet = wb.GetSheet(sheetName);

        return sheet;
    }
}
