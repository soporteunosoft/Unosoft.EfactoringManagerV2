using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;

namespace UnoSoft.EFactoringManager.Services
{
    public abstract class BusinessDocumentExtractorUBL
    {
        //public XPath xpathEnvironment;

        // Estas propiedades deben ser inyectadas por spring
        public String DocumentType { get; set; }
        public String Country { get; set; }
        public String CompanyIdXpath { get; set; }
        public String ReceiverIdXpath { get; set; }
        public String DocumentIdXpath { get; set; }
        public String DocumentTypeXpath { get; set; }
        public String DocumentPrefixXpath { get; set; }
        public String IssueTimeXpath { get; set; }
        public String IssueDateXpath { get; set; }
        public String DueDateXpath { get; set; }
        public String CtsDueDateXpath { get; set; }
        public String ReceiverNameXpath { get; set; }
        public String DescriptionXpath { get; set; }
        public String CurrencyCodeXpath { get; set; }
        public String PaymentDueDateXpath { get; set; }

        public XmlUtils xmlUtils;
        //public DateUtils dateUtils;
        //public DocumentIdSpliterFactory documentIdSpliterFactory;


        public BusinessDocumentExtractorUBL()
        {
            xmlUtils = new XmlUtils();
        }
        public virtual DocumentHeader Extract(byte[] xml)
        {
            try
            {
                XmlDocument xmlDocument = GetDocumentFromXml(xml);
                String xmlRootName = xmlUtils.GetRootXpath(xmlDocument);
                return GetHeader(DocumentType, xmlDocument, xmlRootName);
            }
            catch (Exception)
            {
                throw;
            }
        }


        protected String GetStringValue(String xpath, XmlDocument xmlDocument, String xmlRootName)
        {
            return xmlUtils.GetSingleNodeText(xmlRootName, xpath, xmlDocument);
        }

        protected decimal GetDecimalValue(String xpath, XmlDocument xmlDocument, String xmlRootName)
        {
            String value = GetStringValue(xpath, xmlDocument, xmlRootName);
            var cultureInfo = new CultureInfo("es-CO");
            //if(value=="")
            //{
            //    return -1;
            //}
            //else {
            //decimal decimalValue = 0;
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue);
            return decimalValue;
            //}

        }

        protected String GetStringValueFromMultiplePaths(String xpathValues, String conditionalXpath, String conditionValue, String valueXpath, XmlDocument xmlDocument, String xmlRootName)
        {
            XmlNodeList referenceNodes = GetNodeLists(xpathValues, xmlDocument, xmlRootName);
            if (referenceNodes != null && referenceNodes.Count > 0)
            {
                for (int i = 0; i < referenceNodes.Count; i++)
                {
                    XmlNode node = referenceNodes.Item(i);
                    String codeValue = xmlUtils.GetSingleNodeText(conditionalXpath, node);
                    if (codeValue != null && codeValue == conditionValue)
                    {
                        return xmlUtils.GetSingleNodeText(valueXpath, node);
                    }
                }
            }
            return null;
        }

        protected String GetConditionalValue(String xPath1, String xPath2, XmlDocument xmlDocument, String xmlRootName)
        {
            String dueDate = xmlUtils.GetSingleNodeText(xmlRootName, xPath1, xmlDocument);
            if (String.IsNullOrEmpty(dueDate))
            {
                dueDate = xmlUtils.GetSingleNodeText(xmlRootName, xPath2, xmlDocument);
            }
            return dueDate;
        }

        protected XmlNodeList GetNodeLists(String xPath, XmlDocument xmlDocument, String xmlRootName)
        {

            try
            {
                return xmlUtils.GetAllNodes(xmlRootName, xPath, xmlDocument);
            }
            catch (Exception)
            {
                //LOGGER.error("Error al obtener cadena desde multiples path", ex);
            }
            return null;
        }

        protected String GetStringNodeValue(String xpath, XmlNode xmlNode)
        {
            return xmlUtils.GetSingleNodeText(xpath, xmlNode);
        }

        private DocumentHeader GetHeader(String entityDocumentType, XmlDocument xmlDocument, String xmlRootName)
        {
            DocumentHeader documentHeader = new DocumentHeader()
            {

                DocumentType = entityDocumentType,
                DocumentId = GetStringValue(DocumentIdXpath, xmlDocument, xmlRootName),
                DocumentNumberPrefix = GetStringValue(DocumentPrefixXpath, xmlDocument, xmlRootName),
                SenderIdentification = GetStringValue(CompanyIdXpath, xmlDocument, xmlRootName),
                ReceiverIdentification = GetStringValue(ReceiverIdXpath, xmlDocument, xmlRootName),
                DescriptionDocument = GetStringValue(DescriptionXpath, xmlDocument, xmlRootName),
                ReceiverName = GetStringValue(ReceiverNameXpath, xmlDocument, xmlRootName),
                CurrencyType = GetStringValue(CurrencyCodeXpath, xmlDocument, xmlRootName),
                CreationDate = new DateTime()


            };

            //var xmlHeaderDocument = xmlUtils.ParseXmlDocument(documentHeader.DescriptionDocument);
            //var xmlHeaderRootName = xmlUtils.GetRootXpath(xmlHeaderDocument);

            //documentHeader.ReceiverNumber(getStringValue(receiverIdXpath, xmlDocument, xmlRootName));

            String documentHeaderId = GetStringValue(DocumentIdXpath, xmlDocument, xmlRootName);
            String prefix = GetStringValue(DocumentPrefixXpath, xmlDocument, xmlRootName);
            string dueDate = GetStringValue(PaymentDueDateXpath, xmlDocument, xmlRootName);
            //String prefix = GetStringValue(DocumentPrefixXpath, xmlHeaderDocument, xmlHeaderRootName);
            //string dueDate = GetStringValue(PaymentDueDateXpath, xmlHeaderDocument, xmlHeaderRootName);

            
            //DocumentIdSpliter documentHeaderIdSpliter = documentIdSpliterFactory.getDocumentIdSpliter(country);
            
       
            documentHeader.DocumentNumberPrefix = prefix;
      
            //documentHeader.setNumber(documentHeaderIdSpliter.getNumber(documentHeaderId));

            //documentHeader.setCreationDate(new Date());

            //FOR DO: OBTENER LA FECHA DE VENCIMEINTO WIGELO:26/04/2023
            //String dueDate = GetConditionalValue(DueDateXpath, CtsDueDateXpath, xmlDocument, xmlRootName);

            //String dueDate =   GetStringValue(DueDateXpath, xmlDocument, xmlRootName);

            if (String.IsNullOrEmpty(dueDate))
            {
                documentHeader.DueDate = DateTime.Now; // (dateUtils.convertUblDateStringToDate(dueDate));
            }
            else
            {
                documentHeader.DueDate = DateTime.Parse(dueDate);
            }


            String time = GetStringValue(IssueTimeXpath, xmlDocument, xmlRootName).Trim();
            time = time.Split('-').First();
            time = time.Split('+').First();

            String issueDate = GetStringValue(IssueDateXpath, xmlDocument, xmlRootName);


            if (String.IsNullOrEmpty(time))
            {
                documentHeader.IssueDate = DateTime.ParseExact(issueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                issueDate = issueDate + ' ' + time;
                int index = issueDate.IndexOf(".");
                if (index > 0)
                    issueDate = issueDate.Substring(0, index);

                documentHeader.IssueDate = DateTime.ParseExact(issueDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }


            return documentHeader;
        }

        protected XmlDocument GetDocumentFromXml(byte[] xml)
        {

            return xmlUtils.ParseXmlDocument(xml);
        }
    }
}
