using Aliyun.MNS.Common;
using Aliyun.MNS.Model.Topic;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS.Topic.Apis
{
    public class GetTopicAttributesApiRequest : ApiRequestBase<GetTopicAttributesApiResult>
    {
        public string TopicName { get; set; }

        protected override string Path => $"/topics/{TopicName}";

        protected override HttpMethod Method => HttpMethod.Get;

        public GetTopicAttributesApiRequest(MnsConfig config, string topicName) : base(config)
        {
            TopicName = topicName;
        }
    }

    public class GetTopicAttributesApiResult : ApiResult<TopicAttributeModel>
    {
        public GetTopicAttributesApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Result = XmlSerdeUtility.Deserialize<TopicAttributeModel>(ResponseText);
                    break;
                case HttpStatusCode.NotFound:
                    throw new TopicNotExistException(this.ResponseText);
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }
}
