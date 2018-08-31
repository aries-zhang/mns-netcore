using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace Aliyun.MNS.Topic.Apis
{
    public class SetSubscriptionAttributesApiRequest : ApiRequestBase<SubscriptionAttributeParameter, SetSubscriptionAttributesApiResult>
    {
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }

        protected override string Path => $"/topics/{TopicName}/subscriptions/{SubscriptionName}?metaoverride=true";

        protected override HttpMethod Method => HttpMethod.Put;

        public SetSubscriptionAttributesApiRequest(MnsConfig config, string topicName, string subscriptionName, SubscriptionAttributeParameter parameter) : base(config, parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }

        protected override string Body => XmlSerdeUtility.Serialize(Parameter);
    }

    public class SetSubscriptionAttributesApiResult : ApiResult
    {
        public SetSubscriptionAttributesApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.NoContent:
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    throw new SubscriptionNotExistException(ResponseText);
                default:
                    throw new UnknowException(Response);
            }
        }
    }

    [XmlRoot(ElementName = "Subscription", Namespace = MnsConstants.MNS_XML_NS)]
    public class SubscriptionAttributeParameter : ApiParameterBase
    {
        [XmlElement]
        public string NotifyStrategy { get; set; } = MnsConstants.TOPIC_NOTIFY_STRATEGY_BACKOFF_RETRY;
    }
}
