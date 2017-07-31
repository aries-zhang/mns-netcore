using Aliyun.MNS.Common;
using Aliyun.MNS.Model;
using Aliyun.MNS.Utility;
using System;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS.Apis.Queue
{
    public class SendMessageApiRequest : ApiRequestBase<SendMessageApiParameter, SendMessageApiResult>
    {
        public string QueueName { get; set; }

        protected override string Path => $"/queues/{this.QueueName}/messages";

        protected override HttpMethod Method => HttpMethod.Post;

        public SendMessageApiRequest(MnsConfig config, string queueName, string messageBody, int delaySeconds = 0, int priority = 8) :
            base(config, new SendMessageApiParameter()
            {
                DelaySeconds = delaySeconds,
                MessageBody = messageBody,
                Priority = priority
            })
        {
            this.QueueName = queueName;
        }

        protected override string Body
        {
            get
            {
                var xml = XmlSerdeUtility.Serialize(this.Parameter);
                Console.WriteLine(xml);
                return xml;
            }
        }
    }

    public class SendMessageApiResult : ApiResult<SendMessageApiResultModel>
    {
        public SendMessageApiResult(HttpResponseMessage response) : base(response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    this.Result = XmlSerdeUtility.Deserialize<SendMessageApiResultModel>(this.ResponseText);
                    break;
                case HttpStatusCode.NotFound:
                    throw new QueueNotExistException(this.ResponseText);
                case HttpStatusCode.BadRequest:
                    {
                        var error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                        throw error.Code == "MalformedXML" ? (MnsException)new MalformedXMLException(error) : new InvalidArgumentException(error);
                    }
                default:
                    throw new UnknowException();
            }
        }
    }
}
