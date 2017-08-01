using Aliyun.MNS.Common;
using Aliyun.MNS.Model;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;
using System;

namespace Aliyun.MNS
{
    public class GetQueueAttributeApiRequest : ApiRequestBase<GetQueueAttributeApiResult>
    {
        public string QueueName { get; set; }

        protected override HttpMethod Method => HttpMethod.Get;
        protected override string Path => $"/queues/{QueueName}";

        public GetQueueAttributeApiRequest(MnsConfig config, string queueName) : base(config)
        {
            this.QueueName = queueName;
        }
    }

    public class GetQueueAttributeApiResult : ApiResult<QueueAttributeModel>
    {
        public GetQueueAttributeApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.OK:
                    this.Result = XmlSerdeUtility.Deserialize<QueueAttributeModel>(this.ResponseText);
                    break;
                case HttpStatusCode.NotFound:
                    throw new QueueNotExistException(this.ResponseText);
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }

}