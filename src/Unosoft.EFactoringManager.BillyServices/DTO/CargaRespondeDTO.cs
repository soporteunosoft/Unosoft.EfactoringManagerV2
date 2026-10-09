using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.BillyServices.DTO
{
    public class CargaRespondeDTO
    {
        public string Description { get; set; }
        public List<Error> Errors { get; set; }
     
    }


    public class EncontrarFacturaResponseDTO
    {
        public Data Data { get; set; }
    }

    public class Error
    {
        public string Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }

    public class Data
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public Attributes Attributes { get; set; }
    }

    public class Attributes
    {
        public string AcceptanceType { get; set; }
        public string AcknowledgementDate { get; set; }
        public string Cufe { get; set; }
        public List<Event> Events { get; set; }
        public string InvoiceNumber { get; set; }
        public string HolderId { get; set; }
        public string HolderName { get; set; }
        public string HolderIdNumber { get; set; }
        public string IssueDate { get; set; }
        public string IssuerId { get; set; }
        public string IssuerName { get; set; }
        public string IssuerIdNumber { get; set; }
        public List<string> Links { get; set; }
        public string Status { get; set; }
        public bool Credit { get; set; }
        public string Currency { get; set; }
        public string Expiration { get; set; }
        public string PayerId { get; set; }
        public string PayerName { get; set; }
        public string PayerIdNumber { get; set; }
        public double GrossValue { get; set; }
        public int TaxDiscounts { get; set; }
        public int OtherDiscounts { get; set; }
        public double TaxValue { get; set; }
        public double NetPayableValue { get; set; }
    }

    public class Event
    {
        public string Code { get; set; }
        public List<Detail> Details { get; set; }
        public List<string> Links { get; set; }
    }

    public class Detail
    {
        public string Description { get; set; }
        public long Timestamp { get; set; }
    }
}
