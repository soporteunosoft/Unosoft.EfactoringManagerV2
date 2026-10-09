using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.Services
{
    public class DocumentHeader
    {
        public String DocumentId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime DocumentDate { get; set; }
        //fec vencimiento DueDate
        public DateTime DueDate { get; set; }
        public DateTime IssueDate { get; set; }
        public String LegalState { get; set; }
        public String BusinessState { get; set; }
        public String SummaryState { get; set; }
        public String DocumentType { get; set; }
        public String SenderIdentification { get; set; } 
        public String ProviderID { get; set; }
        public String ReceiverIdentification { get; set; }
        public String DescriptionDocument { get; set; }
        public String ReceiverName { get; set; }
        public String DocumentNumberPrefix { get; set; }
        public String DocumentNumber { get; set; }
        public String CurrencyType { get; set; }
        public DocumentData DocumentData { get; set; }
        public List<ReferenceDocument> ReferenceDocuments { get; set; }
        public List<DocumentItem> DocumentItems { get; set; }
        public DeliveryGovernment DeliveryGovernment { get; set; }
  
        public String TechnicalKey { get; set; }
        public Dictionary<String, Object> AddititionalInfo { get; set; }
        public DocumentDataColombia DocumentDataColombia { get; set; }
        public String FactoringState { get; set; }
       
        public PaymentRecord Payment { get; set; }
    }
}
