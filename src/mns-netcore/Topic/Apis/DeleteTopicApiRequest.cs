using Aliyun.MNS.Common;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS.Topic.Apis
{
    public class DeleteTopicApiRequest : ApiRequestBase<DeleteTopicApiResult>
    {
        public string TopicName { get; set; }

        protected override string Path => $"/topics/{this.TopicName}";

        protected override HttpMethod Method => HttpMethod.Delete;

        public DeleteTopicApiRequest(MnsConfig config, string topicName) : base(config)
        {
            TopicName = topicName;
        }
    }

    public class DeleteTopicApiResult : ApiResult
    {
        public DeleteTopicApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (Response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                default:
                    throw new UnknowException(Response);
            }
        }
    }
}
