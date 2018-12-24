using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace XmlHelper
{
    class XmlHelper
    {
        public static string XmlFilePath { get; set; }

        static XmlHelper()
        {
            XmlFilePath = "database.xml";
        }
        public static string Read(string strGroupName, string strName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodes;
            XmlElement xmlSubEle;

            string filePath = XmlFilePath;
            if (File.Exists(filePath) == false)
            {
                xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?> <database> <table> </table> </database>");
                xmlDoc.Save(filePath);
            }
            xmlDoc.Load(filePath);

            xmlNodes = xmlDoc.GetElementsByTagName(strGroupName);
            foreach (XmlNode xmlNode in xmlNodes)
            {
                foreach (XmlAttribute xmlAttr in xmlNode.Attributes)
                {
                    if (xmlAttr.Name == strName)
                    {
                        return xmlAttr.Value;
                    }
                }

                XmlAttribute xmlAttr1 = xmlDoc.CreateAttribute(strName);
                xmlAttr1.Value = "";
                xmlNode.Attributes.Append(xmlAttr1);
                xmlDoc.Save(filePath);
                return "";
            }

            xmlNodes = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement xmlEle in xmlNodes)
            {
                if (xmlEle.Name.ToLower() == "table")
                {
                    xmlSubEle = xmlDoc.CreateElement(strGroupName);
                    xmlEle.AppendChild(xmlSubEle);
                    xmlSubEle.SetAttribute(strName, "");
                    xmlDoc.Save(filePath);
                    break;
                }
            }
            return "";
        }

        public static void Write(string strGroupName, string strName, string strValue)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList xmlNodes;
            XmlElement xmlSubEle;

            string filePath = XmlFilePath;
            if (File.Exists(filePath) == false)
            {
                xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?> <configuration> <appSettings> </appSettings> </configuration>");
                xmlDoc.Save(filePath);
            }
            xmlDoc.Load(filePath);

            xmlNodes = xmlDoc.GetElementsByTagName(strGroupName);
            foreach (XmlNode xmlNode in xmlNodes)
            {
                foreach (XmlAttribute xmlAttr in xmlNode.Attributes)
                {
                    if (xmlAttr.Name == strName)
                    {
                        xmlAttr.Value = strValue;
                        xmlDoc.Save(filePath);
                        return;
                    }
                }
                XmlAttribute xmlAttr1 = xmlDoc.CreateAttribute(strName);
                xmlAttr1.Value = strValue;
                xmlNode.Attributes.Append(xmlAttr1);
                xmlDoc.Save(filePath);
                return;
            }
            xmlNodes = xmlDoc.DocumentElement.ChildNodes;
            foreach (XmlElement xmlEle in xmlNodes)
            {
                if (xmlEle.Name.ToLower() == "appsettings")
                {
                    xmlSubEle = xmlDoc.CreateElement(strGroupName);
                    xmlEle.AppendChild(xmlSubEle);
                    xmlSubEle.SetAttribute(strName, strValue);
                    xmlDoc.Save(filePath);
                    break;
                }
            }
        }
    }
}
