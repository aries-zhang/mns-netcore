using Aliyun.MNS.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Aliyun.MNS
{
    public class SetQueueAttributesApiRequest : ApiRequestBase<QueueAttributeParameter, SetQueueAttributesApiResult>
    {
        public string QueueName { get; set; }

        protected override string Path => $"/queues/{this.QueueName}?metaoverride=true";

        protected override HttpMethod Method => HttpMethod.Put;

        public SetQueueAttributesApiRequest(MnsConfig config, string queueName, QueueAttributeParameter parameter) : base(config, parameter)
        {
            this.QueueName = queueName;
        }
    }

    public class SetQueueAttributesApiResult : ApiResult
    {
        public SetQueueAttributesApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case System.Net.HttpStatusCode.NoContent:
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    throw new QueueNotExistException(this.ResponseText);
                case System.Net.HttpStatusCode.BadRequest:
                    throw new InvalidArgumentException(this.ResponseText);
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }
}
