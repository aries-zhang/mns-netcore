using Aliyun.MNS.Common;
using Aliyun.MNS.Model;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS
{
    internal class BatchReceiveMessageApiRequest : ApiRequestBase<BatchReceiveMessageApiResult>
    {
        private MnsConfig config;
        private string queueName;
        private int waitseconds;
        private int numOfMessages;

        protected override string Path => $"/queues/{this.queueName}/messages?numOfMessages={this.numOfMessages}&waitseconds={this.waitseconds}";
        protected override HttpMethod Method => HttpMethod.Get;

        public BatchReceiveMessageApiRequest(MnsConfig config, string queueName, int waitseconds = 10, int numOfMessages = 16) : base(config)
        {
            this.config = config;
            this.queueName = queueName;
            this.waitseconds = waitseconds;
            this.numOfMessages = numOfMessages;
        }
    }

    public class BatchReceiveMessageApiResult : ApiResult<BatchReceiveMessageModel>
    {
        public BatchReceiveMessageApiResult(HttpResponseMessage response) : base(response)
        {
            switch (response.StatusCode)
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
                    throw new UnknowException(response);
            }
        }
    }
}