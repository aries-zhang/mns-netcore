using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Aliyun.MNS.Utility
{
    public static class XmlSerdeUtility
    {
        public static string Serialize(object o)
        {
            string xmlString = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(o.GetType());
                xml.Serialize(ms, o);
                byte[] arr = ms.ToArray();
                xmlString = Encoding.UTF8.GetString(arr, 0, arr.Length);
                ms.Dispose();
            }
            return xmlString;
        }

        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
