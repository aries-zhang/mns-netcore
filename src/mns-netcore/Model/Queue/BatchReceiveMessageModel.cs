using Aliyun.MNS.Common;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aliyun.MNS.Model
{
    [XmlRoot(ElementName = "Messages", Namespace = MnsConstants.MNS_XML_NS)]
    public class BatchReceiveMessageModel
    {
        [XmlElement(ElementName = "Message")]
        public List<ReceiveMessageModel> Messages { get; set; }
    }
}
