using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace Aliyun.MNS.Topic.Apis
{
    // TODO: Implement DirectMail & DirectSMS
    public class PublishMessageApiRequest : ApiRequestBase<PublishMessageParameter, PublishMessageApiResult>
    {
        public string TopicName { get; set; }

        protected override string Path => $"/topics/{TopicName}/messages";

        protected override HttpMethod Method => HttpMethod.Post;

        public PublishMessageApiRequest(MnsConfig config, string topicName, PublishMessageParameter parameter) : base(config, parameter)
        {
            TopicName = topicName;
        }

        protected override string Body => XmlSerdeUtility.Serialize(Parameter);
    }

    public class PublishMessageApiResult : ApiResult<MessagePublishedModel>
    {
        public PublishMessageApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    Result = XmlSerdeUtility.Deserialize<MessagePublishedModel>(ResponseText);
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    throw new TopicNotExistException(ResponseText);
                default:
                    throw new UnknowException(Response);
            }
        }
    }

    [XmlRoot(ElementName = "Message", Namespace = MnsConstants.MNS_XML_NS)]
    public class MessagePublishedModel
    {
        [XmlElement]
        public string MessageId { get; set; }

        [XmlElement]
        public string MessageBodyMD5 { get; set; }
    }

    [XmlRoot(ElementName = "Message", Namespace = MnsConstants.MNS_XML_NS)]
    public class PublishMessageParameter : ApiParameterBase
    {
        [XmlElement]
        public string MessageBody { get; set; }

        [XmlElement]
        public string MessageTag { get; set; }

        [XmlElement]
        public TopicMessageAttributes MessageAttributes { get; set; }
    }
    
    public class TopicMessageAttributes 
    {
        [XmlElement]
        public string DirectMail { get; set; }

        [XmlElement]
        public string DirectSMS { get; set; }
    }
}
