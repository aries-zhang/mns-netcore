using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace Aliyun.MNS.Apis.Queue
{
    public class SendMessageApiRequest : ApiRequestBase<SendMessageApiParameter, ApiResult<SendMessageApiResultModel>>
    {
        public string QueueName { get; set; }

        protected override string Path => $"/queues/{this.QueueName}/messages";

        protected override HttpMethod Method => HttpMethod.Post;

        public SendMessageApiRequest(MnsConfig config, string queueName, string messageBody, int delaySeconds = 0, int priority = 8) :
            base(config, new SendMessageApiParameter()
            {
                DelaySeconds = delaySeconds,
                MessageBody = messageBody,
                Priority = priority
            })
        {
            this.QueueName = queueName;
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

    [XmlRootAttribute(ElementName = "Message", Namespace = MnsConstants.MNS_XML_NS)]
    public class SendMessageApiParameter: ApiParameterBase
    {
        [XmlElement]
        public string MessageBody { get; set; }

        [XmlElement]
        public int DelaySeconds { get; set; }

        [XmlElement]
        public int Priority { get; set; }
    }

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
