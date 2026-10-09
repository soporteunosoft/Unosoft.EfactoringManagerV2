using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.Services
{
    public class DocumentItem
    {
        public String Line { get; set; }
        public String ItemId { get; set; }
        public String DocumentId { get; set; }
        public String Description { get; set; }
        public decimal Quantity { get; set; }
        public String UnitMeasure { get; set; }
        public decimal UnitValue { get; set; }
        public decimal TotalValue { get; set; }
    }
}
