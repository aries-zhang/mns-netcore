using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Serialization;

namespace Aliyun.MNS
{
    internal class BatchReceiveMessageApiRequest : ApiRequestBase<BatchReceiveMessageApiResult>
    {
        private MnsConfig config;
        private string queueName;
        private int waitseconds;
        private int numOfMessages;

        protected override string Path => $"/queues/{this.queueName}/messages?numOfMessages={this.numOfMessages}&waitseconds={this.waitseconds}";
        protected override HttpMethod Method => HttpMethod.Get;

        public BatchReceiveMessageApiRequest(MnsConfig config, string queueName, int waitseconds = 10, int numOfMessages = 16) : base(config)
        {
            this.config = config;
            this.queueName = queueName;
            this.waitseconds = waitseconds;
            this.numOfMessages = numOfMessages;
        }
    }

    public class BatchReceiveMessageApiResult : ApiResultBase
    {
        public BatchReceiveMessageModel Result { get; set; }

        public BatchReceiveMessageApiResult(HttpResponseMessage response) : base(response)
        {
            this.Result = XmlSerdeUtility.Deserialize<BatchReceiveMessageModel>(this.ResponseText);
        }
    }

    [XmlRoot(ElementName = "Messages", Namespace = MnsConstants.MNS_XML_NS)]
    public class BatchReceiveMessageModel
    {
        [XmlElement(ElementName = "Message")]
        public List<ReceiveMessageModel> Messages { get; set; }
    }
}