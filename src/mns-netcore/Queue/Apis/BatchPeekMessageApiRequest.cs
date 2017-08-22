using Aliyun.MNS.Common;
using Aliyun.MNS.Model;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS
{
    public class BatchPeekMessageApiRequest : ApiRequestBase<BatchPeekMessageApiResult>
    {
        public string QueueName { get; set; }

        public int NumberOfMessages { get; set; }

        protected override string Path => $"/queues/{this.QueueName}/messages?peekonly=true&numOfMessages={this.NumberOfMessages}";

        protected override HttpMethod Method => HttpMethod.Get;

        public BatchPeekMessageApiRequest(MnsConfig config, string queueName, int numOfMessages) : base(config)
        {
            this.QueueName = queueName;
            this.NumberOfMessages = numOfMessages;
        }
    }

    public class BatchPeekMessageApiResult : ApiResult<BatchReceiveMessageModel>
    {
        public BatchPeekMessageApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.OK:
                    this.Result = XmlSerdeUtility.Deserialize<BatchReceiveMessageModel>(this.ResponseText);
                    break;
                case HttpStatusCode.NotFound:
                    {
                        var error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                        throw error.Code == "QueueNotExist" ? (MnsException)new QueueNotExistException(error) : new MessageNotExistException(error);
                    }
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }
}
