using Aliyun.MNS.Common;
using System.Xml.Serialization;

namespace Aliyun.MNS.Model
{
    [XmlRootAttribute(ElementName = "Message", Namespace = MnsConstants.MNS_XML_NS)]
    public class SendMessageApiParameter : ApiParameterBase
    {
        [XmlElement]
        public string MessageBody { get; set; }

        [XmlElement]
        public int DelaySeconds { get; set; }

        [XmlElement]
        public int Priority { get; set; }
    }
}
