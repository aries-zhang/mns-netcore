using Aliyun.MNS.Common;
using Aliyun.MNS.Model;
using Aliyun.MNS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;

namespace Aliyun.MNS
{
    public class BatchSendMessageApiRequest : ApiRequestBase<BatchSendMessageApiParameter, BatchSendMessageApiResult>
    {
        public string QueueName { get; set; }


        protected override string Path => $"/queues/{this.QueueName}/messages";

        protected override HttpMethod Method => HttpMethod.Post;

        public BatchSendMessageApiRequest(MnsConfig config, string queueName, BatchSendMessageApiParameter messages) : base(config, messages)
        {
            if (messages == null || messages.Messages == null || messages.Messages.Count <= 0 || messages.Messages.Where(m => string.IsNullOrEmpty(m.MessageBody)).Count() > 0)
            {
                throw new ArgumentException("messages");
            }

            this.QueueName = queueName;
        }

        protected override string Body
        {
            get
            {
                var xml = XmlSerdeUtility.Serialize(this.Parameter);

                return xml;
            }
        }
    }

    public class BatchSendMessageApiResult : ApiResult<BatchSendMessageApiResultModel>
    {
        public BatchSendMessageApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.Created:
                    this.Result = XmlSerdeUtility.Deserialize<BatchSendMessageApiResultModel>(this.ResponseText);
                    break;
                case HttpStatusCode.NotFound:
                    throw new QueueNotExistException(this.ResponseText);
                case HttpStatusCode.BadRequest:
                    {
                        var error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                        throw error.Code == "MalformedXML" ? (MnsException)new MalformedXMLException(error) : new InvalidArgumentException(error);
                    }
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }

    [XmlRoot(ElementName = "Messages", Namespace = MnsConstants.MNS_XML_NS)]
    public class BatchSendMessageApiParameter : ApiParameterBase
    {
        [XmlElement(ElementName = "Message")]
        public List<SendMessageApiParameter> Messages { get; set; }
    }

    [XmlRoot(ElementName = "Messages", Namespace = MnsConstants.MNS_XML_NS)]
    public class BatchSendMessageApiResultModel
    {
        [XmlElement(ElementName = "Message")]
        public List<SendMessageApiResultModel> Messages { get; set; }
    }
}
