using Aliyun.MNS.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Aliyun.MNS.Topic.Apis
{
    public class UnsubscribeApiRequest : ApiRequestBase<UnsubscribeApiResult>
    {
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }

        protected override string Path => $"/topics/{TopicName}/subscriptions/{SubscriptionName}";

        protected override HttpMethod Method => HttpMethod.Delete;

        public UnsubscribeApiRequest(MnsConfig config, string topicName, string subscriptionName) : base(config)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }
    }

    public class UnsubscribeApiResult : ApiResult
    {
        public UnsubscribeApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.NoContent:
                    break;
                default:
                    throw new UnknowException(Response);
            }
        }
    }
}
