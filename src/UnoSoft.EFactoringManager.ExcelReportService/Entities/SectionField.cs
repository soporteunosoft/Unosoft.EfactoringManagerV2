using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.ExcelReportService.BL.Entities
{

    public class ReportSize
    {
        public double Height { get; set; }
        public double Width { get; set; }
    }

    public enum FieldType
    {
        TEXT = 1,
        FORMULA = 2,  
        MONEDA = 3,
        PORCENTAJE =4,
        INDEX = 5
    }

    public class SectionField : ReportStyle
    {
        [JsonProperty("Column")]
        public int Column { get; set; }

        [JsonProperty("Row")]
        public int Row { get; set; }

        [JsonProperty("Height")]
        public int Height { get; set; }

        [JsonProperty("Type")]
        public FieldType Type { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Format")]
        public string Format { get; set; }

        [JsonProperty("FieldName")]
        public string FieldName { get; set; }

        [JsonProperty("MergeRight")]
        public int MergeRight { get; set; }

        [JsonProperty("MergeDown")]
        public int MergeDown { get; set; }

        

     
    }
}
