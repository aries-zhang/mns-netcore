using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace Aliyun.MNS
{
    public class BatchDeleteMessageApiRequest : ApiRequestBase<BatchDeleteMessageApiParameter, ApiResult>
    {
        private MnsConfig config;
        private string queueName;

        protected override string Path => $"/queues/{this.queueName}/messages";
        protected override HttpMethod Method => HttpMethod.Delete;

        public BatchDeleteMessageApiRequest(MnsConfig config, string queueName, BatchDeleteMessageApiParameter parameter) : base(config, parameter)
        {
            this.queueName = queueName;
        }

        protected override string Body
        {
            get
            {
                var xml = XmlSerdeUtility.Serialize(this.Parameter);
                Console.WriteLine(xml);
                return xml;
            }
        }
    }

    [XmlRoot(ElementName = "ReceiptHandles", Namespace = MnsConstants.MNS_XML_NS)]
    public class BatchDeleteMessageApiParameter : ApiParameterBase
    {
        [XmlElement(ElementName = "ReceiptHandle")]
        public List<string> ReceiptHandles { get; set; }
    }
}