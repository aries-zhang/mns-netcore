using Aliyun.MNS.Common;
using Aliyun.MNS.Model;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS
{
    public class PeekMessageApiRequest : ApiRequestBase<PeekMessageApiResult>
    {
        public string QueueName { get; set; }

        protected override string Path => $"/queues/{this.QueueName}/messages?peekonly=true";

        protected override HttpMethod Method => HttpMethod.Get;

        public PeekMessageApiRequest(MnsConfig config, string queueName) : base(config)
        {
            this.QueueName = queueName;
        }
    }

    public class PeekMessageApiResult : ApiResult<ReceiveMessageModel>
    {
        public PeekMessageApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
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
                    throw new UnknowException(this.Response);
            }
        }
    }
}
