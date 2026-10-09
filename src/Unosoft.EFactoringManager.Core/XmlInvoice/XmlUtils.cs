using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Text.RegularExpressions;

namespace UnoSoft.EFactoringManager.Services
{
    public class XmlUtils
    {

        public XmlDocument ParseXmlDocument(Stream documentInputStream)
        {
            XmlTextReader reader = new XmlTextReader(documentInputStream);

            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            return doc;
        }

        public XmlDocument ParseXmlDocument(string xml)
        {
            xml = xml.Replace("&", "&amp;");

            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            xml = Regex.Replace(xml, re, "");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }

        public XmlDocument ParseXmlDocument(byte[] xml)
        {
            Stream stream = new MemoryStream(xml);

            XmlDocument document = ParseXmlDocument(EscapeXMLValue(stream));
            return document;
        }
        public static Stream EscapeXMLValue(Stream documentInputStream)
        {
            StreamReader readerXml = new StreamReader(documentInputStream);
            string xmlString = readerXml.ReadToEnd();

            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

            xmlString = xmlString.Replace("&", "&amp;");

            string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            xmlString = Regex.Replace(xmlString, re, "");

            MemoryStream mStrm = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            return mStrm;
        }


        /*
         *  XPathExpression expression1 = XPathExpression.Compile(".//bk:price/text()*10"); // Returns a number.
        XPathExpression expression2 = XPathExpression.Compile("bk:bookstore/bk:book/bk:price"); // Returns a nodeset.

        XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("bk", "http://www.contoso.com/books");

        expression1.SetContext(manager);
        expression2.SetContext(manager);
        */
        public XmlNodeList GetAllNodes(String rootXpath, String nodeXpath, XmlDocument xmlDocument)
        {
            StringBuilder fullNodeXpath = new StringBuilder();
            if (nodeXpath != null)
            {
                if (nodeXpath.StartsWith("/"))
                {
                    fullNodeXpath.Append(rootXpath).Append(nodeXpath);
                }
                else
                {
                    fullNodeXpath.Append(rootXpath).Append("/").Append(nodeXpath);
                }
            }
            XmlNode root = xmlDocument.DocumentElement;


            XmlNodeList nodeList = root.SelectNodes(nodeXpath, GetNameSpaceManager(xmlDocument));
            /* XPathExpression expr = XPathExpression.Compile(fullNodeXpath.ToString());
             Object result = expr.evaluate(xmlDocument, XPathConstants.NODESET);

             XmlNodeList nodeList = (XmlNodeList)result;
             */
            return nodeList;  //xmlDocument.GetElementsByTagName(nodeXpath);
        }

        public XmlNodeList GetAllNodes(String nodeXpath, XmlNode xmlNode)
        {
            return xmlNode.SelectNodes(nodeXpath);
        }

        public String GetSingleNodeText(String rootXpath, String nodeXpath, XmlDocument xmlDocument)
        {
            try
            {
                XmlNodeList nodeList = GetAllNodes(rootXpath, nodeXpath, xmlDocument);
                return (nodeList.Count > 0) ? nodeList[0].InnerText : string.Empty;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public String GetSingleNodeText(String nodeXpath, XmlNode xmlNode)
        {
            try
            {
                XmlNodeList nodeList = GetAllNodes(nodeXpath, xmlNode);
                return (nodeList.Count > 0) ? nodeList.Item(0).InnerText : string.Empty;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public String GetRootXpath(XmlDocument xmlDocument)
        {
            XmlElement root = null;

            XmlNodeList list = xmlDocument.ChildNodes;
            foreach (XmlNode node in list)
            {
                if (node is XmlElement)
                {
                    root = (XmlElement)node;
                    break;
                }
            }

            StringBuilder rootXpath = new StringBuilder("");
            if (root != null)
            {
                rootXpath.Append(
                        String.Format("/*[local-name()='{0:%s}' and namespace-uri()='{1:%s}']",
                                root.LocalName, root.NamespaceURI));
            }
            return rootXpath.ToString();
        }


        private XmlNamespaceManager GetNameSpaceManager(XmlDocument document)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("bk", "urn:newbooks-schema");
            nsmgr.AddNamespace("tns", "http://www.dian.gov.co/contratos/facturaelectronica/v1");
            nsmgr.AddNamespace("fe", "http://www.dian.gov.co/contratos/facturaelectronica/v1");
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            nsmgr.AddNamespace("ccts", "urn:un:unece:uncefact:documentation:2");
            nsmgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            nsmgr.AddNamespace("ebz", "urn:carvajal:names:specification:ubl:ebz:schema:xsd:EBZExtensionContent-1");
            nsmgr.AddNamespace("cts", "urn:carvajal:names:specification:ubl:colombia:schema:xsd:CarvajalAggregateComponents-1");
            nsmgr.AddNamespace("sts", "http://www.dian.gov.co/contratos/facturaelectronica/v1/Structures");
            nsmgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            nsmgr.AddNamespace("qdt", "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2");
            nsmgr.AddNamespace("sac", "urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1");
            nsmgr.AddNamespace("udt", "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2");
            nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            nsmgr.AddNamespace("xades", "http://uri.etsi.org/01903/v1.3.2#");
            return nsmgr;
        }
    }
}
