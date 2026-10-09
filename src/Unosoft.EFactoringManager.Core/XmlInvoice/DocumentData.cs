using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.Services
{
    public class Tax
    {
        public decimal Amount { get; set; } = 0;
        public decimal Percent { get; set; } = 0;
    }

    public class DocumentData
    {
        public String DataId { get; set; }
        public String SenderName { get; set; }
        public String ReceiverIdentificationType { get; set; }
        public String ReceiverName { get; set; }
        public String ReceiverEmail { get; set; }
        public String ReceiverDirOficina { get; set; }//Luis
        public String ReceiverTelDomicilio { get; set; }//Luis
        public String ReceiverCiudad { get; set; }//Luis
        public decimal TotalValue { get; set; }
        public decimal ExtensionValue { get; set; }
        public String TemplateCode { get; set; }
        public String DocumentId { get; set; }
        public String PdfStorageId { get; set; }
        public String SignedStorageId { get; set; }
        public String AttachedStorageId { get; set; }
        public String EntityResultStorageId { get; set; }
        public String OriginalStoredId { get; set; }
        public String GovFileName { get; set; }
        public String Cufe { get; set; }
        public DateTime SigningDate { get; set; }

        public Tax ValTax { get; set; } = new Tax();
        public Tax ValOtherTax { get; set; } = new Tax();
        public Tax ValTaxIca { get; set; } = new Tax();
        public Tax ValTaxReteFuente { get; set; } = new Tax();
        public Tax ValTaxReteICA { get; set; } = new Tax();
        public Tax ValTaxReteIVA { get; set; } = new Tax();

        public String InternalNumber { get; set; }

    }
}
