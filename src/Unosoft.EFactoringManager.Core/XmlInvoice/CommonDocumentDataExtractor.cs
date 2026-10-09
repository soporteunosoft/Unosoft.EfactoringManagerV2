using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UnoSoft.EFactoringManager.Services
{
    public class CommonDocumentDataExtractor : BusinessDocumentExtractorUBL
    {

        public String SenderNameXpath { get; set; }
        public new String ReceiverNameXpath { get; set; }
        public String RecevierIdTypeXpath { get; set; }
        public String ReceiverEmailXpath { get; set; }
        public String ReceiverDirOficinaXpath { get; set; } //Luis
        public String ReceiverTelDomicilioXpath { get; set; } //Luis
        public String ReceiverCiudadXpath { get; set; } //Luis
        public String InternalNumberXpathValues { get; set; }
        public String ConditionalInternalNumberXpath { get; set; }
        public String ConditionalValueInternalNumber { get; set; }
        public String ValueInternalNumberXpath { get; set; }

        public String ProviderIDXpath { get; set; }

        public String CufeXpath { get; set; }

        public String TaxAmount01Xpath { get; set; }
        public String TaxAmount02Xpath { get; set; }
        public String TaxAmount03Xpath { get; set; }
        public String TaxAmount05Xpath { get; set; }
        public String TaxAmount06Xpath { get; set; }
        public String TaxAmount07Xpath { get; set; }

        public String TaxPercent01Xpath { get; set; }
        public String TaxPercent02Xpath { get; set; }
        public String TaxPercent03Xpath { get; set; }
        public String TaxPercent05Xpath { get; set; }
        public String TaxPercent06Xpath { get; set; }
        public String TaxPercent07Xpath { get; set; }

        public String PayableAmountXpath { get; set; }
        public String ExtensionAmountXpath { get; set; }

        public String TemplateCodeXpath { get; set; }
        public String AlternativeTemplateCodeXpath { get; set; }
        public String AlternativeTemplateCodePrefix { get; set; }


        public override DocumentHeader Extract(byte[] xml)
        {
            try
            {
                XmlDocument xmlDocument = GetDocumentFromXml(xml);
                String xmlRootName = xmlUtils.GetRootXpath(xmlDocument);
                DocumentHeader documentHeader = base.Extract(xml);

                DocumentData documentData = GetData(documentHeader.DocumentId, xmlDocument, xmlRootName);
                documentHeader.DocumentData = documentData;

                return documentHeader;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DocumentData GetData(String documentId, XmlDocument xmlDocument, String xmlRootName)
        {
            DocumentData documentData = new DocumentData();

            documentData.DocumentId = documentId;
            documentData.Cufe = GetStringValue(CufeXpath, xmlDocument, xmlRootName);

            documentData.SenderName = GetStringValue(SenderNameXpath, xmlDocument, xmlRootName);
            documentData.ReceiverName = GetStringValue(ReceiverNameXpath, xmlDocument, xmlRootName);
            documentData.ReceiverIdentificationType = GetStringValue(RecevierIdTypeXpath, xmlDocument, xmlRootName);
            documentData.ReceiverEmail = GetStringValue(ReceiverEmailXpath, xmlDocument, xmlRootName);

            //Luis
            documentData.ReceiverDirOficina = GetStringValue(ReceiverDirOficinaXpath, xmlDocument, xmlRootName);
            documentData.ReceiverTelDomicilio = GetStringValue(ReceiverTelDomicilioXpath, xmlDocument, xmlRootName);
            documentData.ReceiverCiudad = GetStringValue(ReceiverCiudadXpath, xmlDocument, xmlRootName);

            documentData.TotalValue = GetDecimalValue(PayableAmountXpath, xmlDocument, xmlRootName);
            documentData.ExtensionValue = GetDecimalValue(ExtensionAmountXpath, xmlDocument, xmlRootName);
            documentData.TemplateCode = ""; // getPdfTemplate(xmlDocument, xmlRootName));

            documentData.ValTax.Amount = GetDecimalValue(TaxAmount01Xpath, xmlDocument, xmlRootName);
            documentData.ValTax.Percent = GetDecimalValue(TaxPercent01Xpath, xmlDocument, xmlRootName);

            documentData.ValOtherTax.Amount = GetDecimalValue(TaxAmount02Xpath, xmlDocument, xmlRootName);
            documentData.ValOtherTax.Percent = GetDecimalValue(TaxPercent02Xpath, xmlDocument, xmlRootName);

            documentData.ValTaxIca.Amount = GetDecimalValue(TaxAmount03Xpath, xmlDocument, xmlRootName);
            documentData.ValTaxIca.Percent = GetDecimalValue(TaxPercent03Xpath, xmlDocument, xmlRootName);

            documentData.ValTaxReteFuente.Amount = GetDecimalValue(TaxAmount06Xpath, xmlDocument, xmlRootName);
            documentData.ValTaxReteFuente.Percent = GetDecimalValue(TaxPercent06Xpath, xmlDocument, xmlRootName);

            documentData.ValTaxReteICA.Amount = GetDecimalValue(TaxAmount07Xpath, xmlDocument, xmlRootName);
            documentData.ValTaxReteICA.Percent = GetDecimalValue(TaxPercent07Xpath, xmlDocument, xmlRootName);

            documentData.ValTaxReteIVA.Amount = GetDecimalValue(TaxAmount05Xpath, xmlDocument, xmlRootName);
            documentData.ValTaxReteIVA.Percent = GetDecimalValue(TaxPercent05Xpath, xmlDocument, xmlRootName);

            if (InternalNumberXpathValues != null)
            {
                documentData.InternalNumber = GetStringValueFromMultiplePaths(InternalNumberXpathValues, ConditionalInternalNumberXpath, ConditionalValueInternalNumber,
                        ValueInternalNumberXpath, xmlDocument, xmlRootName);
            }

            return documentData;
        }
    }
}
