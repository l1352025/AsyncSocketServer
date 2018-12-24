using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace XmlHelper
{
    class XmlHelper
    {
        /// <summary>
        /// 检查XML文件是否存在，不存在则建立
        /// </summary>
        public static void CheckXmlFile(string configFile)
        {
            if (true == File.Exists(configFile))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();

            XmlNode xmlDeclare = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(xmlDeclare);

            XmlElement ConfigNode = (XmlElement)doc.CreateElement("Config");
            doc.AppendChild(ConfigNode);

            XmlElement ConcSimulatorNode = (XmlElement)doc.CreateElement("ConcSimulator");
            ConfigNode.AppendChild(ConcSimulatorNode);

            XmlElement DataMonitorNode = (XmlElement)doc.CreateElement("DataMonitor");
            ConfigNode.AppendChild(DataMonitorNode);

            doc.Save(configFile);
        }
        /// <summary>
        /// 获取XML 节点的值
        /// </summary>
        /// <param name="xmlInfo">XML文件路径 或者字符串</param>
        /// <param name="selectNode">XML选择的节点路径</param>
        /// <returns>返回节点的值</returns>
        public static string GetNodeValue(string xmlInfo, string selectNode)
        {
            XPathDocument xPath = null;
            try
            {
                xPath = new XPathDocument(xmlInfo);

                XPathNavigator xNav = xPath.CreateNavigator();
                XPathNavigator xnSelectNode = xNav.SelectSingleNode(selectNode);
                return xnSelectNode.InnerXml;
            }
            catch
            {
                return "";
            }
            finally
            {
                xPath = null;
            }
        }
        /// <summary>
        /// 获取XML 节点的值
        /// </summary>
        /// <param name="xmlInfo">XML文件路径 或者字符串</param>
        /// <param name="selectNode">XML选择的节点路径</param>
        /// <returns>返回节点的值</returns>
        public static string GetNodeDefValue(string xmlInfo, string selectNode, string DefaultValue)
        {
            XPathDocument xPath = null;
            try
            {
                xPath = new XPathDocument(xmlInfo);

                return GetNodeValue(xPath, selectNode);
            }
            catch
            {
                return DefaultValue;
            }
            finally
            {
                xPath = null;
            }
        }
        /// <summary>
        /// 获取XML 节点的值
        /// </summary>
        /// <param name="path">创建只读的Xml类</param>
        /// <param name="selectNode">XML选择的节点路径</param>
        /// <returns>返回节点的值</returns>
        public static string GetNodeValue(XPathDocument xPath, string selectNode)
        {
            try
            {
                XPathNavigator xNav = xPath.CreateNavigator();
                XPathNavigator xnSelectNode = xNav.SelectSingleNode(selectNode);
                return xnSelectNode.InnerXml;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="xmlInfo">XML文件路径 或者字符串</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.GetNodeValue(path, "/Node", "")
         * XmlHelper.GetNodeValue(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public static string GetNodeValue(string xmlInfo, string selectNode, string attribute)
        {
            XPathDocument xPath = null;
            try
            {
                xPath = new XPathDocument(xmlInfo);

                return GetNodeValue(xPath, selectNode, attribute);
            }
            catch
            {
                return "";
            }
            finally
            {
                xPath = null;
            }
        }

        public static string GetNodeValue(XPathDocument xPath, string selectNode, string attribute)
        {
            try
            {
                XPathNavigator xNav = xPath.CreateNavigator();
                XPathNavigator xnSelectNode = xNav.SelectSingleNode(selectNode);
                return (attribute == null ? xnSelectNode.InnerXml : xnSelectNode.GetAttribute(attribute, ""));
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="fatherNode">父级节点路径</param>
        /// <param name="childNode">子节点</param>
        /// <param name="childValue">子节点值</param>
        public static bool SetNodeValue(string path, string fatherNode, string childNode, string childValue)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(fatherNode + "/" + childNode);
                if (xn == null)
                {
                    XmlElement inertxm = doc.CreateElement(childNode);
                    inertxm.InnerText = childValue;

                    XmlNode xnSystem = doc.SelectSingleNode(fatherNode);
                    xnSystem.AppendChild(inertxm);
                }
                else//更新
                {
                    XmlElement xe = (XmlElement)xn;
                    xe.InnerText = childValue;
                }
                doc.Save(path);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                doc = null;
            }
        }
    }
}
