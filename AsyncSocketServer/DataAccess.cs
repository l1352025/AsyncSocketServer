using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace AsyncSocketServer
{
    class DataAccess
    {

        private List<UserInfo> _users;

        private string _dbFileUser;
        private string _dbFileOrder;
        private string _dbFileDefualt;

        public DataAccess ()
        {
            _dbFileUser = "user.xml";
            _dbFileOrder = "order.xml";
            _dbFileDefualt = "default.xml";

            _users = new List<UserInfo>();

            //DbCreate();
        }

        private void DbCreate()
        {
            if (false == File.Exists(_dbFileUser))
            {
                File.Create(_dbFileUser);

                XmlDocument doc = new XmlDocument();

                XmlNode xmlDeclare = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(xmlDeclare);

                XmlElement node = (XmlElement)doc.CreateElement("Table");
                doc.AppendChild(node);

                doc.Save(_dbFileUser);
            }
        }

        public void DbInsert<T> (T obj)
        {
            if (obj.GetType() == typeof(UserInfo))
            {
                lock (_users)
                {
                    _users.Add(obj as UserInfo);
                    WriteToDb<List<UserInfo>>(_users);
                }
            }
            else
            {
                WriteToDb<T>(obj);
            }
        }

        public void DbDelete(Type obj)
        {

        }

        public void DbUptate(Type obj)
        {

        }

        private void WriteToDb<T> (T obj)
        {
            string path = null;

            if (obj.GetType() == typeof(List<UserInfo>))
            {
                path = _dbFileUser;
            }
            else
            {
                path = "defaultDb.xml";
            }

            using (StreamWriter sw = new StreamWriter(path))
            {

                XmlSerializer xmlSerial = new XmlSerializer(obj.GetType());

                xmlSerial.Serialize(sw, obj);
            }
        }

        private T ReadFromDb<T>() where T : class
        {
            StreamReader sr = null;


            if (typeof(T) == typeof(UserInfo))
            {
                sr = new StreamReader(_dbFileUser);
            }

            XmlSerializer xmlSerial = new XmlSerializer(typeof(T));
   
            return xmlSerial.Deserialize(sr) as T;
        }
    }

    public class UserInfo
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

    }
}
