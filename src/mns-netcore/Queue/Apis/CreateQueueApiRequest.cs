using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;

namespace Aliyun.MNS
{
    public class CreateQueueApiRequest : ApiRequestBase<QueueAttributeParameter, CreateQueueApiResult>
    {
        public string QueueName { get; set; }

        protected override string Path => $"/queues/{this.QueueName}";

        protected override HttpMethod Method => HttpMethod.Put;

        public CreateQueueApiRequest(MnsConfig config, string queueName, QueueAttributeParameter parameter) : base(config, parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentException("parameter");
            }

            parameter.Validate();

            this.QueueName = queueName;
        }

        protected override string Body
        {
            get
            {
                var xml = XmlSerdeUtility.Serialize(this.Parameter);

                return xml;
            }
        }
    }

    public class CreateQueueApiResult : ApiResult<string>
    {
        public CreateQueueApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.Created:
                    this.Result = this.Response.Headers.Location.ToString();
                    break;
                case HttpStatusCode.Conflict:
                    throw new QueueAlreadyExistException(this.ResponseText);
                case HttpStatusCode.BadRequest:
                    {
                        var error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                        throw error.Code == "InvalidArgument" ? (MnsException)new InvalidArgumentException(error) : new QueueNumExceededLimitException(error);
                    }
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }

    /*
     *  DelaySeconds	发送到该 Queue 的所有消息默认将以DelaySeconds参数指定的秒数延后可被消费，单位为秒。	0-604800秒（7天）范围内某个整数值，默认值为0
        MaximumMessageSize	发送到该Queue的消息体的最大长度，单位为byte。	1024(1KB)-65536（64KB）范围内的某个整数值，默认值为65536（64KB）。
        MessageRetentionPeriod	消息在该 Queue 中最长的存活时间，从发送到该队列开始经过此参数指定的时间后，不论消息是否被取出过都将被删除，单位为秒。	60 (1分钟)-1296000 (15 天)范围内某个整数值，默认值345600 (4 天)
        VisibilityTimeout	消息从该 Queue 中取出后从Active状态变成Inactive状态后的持续时间，单位为秒。	1-43200(12小时)范围内的某个值整数值，默认为30（秒）
        PollingWaitSeconds	当 Queue 中没有消息时，针对该 Queue 的 ReceiveMessage 请求最长的等待时间，单位为秒。	0-30秒范围内的某个整数值，默认为0（秒）
        LoggingEnabled	是否开启日志管理功能，True表示启用，False表示停用	True/False，默认为False
     */
    [XmlRootAttribute(ElementName = "Queue", Namespace = MnsConstants.MNS_XML_NS)]
    public class QueueAttributeParameter : ApiParameterBase
    {
        [XmlElement]
        public int DelaySeconds { get; set; } = 0;

        [XmlElement]
        public int MaximumMessageSize { get; set; } = 65536;

        [XmlElement]
        public int MessageRetentionPeriod { get; set; } = 345600;

        [XmlElement]
        public int VisibilityTimeout { get; set; } = 30;

        [XmlElement]
        public int PollingWaitSeconds { get; set; } = 0;

        [XmlElement]
        public bool LoggingEnabled { get; set; } = false;

        public void Validate()
        {
            if (this.DelaySeconds < 0 || this.DelaySeconds > 604800)
            {
                throw new ArgumentException("DelaySeconds should be in range 0-604800");
            }

            if (this.MaximumMessageSize < 1024 || this.MaximumMessageSize > 65536)
            {
                throw new ArgumentException("MaximumMessageSize should be in range 1024(1KB)-65536（64KB）");
            }

            if (this.MessageRetentionPeriod < 60 || this.MessageRetentionPeriod > 1296000)
            {
                throw new ArgumentException("MessageRetentionPeriod should be in range 60 (1 min)-1296000 (15 days)");
            }

            if (this.VisibilityTimeout < 1 || this.VisibilityTimeout > 43200)
            {
                throw new ArgumentException("VisibilityTimeout should be in range 1-43200 (12 hours)");
            }

            if (this.PollingWaitSeconds < 0 || this.PollingWaitSeconds > 30)
            {
                throw new ArgumentException("PollingWaitSeconds should be in range 	0-30 seconds");
            }
        }
    }
}
