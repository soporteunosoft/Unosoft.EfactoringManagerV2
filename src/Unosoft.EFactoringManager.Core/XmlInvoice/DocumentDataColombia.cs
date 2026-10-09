using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.Services
{
    public class DocumentDataColombia
    {
        public String DocumentId { get; set; }
        public String AcceptanceStoredId { get; set; }
        public String AppResponseStoredId { get; set; }
        public String ReferenceFileStoredId { get; set; }
        public String CashAgreementCode { get; set; }
        public String FirstReference { get; set; }
        public String SecondReference { get; set; }
        public decimal AccountingValue { get; set; }  // Valor Neto
        public decimal PendingBalance { get; set; }   // Saldo Pendiente
        public String PaymentStatus { get; set; }
        public String VoucherBookletStoredId { get; set; }
    }
}
