using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.Services
{
    public class ReferenceDocument
    {
        public String ReferenceId { get; set; }
        public String DocumentId { get; set; }
        public String ItemId { get; set; }
        public List<String> DocumentItems { get; set; }
        public String DocumentReferenceId { get; set; }
        public DateTime Date { get; set; }
        public String DocumentType { get; set; }
        public String ReferencePosition { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
