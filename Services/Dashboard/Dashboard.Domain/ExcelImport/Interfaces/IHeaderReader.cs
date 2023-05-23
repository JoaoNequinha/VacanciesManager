using NPOI.SS.UserModel;

namespace Dashboard.Domain.ExcelImport;

public interface IHeaderReader
{
    Dictionary<string, int> GetHeaderMap(ISheet sheet);
}
