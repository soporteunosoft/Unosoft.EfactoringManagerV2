using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.ExcelReportService.Utils
{
    public static  class DataSetConvert
    {


        public static List<dynamic> ConvertToDynamicList(DataTable dataTable)
        {
            var dynamicList = new List<dynamic>();

            foreach (DataRow row in dataTable.Rows)
            {
                dynamic expandoObject = new ExpandoObject();
                //var dict = (IDictionary<string, object>)expandoObject;
                var dict = expandoObject as IDictionary<string, object>;

                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }

                dynamicList.Add(expandoObject);
            }

            return dynamicList;
        }

    }
}
