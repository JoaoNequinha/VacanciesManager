using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.ExcelImport;

public class FileReader : IFileReader
{

    public IWorkbook GetWorkBookFromStream(Stream fs)
    {
        IWorkbook wb = new XSSFWorkbook(fs);
        return wb;
    }

    public IWorkbook GetWorkBookFromFileStream(string path)
    {

        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); ;
        IWorkbook wb = new XSSFWorkbook(fs);
        return wb;
    }
}


