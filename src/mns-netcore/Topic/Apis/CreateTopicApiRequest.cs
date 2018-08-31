using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;

namespace Aliyun.MNS.Topic.Apis
{
    public class CreateTopicApiRequest : ApiRequestBase<TopicAttributeParameter, CreateTopicApiResult>
    {
        public string TopicName { get; set; }

        protected override string Path => $"/topics/{TopicName}";

        protected override HttpMethod Method => HttpMethod.Put;

        public CreateTopicApiRequest(MnsConfig config, string topicName, TopicAttributeParameter parameter) :
            base(config, parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentException("parameter");
            }

            parameter.Validate();

            this.TopicName = topicName;
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

    public class CreateTopicApiResult : ApiResult<string>
    {
        public CreateTopicApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.Created:
                    this.Result = this.Response.Headers.Location.ToString();
                    break;
                case HttpStatusCode.Conflict:
                    throw new TopicAlreadyExistException(this.ResponseText);
                case HttpStatusCode.BadRequest:
                    throw new TopicNameLengthErrorException(this.ResponseText);
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }

    [XmlRoot(ElementName = "Topic", Namespace = MnsConstants.MNS_XML_NS)]
    public class TopicAttributeParameter : ApiParameterBase
    {
        [XmlElement]
        public int MaximumMessageSize { get; set; } = 65536;

        [XmlElement]
        public bool LoggingEnabled { get; set; } = false;

        public void Validate()
        {
            if (this.MaximumMessageSize < 1024 || this.MaximumMessageSize > 65536)
            {
                throw new ArgumentException("MaximumMessageSize should be in range 1024(1KB) - 65536(64KB)");
            }
        }
    }
}
