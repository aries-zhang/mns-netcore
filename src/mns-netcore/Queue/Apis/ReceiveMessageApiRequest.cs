using Aliyun.MNS.Apis.Queue;
using Aliyun.MNS.Common;
using Aliyun.MNS.Model;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;

namespace Aliyun.MNS
{
    public class ReceiveMessageApiRequest : ApiRequestBase<ReceiveMessageApiResult>
    {
        private MnsConfig config;
        private string queueName;
        private int waitSeconds;

        protected override string Path => $"/queues/{this.queueName}/messages?waitseconds={this.waitSeconds}";

        protected override HttpMethod Method => HttpMethod.Get;

        public ReceiveMessageApiRequest(MnsConfig config, string queueName, int waitSeconds = 10) : base(config)
        {
            this.config = config;
            this.queueName = queueName;
            this.waitSeconds = waitSeconds;
        }
    }

    public class ReceiveMessageApiResult : ApiResult<ReceiveMessageModel>
    {
        public ReceiveMessageApiResult(HttpResponseMessage response) : base(response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    this.Result = XmlSerdeUtility.Deserialize<ReceiveMessageModel>(this.ResponseText);
                    break;
                case HttpStatusCode.NotFound:
                    {
                        var error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                        throw error.Code == "QueueNotExist" ? (MnsException)new QueueNotExistException(error) : new MessageNotExistException(error);
                    }
                default:
                    throw new UnknowException();
            }
        }
    }

}