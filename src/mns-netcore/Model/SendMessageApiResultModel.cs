using Aliyun.MNS.Common;
using System.Xml.Serialization;

namespace Aliyun.MNS.Model
{
    [XmlRootAttribute(ElementName = "Message", Namespace = MnsConstants.MNS_XML_NS)]
    public class SendMessageApiResultModel
    {
        [XmlElement]
        public string MessageId { get; set; }

        [XmlElement]
        public string MessageBodyMD5 { get; set; }

        [XmlElement(IsNullable = true)]
        public string ReceiptHandle { get; set; }
    }
}
