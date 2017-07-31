using Aliyun.MNS.Apis.Queue;
using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS
{
    public class DeleteMessageApiRequest : ApiRequestBase<ApiResult>
    {
        private MnsConfig config;
        private string queneName;
        private string receiptHandle;

        public DeleteMessageApiRequest(MnsConfig config, string queneName, string receiptHandle) : base(config)
        {
            this.config = config;
            this.queneName = queneName;
            this.receiptHandle = receiptHandle;
        }

        protected override string Path => $"/queues/{this.queneName}/messages?ReceiptHandle={this.receiptHandle}";

        protected override HttpMethod Method => HttpMethod.Delete;
    }

    public class DeleteMessageApiResult : ApiResult
    {
        public DeleteMessageApiResult(HttpResponseMessage response) : base(response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.NotFound:
                    throw new QueueNotExistException(this.ResponseText);
                case HttpStatusCode.BadRequest:
                    {
                        var error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                        throw error.Code == "InvalidArgument" ? (MnsException)new InvalidArgumentException(error) : new ReceiptHandleErrorException(error);
                    }
                default:
                    throw new UnknowException();
            }
        }
    }
}