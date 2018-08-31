using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System.Net.Http;
using System.Xml.Serialization;

namespace Aliyun.MNS.Topic.Apis
{
    public class GetSubscriptionAttributesApiRequest : ApiRequestBase<GetSubscriptionAttributesApiResult>
    {
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }

        protected override string Path => $"/topics/{TopicName}/subscriptions/{SubscriptionName}";

        protected override HttpMethod Method => HttpMethod.Get;

        public GetSubscriptionAttributesApiRequest(MnsConfig config, string topicName, string subscriptionName) : base(config)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }
    }

    public class GetSubscriptionAttributesApiResult : ApiResult<SubscriptionAttributeModel>
    {
        public GetSubscriptionAttributesApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    Result = XmlSerdeUtility.Deserialize<SubscriptionAttributeModel>(ResponseText);
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    throw new SubscriptionNotExistException(ResponseText);
                default:
                    throw new UnknowException(Response);
            }
        }
    }

    [XmlRoot(ElementName = "Subscription", Namespace = MnsConstants.MNS_XML_NS)]
    public class SubscriptionAttributeModel
    {
        public string SubscriptionName { get; set; } // Subscription 的名称
        public string Subscriber { get; set; } //  Subscription 订阅者的 AccountId
        public string TopicOwner { get; set; } //  Subscription 订阅的主题所有者的 AccountId
        public string TopicName { get; set; } //   Subscription 订阅的主题名称
        public string Endpoint { get; set; } // 订阅的终端地址
        public string NotifyStrategy { get; set; } // 向 Endpoint 推送消息错误时的重试策略
        public string NotifyContentFormat { get; set; } // 向 Endpoint 推送的消息内容格式
        public string FilterTag { get; set; } // 描述了该订阅中消息过滤的标签（仅标签一致的消息才会被推送）
        public long CreateTime { get; set; } // Subscription的创建时间，从 1970-1-1 00:00:00 到现在的秒值
        public long LastModifyTime { get; set; } //  修改 Subscription 属性信息最近时间，从 1970-1-1 00:00:00 到现在的秒值
    }
}
