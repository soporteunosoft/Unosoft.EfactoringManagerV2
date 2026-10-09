using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

namespace UnoSoft.EFactoringManager.Services
{
    public class DocumentItemExtractor
    {
        public DocumentHeader Extract(byte[] contentFile)
        {


            //XMLInputFactory factory = XMLInputFactory.newInstance();
            DocumentHeader document = new DocumentHeader();
            document.DocumentItems = new List<DocumentItem>();
            Stream documentInputStream = new MemoryStream(contentFile);

            try
            {
                XmlTextReader reader = new XmlTextReader(EscapeXMLValue(documentInputStream));

                DocumentItem item = null;

                while (reader.Read())
                {
                    String qName = "";
                    switch (reader.NodeType)
                    {

                        case XmlNodeType.Element:
                            //qName = event.asStartElement().getName().getLocalPart();
                            qName = reader.Name;
                            if (qName.ToLower() == "invoiceline")
                            {
                                item = new DocumentItem();
                            }
                            break;
                        case XmlNodeType.Text:
                            Console.Write(reader.Value);
                            break;
                        case XmlNodeType.CDATA:
                            Console.Write("<![CDATA[{0}]]>", reader.Value);
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            Console.Write("<?{0} {1}?>", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.Comment:
                            Console.Write("<!--{0}-->", reader.Value);
                            break;
                        case XmlNodeType.XmlDeclaration:
                            Console.Write("<?xml version='1.0'?>");
                            break;
                        case XmlNodeType.Document:
                            break;
                        case XmlNodeType.DocumentType:
                            Console.Write("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.EntityReference:
                            Console.Write(reader.Name);
                            break;
                        case XmlNodeType.EndElement:
                            Console.Write("</{0}>", reader.Name);
                            break;
                    }
                }
                /*
                while (reader.hasNext())
                {
                    XMLEvent event = eventReader.nextEvent();
        String qName ="";
                switch(event.getEventType()) {
                    case XMLStreamConstants.START_ELEMENT :
                        qName = event.asStartElement().getName().getLocalPart();
                        if (qName.equalsIgnoreCase("invoiceline")) {
            item = new DocumentItem();
        }
                        if (item != null) {
            if (qName.equalsIgnoreCase("ID"))
            {
                id = true;
            }
            else
               if (qName.equalsIgnoreCase("Description"))
            {
                description = true;
            }
        }                         
                        break;
                        
                    case XMLStreamConstants.CHARACTERS :
                        Characters characters = event.asCharacters();
                        if (item != null) {
            if (id)
            {
                item.setItemId(characters.getData());
                id = false;
            }
            else if (description)
            {
                item.setDescription(characters.getData());
                description = false;
            }
        }
                        break;
                        
                    case XMLStreamConstants.END_ELEMENT :
                        EndElement endElement = event.asEndElement();
                        if (endElement.getName().getLocalPart().equalsIgnoreCase("invoiceline")) {
            document.getDocumentItems().add(item);
            item = null;
        }
                        
                        break;

        }

    }*/
                return document;

            }
            catch (Exception)
            {
                throw;
            }

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
    }
}
