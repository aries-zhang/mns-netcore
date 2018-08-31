using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS.Topic.Apis
{
    public class SetTopicAttributesApiRequest : ApiRequestBase<TopicAttributeParameter, SetTopicAttributesApiResult>
    {
        public string TopicName { get; set; }

        protected override string Path => $"/topics/{TopicName}?metaoverride=true";

        protected override HttpMethod Method => HttpMethod.Put;

        public SetTopicAttributesApiRequest(MnsConfig config, string topicName, TopicAttributeParameter parameter) : base(config, parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentException("parameter");
            }

            parameter.Validate();

            this.TopicName = topicName;
        }

        protected override string Body => XmlSerdeUtility.Serialize(this.Parameter);
    }

    public class SetTopicAttributesApiResult : ApiResult
    {
        public SetTopicAttributesApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.NotFound:
                    throw new TopicNotExistException(ResponseText);
                default:
                    throw new UnknowException(Response);
            }
        }
    }
}
