using Aliyun.MNS.Common;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS
{
    public class DeleteQueueApiRequest : ApiRequestBase<DeleteQueueApiResult>
    {
        public string QueueName { get; set; }

        protected override string Path => $"/queues/{this.QueueName}";

        protected override HttpMethod Method => HttpMethod.Delete;

        public DeleteQueueApiRequest(MnsConfig config, string queueName) : base(config)
        {
            this.QueueName = queueName;
        }
    }

    public class DeleteQueueApiResult : ApiResult
    {
        public DeleteQueueApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }
}
