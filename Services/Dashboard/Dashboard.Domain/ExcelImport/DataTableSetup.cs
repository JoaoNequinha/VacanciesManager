using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.ExcelImport;

public class DataTableSetup : IDataTableSetup
{


    public DataTableSetup()
    {
    }

    public DataTable SetupVacancyDatatable()
    {
        DataTable ExceldataTable = new DataTable();

        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.AllowDBNull = false;
        column.Caption = "Vacancy name";
        column.ColumnName = "name";

        DataColumn column1 = new DataColumn();
        column1.DataType = System.Type.GetType("System.String");
        column1.AllowDBNull = true;
        column1.Caption = "Skills";
        column1.ColumnName = "skill";

        DataColumn column2 = new DataColumn();
        column2.DataType = System.Type.GetType("System.String");
        column2.AllowDBNull = true;
        column2.Caption = "Location";
        column2.ColumnName = "location";
        //column2.DefaultValue = null;

        DataColumn column3 = new DataColumn();
        column3.DataType = System.Type.GetType("System.DateTime");
        column3.AllowDBNull = true;
        column3.Caption = "Start Date";
        column3.ColumnName = "target_start_date";

        DataColumn column4 = new DataColumn();
        column4.DataType = System.Type.GetType("System.String");
        column4.AllowDBNull = false;
        column4.Caption = "Project Name";
        column4.ColumnName = "project_name";

        DataColumn column5 = new DataColumn();
        column5.DataType = System.Type.GetType("System.String");
        column5.AllowDBNull = false;
        column5.Caption = "Client Name";
        column5.ColumnName = "client_name";

        DataColumn column6 = new DataColumn();
        column6.DataType = System.Type.GetType("System.String");
        column6.AllowDBNull = false;
        column6.Caption = "Status";
        column6.ColumnName = "is_open";
        column6.DefaultValue = "Available";

        DataColumn column7 = new DataColumn();
        column7.DataType = System.Type.GetType("System.Int32");
        column7.AllowDBNull = true;
        column7.Caption = "Count";
        column7.ColumnName = "vacancy_count";

        ExceldataTable.Columns.Add(column5);
        ExceldataTable.Columns.Add(column4);
        ExceldataTable.Columns.Add(column1);
        ExceldataTable.Columns.Add(column3);
        ExceldataTable.Columns.Add(column);
        ExceldataTable.Columns.Add(column2);
        ExceldataTable.Columns.Add(column6);
        ExceldataTable.Columns.Add(column7);

        return ExceldataTable;
    }
}


