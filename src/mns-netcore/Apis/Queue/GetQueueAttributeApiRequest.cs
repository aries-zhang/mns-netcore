using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System.Xml.Serialization;

namespace Aliyun.MNS
{
    public class GetQueueAttributeApiRequest : ApiRequestBase<GetQueueAttributeResult>
    {
        public string QueueName { get; set; }

        protected override HttpMethod Method => HttpMethod.Get;
        protected override string Path => $"/queues/{QueueName}";

        public GetQueueAttributeApiRequest(MnsConfig config, string queueName) : base(config)
        {
            this.QueueName = queueName;
        }
    }
    
    public class GetQueueAttributeResult : ApiResultBase
    {
        public QueueAttributeModel Result { get; set; }

        public GetQueueAttributeResult(HttpResponseMessage response) : base(response)
        {
            this.Result = XmlSerdeUtility.Deserialize<QueueAttributeModel>(this.ResponseText);
        }
    }

    [XmlRootAttribute(ElementName = "Queue", Namespace = MnsConstants.MNS_XML_NS)]
    public class QueueAttributeModel
    {
        public string QueueName { get; set; } //Queue 的名称
        public long CreateTime { get; set; }  //Queue 的创建时间，从1970-1-1 00:00:00 到现在的秒值
        public long LastModifyTime { get; set; }  //修改 Queue 属性信息最近时间，从1970-1-1 00:00:00 到现在的秒值
        public int DelaySeconds { get; set; }    //发送消息到该 Queue 的所有消息默认将以 DelaySeconds 参数指定的秒数延后可被消费，单位为秒
        public long MaximumMessageSize { get; set; }  //发送到该 Queue 的消息体的最大长度，单位为byte
        public int MessageRetentionPeriod { get; set; }  //消息在该 Queue 中最长的存活时间，从发送到该队列开始经过此参数指定的时间后，不论消息是否被取出过都将被删除，单位为秒
        public int PollingWaitSeconds { get; set; }  //当 Queue 消息量为空时，针对该 Queue 的 ReceiveMessage 请求最长的等待时间，单位为秒
        public long Activemessages { get; set; }  //在该 Queue 中处于 Active 状态的消息总数，为近似值
        public long InactiveMessages { get; set; }    //在该 Queue 中处于 Inactive 状态的消息总数，为近似值
        public long DelayMessages { get; set; }   //在该 Queue 中处于 Delayed 状态的消息总数，为近似值
        public string LoggingEnabled { get; set; }  //是否开启日志管理功能，True表示启用，False表示停用
    }
}
