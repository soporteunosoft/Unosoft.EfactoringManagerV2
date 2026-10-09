using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ExcelDataReader;

namespace UnoSoft.EFactoringManager.Services
{
    public class DocumentDataExtractor : CommonDocumentDataExtractor
    {
        private DocumentItemExtractor documentItemExtractor;

        public String CashAgreementCodeXpath { get; set; }
        public String FirstReferenceXpath { get; set; }
        public String SecondReferenceXpath { get; set; }
        public String AccountingValueXpath { get; set; }
        public String PendingBalanceXpath { get; set; }
        public String BillingReferenceXpath { get; set; }
        public String InvoiceReferenceIdXpath { get; set; }
        public String InvoiceReferenceDocumentTypeXpath { get; set; }
        public String InvoiceReferenceIssueDateXpath { get; set; }
        public String DebitNoteReferenceIdXpath { get; set; }
        public String DebitNoteReferenceDocumentTypeXpath { get; set; }
        public String DebitNoteReferenceIssueDateXpath { get; set; }
        public String CreditNoteReferenceIdXpath { get; set; }
        public String CreditNoteReferenceDocumentTypeXpath { get; set; }
        public String CreditNoteReferenceIssueDateXpath { get; set; }
        public String OrderReferenceXpath { get; set; }
        public String DespatchReferenceXpath { get; set; }
        public String ReceiptReferenceXpath { get; set; }
        public String ContractReferenceXpath { get; set; }
        public String OriginatorReferenceXpath { get; set; }
        public String AdditionalReferenceXpath { get; set; }
        public String InvoiceLineXpath { get; set; }
        public String InvoiceLineIdXpath { get; set; }
        public String DocumentLineReferenceXpath { get; set; }
        public String SpecificInvoiceLinePattern { get; set; }
        public String ReferenceLineXpath { get; set; }
        public String LineIdXpath { get; set; }
        public String InvoiceLineCtsXpath { get; set; }
        public String InvoiceLineIdCtsXpath { get; set; }
        public String LineIdCtsXpath { get; set; }
        public String DocumentLineReferenceCtsXpath { get; set; }
        public String DocumentReferenceIssueDateCtsXpath { get; set; }
        public String SpecificInvoiceLineCtsPattern { get; set; }
        public String ReceiptLineReferenceCtsXpath { get; set; }
        public String ItemLineXpath { get; set; }
        public String ItemLineIdXpath { get; set; }
        public String ItemLineQuantityXpath { get; set; }
        public String ItemLineDescriptionXpath { get; set; }
        public String ItemLineUnitCodeXpath { get; set; }
        public String ItemLinePriceAmountXpath { get; set; }
        public String ItemLineAmountXpath { get; set; }
        public String SenderNaturalFirstNameXpath { get; set; }
        public String SenderNaturalLastNameXpath { get; set; }
        public String ReceiverNaturalFirstNameXpath { get; set; }
        public String ReceiverNaturalLastNameXpath { get; set; }
        public String ReferenceDocumentTypeXpath { get; set; }
        public String ReferenceIssueDateXpath { get; set; }

        public DocumentDataExtractor()
        {
            IssueDateXpath = "cbc:IssueDate";
            IssueTimeXpath = "cbc:IssueTime";
            //CompanyIdXpath = "fe:AccountingSupplierParty/fe:Party/cac:PartyIdentification/cbc:ID";
            CompanyIdXpath = "cac:AccountingSupplierParty/cac:Party/cac:PartyTaxScheme/cbc:CompanyID";
            //ReceiverIdXpath = "fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID";
            ReceiverIdXpath = "cac:AccountingCustomerParty/cac:Party/cac:PartyTaxScheme/cbc:CompanyID";
            DescriptionXpath = "cac:Attachment/cac:ExternalReference/cbc:Description";
            DocumentIdXpath = "cbc:ID";
            CufeXpath = "cbc:UUID";
            CurrencyCodeXpath = "cbc:DocumentCurrencyCode";
            DocumentPrefixXpath = "ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sts:DianExtensions/sts:InvoiceControl/sts:AuthorizedInvoices/sts:Prefix";
            PaymentDueDateXpath = "cac:PaymentMeans/cbc:PaymentDueDate";
            //DueDateXpath = "ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/cts:AdditionalDocumentInformation/cts:Header/cts:DocumentDueDate";
            DueDateXpath = "cbc:DueDate";
            //FOR DO WIGELO:26/04/2023 REVISAR FECHA DE VENCIMIENTO LA ACTUAL QUEDA EN NULL
            //DueDateXpath = "cac:PaymentMeans/cbc:PaymentDueDate";
            SenderNameXpath = "fe:AccountingSupplierParty/fe:Party/fe:PartyLegalEntity/cbc:RegistrationName";
            SenderNaturalFirstNameXpath = "fe:AccountingSupplierParty/fe:Party/fe:Person/cbc:FirstName";
            SenderNaturalLastNameXpath = "fe:AccountingSupplierParty/fe:Party/fe:Person/cbc:FamilyName";
            ReceiverNameXpath = "cac:AccountingCustomerParty/cac:Party/cac:PartyTaxScheme/cbc:RegistrationName";
            ReceiverNaturalFirstNameXpath = "fe:AccountingCustomerParty/fe:Party/fe:Person/cbc:FirstName";
            ReceiverNaturalLastNameXpath = "fe:AccountingCustomerParty/fe:Party/fe:Person/cbc:FamilyName";
            //RecevierIdTypeXpath = "fe:AccountingCustomerParty/fe:Party/cac:PartyIdentification/cbc:ID/@schemeID";
            RecevierIdTypeXpath = "cac:AccountingCustomerParty/cac:Party/cac:PartyTaxScheme/cbc:CompanyID/@schemeID";
            ReceiverEmailXpath = "cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:ElectronicMail";

            //Luis
            ReceiverDirOficinaXpath = "cac:AccountingCustomerParty/cac:Party/cac:PhysicalLocation/cac:Address/cac:AddressLine/cbc:Line";
            ReceiverTelDomicilioXpath = "cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:Telephone";
            ReceiverCiudadXpath = "cac:AccountingCustomerParty/cac:Party/cac:PhysicalLocation/cac:Address/cbc:ID";

            //PayableAmountXpath = "fe:LegalMonetaryTotal/cbc:PayableAmount";
            PayableAmountXpath = "cac:LegalMonetaryTotal/cbc:PayableAmount";
            ExtensionAmountXpath = "cac:LegalMonetaryTotal/cbc:LineExtensionAmount";

            //TaxAmount01Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='01']/fe:TaxSubtotal/cbc:TaxAmount";
            TaxAmount01Xpath = "cac:TaxTotal[cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='01']/cac:TaxSubtotal/cbc:TaxAmount";
            TaxAmount02Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='02']/fe:TaxSubtotal/cbc:TaxAmount";
            TaxAmount03Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='03']/fe:TaxSubtotal/cbc:TaxAmount";
            //TaxAmount05Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='05']/fe:TaxSubtotal/cbc:TaxAmount";
            TaxAmount05Xpath = "cac:WithholdingTaxTotal[cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='05']/cac:TaxSubtotal/cbc:TaxAmount";
            // TaxAmount06Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='06']/fe:TaxSubtotal/cbc:TaxAmount";
            TaxAmount06Xpath = "cac:WithholdingTaxTotal[cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='06']/cac:TaxSubtotal/cbc:TaxAmount";
            //TaxAmount07Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='07']/fe:TaxSubtotal/cbc:TaxAmount";
            TaxAmount07Xpath = "cac:WithholdingTaxTotal[cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='07']/cac:TaxSubtotal/cbc:TaxAmount";

            //TaxPercent01Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='01']/fe:TaxSubtotal/cbc:Percent";
            TaxPercent01Xpath = "cac:TaxTotal[cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='01']/cac:TaxSubtotal/cac:TaxCategory/cbc:Percent";
            TaxPercent02Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='02']/fe:TaxSubtotal/cbc:Percent";
            TaxPercent03Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='03']/fe:TaxSubtotal/cbc:Percent";
            //TaxPercent05Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='05']/fe:TaxSubtotal/cbc:Percent";
            TaxPercent05Xpath = "cac:WithholdingTaxTotal[cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='05']/cac:TaxSubtotal/cac:TaxCategory/cbc:Percent";
            //TaxPercent06Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='06']/fe:TaxSubtotal/cbc:Percent";
            TaxPercent06Xpath = "cac:WithholdingTaxTotal[cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='06']/cac:TaxSubtotal/cac:TaxCategory/cbc:Percent";
            //TaxPercent07Xpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='07']/fe:TaxSubtotal/cbc:Percent";
            TaxPercent07Xpath = "cac:WithholdingTaxTotal[cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='07']/cac:TaxSubtotal/cac:TaxCategory/cbc:Percent";

            TemplateCodeXpath = "ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/cts:AdditionalDocumentInformation/cts:Header/cts:PDFTemplate";
            AlternativeTemplateCodeXpath = "cbc:Note";
            //alternativeTemplateCodePrefix>PXP-</invoice.config.alternativeTemplateCodePrefix>
            /*
            totalOtherTaxesXpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='9999']/cbc:TaxAmount";
            totalIscXpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='2000']/cbc:TaxAmount";
            totalIgvXpath = "fe:TaxTotal[fe:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID='1000']/cbc:TaxAmount";
            */

            InvoiceReferenceIdXpath = "cac:InvoiceDocumentReference/cbc:ID";
            InvoiceReferenceDocumentTypeXpath = "cac:InvoiceDocumentReference/cbc:DocumentTypeCode";
            InvoiceReferenceIssueDateXpath = "cac:InvoiceDocumentReference/cbc:IssueDate";
            DebitNoteReferenceIdXpath = "cac:DebitNoteDocumentReference/cbc:ID";
            DebitNoteReferenceDocumentTypeXpath = "cac:DebitNoteDocumentReference/cbc:DocumentTypeCode";
            DebitNoteReferenceIssueDateXpath = "cac:DebitNoteDocumentReference/cbc:IssueDate";
            CreditNoteReferenceIdXpath = "cac:CreditNoteDocumentReference/cbc:ID";
            CreditNoteReferenceDocumentTypeXpath = "cac:CreditNoteDocumentReference/cbc:DocumentTypeCode";
            CreditNoteReferenceIssueDateXpath = "cac:CreditNoteDocumentReference/cbc:IssueDate";
            OrderReferenceXpath = "cac:OrderReference";
            DespatchReferenceXpath = "cac:DespatchDocumentReference";
            ReceiptReferenceXpath = "cac:ReceiptDocumentReference";
            OriginatorReferenceXpath = "cac:OriginatorDocumentReference";
            ContractReferenceXpath = "cac:ContractDocumentReference";
            AdditionalReferenceXpath = "cac:AdditionalDocumentReference";
            InvoiceLineXpath = "fe:InvoiceLine";
            DocumentLineReferenceXpath = "cac:DocumentReference";
            InvoiceLineIdXpath = "cbc:ID";
            SpecificInvoiceLinePattern = "fe:InvoiceLine[cbc:ID='%s']";
            ReferenceLineXpath = "cac:ReceiptLineReference";
            LineIdXpath = "cbc:LineID";
            InvoiceLineCtsXpath = "ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/cts:AdditionalDocumentInformation/cts:Items/cts:Item";
            InvoiceLineIdCtsXpath = "cts:LineNumber";
            SpecificInvoiceLineCtsPattern = "ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/cts:AdditionalDocumentInformation/cts:Items/cts:Item[cts:LineNumber='%s']";
            LineIdCtsXpath = "cts:LineID";
            DocumentLineReferenceCtsXpath = "cts:DocumentReferenceID";
            DocumentReferenceIssueDateCtsXpath = "cts:DocumentReferenceIssueDate";

            //< !--ExtractorPaths INVOICE-- >
            ItemLineXpath = "fe:InvoiceLine";
            ItemLineIdXpath = "cbc:ID";
            ItemLineDescriptionXpath = "fe:Item/cbc:Description";
            ItemLinePriceAmountXpath = "fe:Price/cbc:PriceAmount";

            /*   //< !--ExtractorPaths NOTES-- >

                < notes.config.dueDateXpath = "cac:PaymentMeans/cbc:PaymentDueDate</notes.config.dueDateXpath ="
                < notes.config.senderNaturalFirstNameXpath = "fe:AccountingCustomerParty/fe:Party/fe:Person/cbc:FirstName</notes.config.senderNaturalFirstNameXpath ="
                < notes.config.senderNaturalLastNameXpath = "fe:AccountingCustomerParty/fe:Party/fe:Person/cbc:FamilyName</notes.config.senderNaturalLastNameXpath ="

                < !--ExtractorPaths DEBITNOTE-- >

                 < debitnote.config.itemLineXpath = "cac:DebitNoteLine</debitnote.config.itemLineXpath ="
                 < debitnote.config.itemLineQuantityXpath = "cbc:DebitedQuantity</debitnote.config.itemLineQuantityXpath ="
                 < debitnote.config.itemLineUnitCodeXpath = "cbc:DebitedQuantity/@unitCode</debitnote.config.itemLineUnitCodeXpath ="

                 < !--ExtractorPaths CREDITNOTE-- >

                  < creditnote.config.itemLineXpath = "cac:CreditNoteLine</creditnote.config.itemLineXpath ="
                  < creditnote.config.itemLineQuantityXpath = "cbc:CreditedQuantity</creditnote.config.itemLineQuantityXpath ="
                  < creditnote.config.itemLineUnitCodeXpath = "cbc:CreditedQuantity/@unitCode</creditnote.config.itemLineUnitCodeXpath ="
                  */
            documentItemExtractor = new DocumentItemExtractor();
        }

        public override DocumentHeader Extract(byte[] xml)
        {
            DocumentHeader documentHeader = null;

            try
            {

                //EntityDocumentType entityDocumentType = EntityDocumentType.getByLabel(documentType);
                XmlDocument xmlDocument = base.GetDocumentFromXml(xml);
                String xmlRootName = xmlUtils.GetRootXpath(xmlDocument);


                documentHeader = base.Extract(xml);
                //documentHeader.setType(entityDocumentType.getLabel());

                //DocumentDataColombia documentDataColombia = 
                documentHeader.DocumentDataColombia = GetColombiaData(documentHeader.DocumentId, xmlDocument, xmlRootName);

                // Ajustar sender-name según logica colombia
                //documentHeader.DocumentData.SenderName = (getConditional(base.senderNameXpath, senderNaturalFirstNameXpath, senderNaturalLastNameXpath, xmlDocument, xmlRootName));
                //documentHeader.getDocumentData().setReceiverName(getConditional(base.receiverNameXpath, receiverNaturalFirstNameXpath, receiverNaturalLastNameXpath, xmlDocument, xmlRootName));

                documentHeader.ReferenceDocuments = GetReferences(documentHeader.DocumentId, xmlDocument, xmlRootName);

                //List<DocumentItem> documentItems = getItems(documentHeader.getId(), xmlDocument, xmlRootName);
                documentHeader.DocumentItems = GetItems(documentHeader.DocumentId, xmlDocument, xmlRootName);
                DocumentHeader documentHeader1 = documentItemExtractor.Extract(xml);
                documentHeader.DocumentItems = documentHeader1.DocumentItems;

            }
            catch (Exception)
            {
                throw;
            }

            return documentHeader;
        }


        /*  private void createXPathEnvironment() throws DocumentExtractorException
          {
              Map<String, String> xmlns = new HashMap<>();
          xmlns.put("tns", "http://www.dian.gov.co/contratos/facturaelectronica/v1");
          xmlns.put("fe", "http://www.dian.gov.co/contratos/facturaelectronica/v1");
          xmlns.put("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
          xmlns.put("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
          xmlns.put("ccts", "urn:un:unece:uncefact:documentation:2");
          xmlns.put("ds", "http://www.w3.org/2000/09/xmldsig#");
          xmlns.put("ebz", "urn:carvajal:names:specification:ubl:ebz:schema:xsd:EBZExtensionContent-1");
          xmlns.put("cts", "urn:carvajal:names:specification:ubl:colombia:schema:xsd:CarvajalAggregateComponents-1");
          xmlns.put("sts", "http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures");
          xmlns.put("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
          xmlns.put("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
          xmlns.put("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
          xmlns.put("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
          xmlns.put("xsi", "http://www.w3.org/2001/XMLSchema-instance");
          xmlns.put("xades", "http://uri.etsi.org/01903/v1.3.2#");
          xpathEnvironment = xmlUtils.createXPathEvaluationEnvironment(xmlns);
      }*/

        private DocumentDataColombia GetColombiaData(String documentId, XmlDocument xmlDocument, String xmlRootName)
        {
            DocumentDataColombia documentDataColombia = new DocumentDataColombia();

            try
            {
                documentDataColombia.DocumentId = documentId;
                if (CashAgreementCodeXpath != null)
                {
                    String cashAgreementCode = base.GetStringValue(CashAgreementCodeXpath, xmlDocument, xmlRootName);
                    if (!string.IsNullOrEmpty(cashAgreementCode))
                    {
                        documentDataColombia.CashAgreementCode = cashAgreementCode;
                    }

                    if (FirstReferenceXpath != null)
                    {
                        documentDataColombia.FirstReference = base.GetStringValue(FirstReferenceXpath, xmlDocument, xmlRootName);
                    }
                    if (SecondReferenceXpath != null)
                    {
                        documentDataColombia.SecondReference = base.GetStringValue(SecondReferenceXpath, xmlDocument, xmlRootName);
                    }
                }

                // documentDataColombia.PaymentStatus = PaymentStatusType.PENDING_PAID.getName());
                documentDataColombia.AccountingValue = base.GetDecimalValue(AccountingValueXpath, xmlDocument, xmlRootName);
                // documentDataColombia.PendingBalance = base.getDecimalValue(PendingBalanceXpath, xmlDocument, xmlRootName);
            }
            catch (Exception)
            {
                throw;
            }

            return documentDataColombia;
        }

        private List<ReferenceDocument> GetReferences(String documentId, XmlDocument xmlDocument, String xmlRootName)
        {
            List<ReferenceDocument> references = new List<ReferenceDocument>();

            try
            {
                references.AddRange(GetDocumentHeaderReferences(xmlDocument, xmlRootName));
                references.AddRange(GetDocumentDetailReferences(xmlDocument, xmlRootName));
            }
            catch (Exception ex)
            {
                throw new Exception("Error en generación referencias documento: " + documentId + " :" + ex.Message);
            }

            return references;
        }

        private List<ReferenceDocument> GetDocumentHeaderReferences(XmlDocument xmlDocument, String xmlRootName)
        {
            List<ReferenceDocument> referencesInHeader = new List<ReferenceDocument>();

            XmlNodeList billingReferenceNodes = base.GetNodeLists(BillingReferenceXpath, xmlDocument, xmlRootName);

            if (billingReferenceNodes != null && billingReferenceNodes.Count > 0)
            {
                foreach (XmlNode node in billingReferenceNodes)
                {
                    //for (int i = 0; i<billingReferenceNodes.Count; i++) {
                    //XmlNode node = billingReferenceNodes.item(i);
                    String invoiceReferenceNode = base.GetStringNodeValue(InvoiceReferenceIdXpath, node);
                    if (!string.IsNullOrEmpty(invoiceReferenceNode)) //!invoiceReferenceNode.isEmpty() && invoiceReferenceNode.length() > 0 && invoiceReferenceNode != null)
                    {
                        String number = invoiceReferenceNode;
                        String docTypeNodeValue = base.GetStringNodeValue(InvoiceReferenceDocumentTypeXpath, node);
                        //ReferenceDocumentType referenceDocumentType = ReferenceDocumentType.getByLabel(docTypeNodeValue);
                        var referenceDocumentType = "INVOICE";

                        ReferenceDocument reference = new ReferenceDocument()
                        {
                            DocumentType = referenceDocumentType,
                            DocumentReferenceId = number,
                            ReferencePosition = "HEAD"
                        };
                        String referenceIssueDate = GetStringNodeValue(InvoiceReferenceIssueDateXpath, node);
                        if (!String.IsNullOrEmpty(referenceIssueDate))
                        {
                            DateTime referenceDate = DateTime.Today;
                            DateTime.TryParse(referenceIssueDate, out referenceDate);
                            reference.Date = referenceDate;
                        }
                        referencesInHeader.Add(reference);
                    }

                    String debitNoteReferenceNode = GetStringNodeValue(DebitNoteReferenceIdXpath, node);
                    if (!String.IsNullOrEmpty(debitNoteReferenceNode))
                    {
                        String number = debitNoteReferenceNode;
                        String docTypeNodeValue = base.GetStringNodeValue(DebitNoteReferenceDocumentTypeXpath, node);

                        var referenceDocumentType = docTypeNodeValue;


                        ReferenceDocument reference = new ReferenceDocument()
                        {
                            DocumentType = docTypeNodeValue,
                            DocumentReferenceId = number,
                            ReferencePosition = "HEAD"
                        };

                        String referenceIssueDate = GetStringNodeValue(DebitNoteReferenceIssueDateXpath, node);
                        if (!String.IsNullOrEmpty(referenceIssueDate))
                        {
                            DateTime referenceDate = DateTime.Now;
                            //referenceDate = dateUtils.convertUblDateStringToDate(referenceIssueDate);
                            reference.Date = referenceDate;
                        }
                        referencesInHeader.Add(reference);
                    }

                    String creditNoteReferenceNode = base.GetStringNodeValue(CreditNoteReferenceIdXpath, node);
                    if (!String.IsNullOrEmpty(creditNoteReferenceNode))
                    {
                        String number = creditNoteReferenceNode;
                        String docTypeNodeValue = base.GetStringNodeValue(CreditNoteReferenceDocumentTypeXpath, node);

                        ReferenceDocument reference = new ReferenceDocument()
                        {
                            DocumentType = docTypeNodeValue,
                            DocumentReferenceId = number,
                            ReferencePosition = "HEAD"
                        };

                        String referenceIssueDate = base.GetStringNodeValue(CreditNoteReferenceIssueDateXpath, node);
                        if (!String.IsNullOrEmpty(referenceIssueDate))
                        {
                            DateTime referenceDate = DateTime.Now;
                            //referenceDate = dateUtils.convertUblDateStringToDate(referenceIssueDate);
                            reference.Date = referenceDate;
                        }
                        referencesInHeader.Add(reference);
                    }
                }
            }

            XmlNodeList orderReferenceNodes = base.GetNodeLists(OrderReferenceXpath, xmlDocument, xmlRootName);
            if (orderReferenceNodes != null && orderReferenceNodes.Count > 0)
            {
                foreach (XmlNode node in orderReferenceNodes)
                {


                    String orderReferenceId = base.GetStringNodeValue(base.DocumentIdXpath, node);
                    if (!String.IsNullOrEmpty(orderReferenceId))
                    {
                        String number = orderReferenceId;

                        //ReferenceDocumentType referenceDocumentType = ReferenceDocumentType.ORDERS;

                        ReferenceDocument reference = new ReferenceDocument()
                        {
                            DocumentType = "ReferenceDocumentType.ORDERS",
                            DocumentReferenceId = number,
                            ReferencePosition = "HEAD"
                        };

                        String referenceIssueDate = base.GetStringNodeValue(base.IssueDateXpath, node);


                        if (!String.IsNullOrEmpty(referenceIssueDate))
                        {
                            DateTime referenceDate = DateTime.Now;
                            //referenceDate = dateUtils.convertUblDateStringToDate(referenceIssueDate);
                            reference.Date = referenceDate;
                        }
                        referencesInHeader.Add(reference);
                    }
                }
            }

            List<String> referenceXPaths = new List<String>();
            referenceXPaths.Add(DespatchReferenceXpath);
            referenceXPaths.Add(ReceiptReferenceXpath);
            referenceXPaths.Add(OriginatorReferenceXpath);
            referenceXPaths.Add(ContractReferenceXpath);
            referenceXPaths.Add(AdditionalReferenceXpath);

            foreach (String referenceXpath in referenceXPaths)
            {
                XmlNodeList referenceNodes = base.GetNodeLists(referenceXpath, xmlDocument, xmlRootName);
                if (referenceNodes != null && referenceNodes.Count > 0)
                {
                    foreach (XmlNode node in referenceNodes)
                    {

                        String idNodeValue = base.GetStringNodeValue(base.DocumentIdXpath, node);
                        String number = idNodeValue;
                        String docTypeNodeValue = base.GetStringNodeValue(ReferenceDocumentTypeXpath, node);

                        ReferenceDocument reference = new ReferenceDocument()
                        {
                            DocumentType = docTypeNodeValue,
                            DocumentReferenceId = number,
                            ReferencePosition = "HEAD",
                        };

                        String referenceIssueDate = base.GetStringNodeValue(ReferenceIssueDateXpath, node);
                        if (!String.IsNullOrEmpty(referenceIssueDate))
                        {
                            DateTime referenceDate = DateTime.Now;
                            DateTime.TryParse(referenceIssueDate, out referenceDate);
                            reference.Date = referenceDate;
                        }

                        referencesInHeader.Add(reference);
                    }
                }
            }

            return referencesInHeader;
        }

        private List<ReferenceDocument> GetDocumentDetailReferences(XmlDocument xmlDocument, String xmlRootName)
        {
            List<ReferenceDocument> referencesInDetail = new List<ReferenceDocument>();

            // ITEM REFERENCES
            XmlNodeList itemBillingReferenceNodes = base.GetNodeLists(InvoiceLineXpath + "/" + BillingReferenceXpath, xmlDocument, xmlRootName);
            // Nodo para obtener el identificador del item de referencia
            XmlNodeList itemInvoiceNodes = base.GetNodeLists(InvoiceLineXpath, xmlDocument, xmlRootName);

            if (itemInvoiceNodes != null && itemInvoiceNodes.Count > 0)
            {
                foreach (XmlNode nodeInvoice in itemInvoiceNodes)
                {
                    // INVOICE_LINE_ID
                    // = itemInvoiceNodes.item(h);
                    String invoiceIdNode = base.GetStringNodeValue(InvoiceLineIdXpath, nodeInvoice);
                    if (itemBillingReferenceNodes != null && itemBillingReferenceNodes.Count > 0)
                    {
                        foreach (XmlNode node in itemBillingReferenceNodes)
                        {

                            String itemInvoiceReferenceNode = base.GetStringNodeValue(InvoiceReferenceIdXpath, node);
                            if (!String.IsNullOrEmpty(itemInvoiceReferenceNode))
                            {

                                String number = itemInvoiceReferenceNode;

                                String docTypeNodeValue = base.GetStringNodeValue(InvoiceReferenceDocumentTypeXpath, node);

                                /*ReferenceDocumentType referenceDocumentType = ReferenceDocumentType.getByLabel(docTypeNodeValue);
                                if (referenceDocumentType == null)
                                {
                                    referenceDocumentType = ReferenceDocumentType.INVOICE;
                                }*/

                                ReferenceDocument reference = new ReferenceDocument()
                                {
                                    DocumentType = docTypeNodeValue,
                                    DocumentReferenceId = number,
                                    ItemId = invoiceIdNode,
                                    ReferencePosition = "LIN"
                                };

                                String referenceIssueDate = base.GetStringNodeValue(InvoiceReferenceIssueDateXpath, node);


                                if (!String.IsNullOrEmpty(referenceIssueDate))
                                {
                                    DateTime referenceDate = DateTime.Today;
                                    DateTime.TryParse(referenceIssueDate, out referenceDate);
                                    reference.Date = referenceDate;
                                }

                                referencesInDetail.Add(reference);
                            }
                        }
                    }
                }
            }

            List<String> referenceLineXPaths = new List<String>();
            referenceLineXPaths.Add(DespatchReferenceXpath);
            referenceLineXPaths.Add(DocumentLineReferenceXpath);

            foreach (String referenceLin in referenceLineXPaths)
            {
                if (itemInvoiceNodes != null && itemInvoiceNodes.Count > 0)
                {
                    List<String> itemIds = new List<String>();
                    foreach (XmlNode nodeInvoice in itemInvoiceNodes)
                    {
                        String invoiceIdNode = base.GetStringNodeValue(InvoiceLineIdXpath, nodeInvoice);
                        // aseguramos que solo vaya por una linea si esta se repite en el documento
                        if (!itemIds.Contains(invoiceIdNode))
                        {
                            itemIds.Add(invoiceIdNode);

                            XmlNodeList referenceLineNodes = base.GetNodeLists(String.Format(SpecificInvoiceLinePattern, invoiceIdNode) + "/" + referenceLin, xmlDocument, xmlRootName);
                            if (referenceLineNodes != null && referenceLineNodes.Count > 0)
                            {
                                foreach (XmlNode node in referenceLineNodes)
                                {

                                    String idNodeValue = base.GetStringNodeValue(base.DocumentIdXpath, node);
                                    String number = idNodeValue;
                                    String docTypeNodeValue = base.GetStringNodeValue(base.DocumentTypeXpath, node);

                                    /*ReferenceDocumentType referenceDocumentType = ReferenceDocumentType.getByLabel(docTypeNodeValue);
                                    if (referenceDocumentType == null)
                                    {
                                        referenceDocumentType = ReferenceDocumentType.INVOICE;
                                    }*/


                                    ReferenceDocument reference = new ReferenceDocument()
                                    {
                                        DocumentType = docTypeNodeValue,
                                        DocumentReferenceId = number,
                                        ItemId = invoiceIdNode,
                                        ReferencePosition = "LIN"
                                    };

                                    String referenceIssueDate = base.GetStringNodeValue(InvoiceReferenceIssueDateXpath, node);


                                    if (!String.IsNullOrEmpty(referenceIssueDate))
                                    {
                                        DateTime referenceDate = DateTime.Today;
                                        DateTime.TryParse(referenceIssueDate, out referenceDate);
                                        reference.Date = referenceDate;
                                    }
                                    referencesInDetail.Add(reference);
                                }
                            }
                        }
                    }
                }
            }

            // RECADV REFERENCE IN LINE LEVEL
            XmlNodeList detailReferenceNodes = base.GetNodeLists(InvoiceLineXpath, xmlDocument, xmlRootName);
            if (detailReferenceNodes != null && detailReferenceNodes.Count > 0)
            {
                List<String> itemIds = new List<String>();
                foreach (XmlNode detailNode in detailReferenceNodes)
                {

                    String lineNumber = base.GetStringNodeValue(base.DocumentIdXpath, detailNode);
                    if (!itemIds.Contains(lineNumber))
                    {
                        itemIds.Add(lineNumber);
                        XmlNodeList referenceLineNodes = base.GetNodeLists(String.Format(SpecificInvoiceLinePattern, lineNumber) + "/" + ReferenceLineXpath, xmlDocument, xmlRootName);
                        if (referenceLineNodes != null && referenceLineNodes.Count > 0)
                        {
                            foreach (XmlNode node in referenceLineNodes)
                            {

                                String lineNumberInRecadvReference = base.GetStringNodeValue(LineIdXpath, node);
                                String number = base.GetStringNodeValue(DocumentLineReferenceXpath + "/" + base.DocumentIdXpath, node);

                                //ReferenceDocumentType referenceDocumentType = ReferenceDocumentType.RECADV;

                                ReferenceDocument reference = new ReferenceDocument()
                                {
                                    DocumentType = "ReferenceDocumentType.RECADV",
                                    DocumentReferenceId = number,
                                    ItemId = lineNumberInRecadvReference,
                                    ReferencePosition = "LIN"
                                };

                                //String referenceIssueDate = base.GetStringNodeValue(InvoiceReferenceIssueDateXpath, node);
                                String referenceIssueDate = base.GetStringNodeValue(DocumentLineReferenceXpath + "/" + InvoiceReferenceIssueDateXpath, node);

                                if (!String.IsNullOrEmpty(referenceIssueDate))
                                {
                                    DateTime referenceDate = DateTime.Today;
                                    DateTime.TryParse(referenceIssueDate, out referenceDate);
                                    reference.Date = referenceDate;
                                }
                                referencesInDetail.Add(reference);
                            }
                        }
                    }
                }
            }

            // RECADV REFERENCE IN CTS EXTENSION
            XmlNodeList detailReferenceInExtensionNodes = base.GetNodeLists(InvoiceLineCtsXpath, xmlDocument, xmlRootName);
            if (detailReferenceInExtensionNodes != null && detailReferenceInExtensionNodes.Count > 0)
            {
                List<String> itemIds = new List<String>();
                foreach (XmlNode detailNode in detailReferenceInExtensionNodes)
                {

                    String lineNumber = base.GetStringNodeValue(InvoiceLineIdCtsXpath, detailNode);
                    if (!itemIds.Contains(lineNumber))
                    {
                        itemIds.Add(lineNumber);
                        XmlNodeList referenceLineNodes = base.GetNodeLists(String.Format(SpecificInvoiceLineCtsPattern, lineNumber) + "/" + ReceiptLineReferenceCtsXpath, xmlDocument, xmlRootName);
                        if (referenceLineNodes != null && referenceLineNodes.Count > 0)
                        {
                            foreach (XmlNode node in referenceLineNodes)
                            {
                                String lineNumberInRecadvReference = base.GetStringNodeValue(LineIdCtsXpath, node);
                                String number = base.GetStringNodeValue(DocumentLineReferenceCtsXpath, node);
                                //ReferenceDocumentType referenceDocumentType = ReferenceDocumentType.RECADV;


                                ReferenceDocument reference = new ReferenceDocument()
                                {
                                    DocumentType = "ReferenceDocumentType.RECADV",
                                    DocumentReferenceId = number,
                                    ItemId = lineNumberInRecadvReference,
                                    ReferencePosition = "LIN"
                                };



                                String referenceIssueDate = base.GetStringNodeValue(DocumentReferenceIssueDateCtsXpath, node);
                                if (!String.IsNullOrEmpty(referenceIssueDate))
                                {
                                    DateTime referenceDate = DateTime.Today;
                                    DateTime.TryParse(referenceIssueDate, out referenceDate);
                                    reference.Date = referenceDate;
                                }

                                referencesInDetail.Add(reference);
                            }
                        }
                    }
                }
            }

            return referencesInDetail;
        }

        private List<DocumentItem> GetItems(String documentId, XmlDocument xmlDocument, String xmlRootName)
        {
            List<DocumentItem> invoiceItems = new List<DocumentItem>();

            try
            {
                XmlNodeList nodes = base.GetNodeLists(ItemLineXpath, xmlDocument, xmlRootName);
                if (nodes != null && nodes.Count > 0)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        XmlNode node = nodes.Item(i);

                        String intemIdValue = base.GetStringNodeValue(ItemLineIdXpath, node);
                        String itemNumber = string.IsNullOrEmpty(intemIdValue) ? intemIdValue : (i + 1).ToString();
                        String quantityStringValue = base.GetStringNodeValue(ItemLineQuantityXpath, node);
                        decimal itemQuantity = 0;
                        decimal.TryParse(quantityStringValue, out itemQuantity);

                        String itemDescription = base.GetStringNodeValue(ItemLineDescriptionXpath, node);
                        if (string.IsNullOrEmpty(itemDescription))
                        {
                            itemDescription = "-";
                        }

                        String unitCode = base.GetStringNodeValue(ItemLineUnitCodeXpath, node);
                        if (string.IsNullOrEmpty(unitCode))
                        {
                            unitCode = "-";
                        }

                        String priceAmountStringValue = base.GetStringNodeValue(ItemLinePriceAmountXpath, node);
                        decimal priceAmount = 0;
                        decimal.TryParse(priceAmountStringValue, out priceAmount);

                        String totalValueStringValue = base.GetStringNodeValue(ItemLineAmountXpath, node);
                        decimal totalValue = 0;
                        decimal.TryParse(totalValueStringValue, out totalValue);



                        DocumentItem invoiceItem = new DocumentItem()
                        {
                            ItemId = itemNumber,
                            Quantity = itemQuantity,
                            Description = itemDescription,
                            DocumentId = documentId,
                            UnitMeasure = unitCode,
                            TotalValue = totalValue,
                            UnitValue = priceAmount
                        };
                        invoiceItems.Add(invoiceItem);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error en generación items documento: " + documentId + " :" + ex.Message);
            }

            return invoiceItems;
        }

        public DocumentHeader ExtractFromXls(byte[] xlsBytes)
        {
            using (Stream stream = new MemoryStream(xlsBytes))
            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
            {
                DataSet dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = false }
                });

                if (dataSet.Tables.Count == 0)
                    throw new Exception("El archivo Excel no contiene hojas.");

                DataTable table = dataSet.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    if (row.ItemArray.Length < 2)
                        continue;

                    string xmlContent = row[1]?.ToString()?.Trim();

                    if (string.IsNullOrEmpty(xmlContent) || !xmlContent.StartsWith("<"))
                        continue;

                    return Extract(Encoding.UTF8.GetBytes(xmlContent));
                }

                throw new Exception("No se encontró contenido XML válido en el archivo Excel.");
            }
        }

        private String GetConditional(String nameXpath, String firstNameXpath, String secondNameXpath, XmlDocument xmlDocument, String xmlRootName)
        {
            String senderName = base.GetStringValue(nameXpath, xmlDocument, xmlRootName);
            if (string.IsNullOrEmpty(senderName))
            {
                StringBuilder nameBuilder = new StringBuilder();
                nameBuilder.Append(base.GetStringValue(firstNameXpath, xmlDocument, xmlRootName)).Append(" ").Append(base.GetStringValue(secondNameXpath, xmlDocument, xmlRootName));
                senderName = nameBuilder.ToString();
            }
            return senderName;
        }


    }
}
