using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Serialization;

namespace Aliyun.MNS.Topic.Apis
{
    public class ListSubscriptionByTopicApiRequest : ApiRequestBase<ListSubscriptionByTopicApiResult>
    {
        public string Prefix { get; set; }

        public int PageSize { get; set; }

        public string NextMarker { get; set; }

        public string TopicName { get; set; }

        protected override string Path => $"/topics/{TopicName}/subscriptions";

        protected override HttpMethod Method => throw new NotImplementedException();

        public ListSubscriptionByTopicApiRequest(MnsConfig config, string topicName, string prefix, int pageSize, string nextMarker) : base(config)
        {
            if (pageSize <= 0 || pageSize > 1000)
            {
                throw new InvalidArgumentException("Page size should be in range 1 - 10000");
            }

            TopicName = topicName;
            Prefix = prefix;
            PageSize = pageSize;
            NextMarker = nextMarker;
        }

        protected override void AdditionalHeaders(HttpRequestHeaders headers)
        {
            if (!string.IsNullOrEmpty(this.Prefix))
            {
                headers.Add("x-mns-prefix", this.Prefix);
            }

            if (this.PageSize > 0)
            {
                headers.Add("x-mns-ret-number", this.PageSize.ToString());
            }

            if (!string.IsNullOrEmpty(this.NextMarker))
            {
                headers.Add("x-mns-marker", this.NextMarker);
            }
        }
    }

    public class ListSubscriptionByTopicApiResult : ApiResult<SubscriptionListModel>
    {
        public ListSubscriptionByTopicApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    Result = XmlSerdeUtility.Deserialize<SubscriptionListModel>(ResponseText);
                    break;
                default:
                    throw new UnknowException(Response);
            }
        }
    }

    [XmlRoot(ElementName = "Subscriptions", Namespace = MnsConstants.MNS_XML_NS)]
    public class SubscriptionListModel
    {
        [XmlElement(ElementName = "Subscription")]
        public List<SubscriptionListItem> Items { get; set; }

        [XmlElement(ElementName = "NextMarker")]
        public string NextMarker { get; set; }
    }

    public class SubscriptionListItem
    {
        public string SubscriptionURL { get; set; }
    }
}
