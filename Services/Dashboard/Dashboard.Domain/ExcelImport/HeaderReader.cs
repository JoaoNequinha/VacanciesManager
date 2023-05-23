using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.ExcelImport;

public class HeaderReader : IHeaderReader
{

    public Dictionary<string, int> GetHeaderMap(ISheet sheet)
    {
        Dictionary<string, int> headerMap = new Dictionary<string, int>();

        IRow headers = sheet.GetRow(0);
        for (int i = 0; i < headers.LastCellNum; i++)
        {
            ICell cell = headers.GetCell(i);
            if (cell.CellType != CellType.Blank)
            {
               
                if (!headerMap.ContainsKey(cell.StringCellValue))
                {
                    headerMap.Add(cell.StringCellValue, i);
                }
                else
                {
                    continue;
                }

            }
            else
            {
                break;
            }
        }
        return headerMap;
    }
}
