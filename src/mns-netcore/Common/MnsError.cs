using System.Net;
using System.Xml.Serialization;

namespace Aliyun.MNS.Common
{
    [XmlRootAttribute(ElementName = "Error", Namespace = MnsConstants.MNS_XML_NS)]
    public class MnsError
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        [XmlElement]
        public string Code { get; set; }
        [XmlElement]
        public string Message { get; set; }
        [XmlElement]
        public string RequestIdMessage { get; set; }
        [XmlElement]
        public string HostIdMessage { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
