using Aliyun.MNS.Common;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aliyun.MNS.Model
{
    [XmlRoot(ElementName = "ReceiptHandles", Namespace = MnsConstants.MNS_XML_NS)]
    public class BatchDeleteMessageApiParameter : ApiParameterBase
    {
        [XmlElement(ElementName = "ReceiptHandle")]
        public List<string> ReceiptHandles { get; set; }
    }
}
