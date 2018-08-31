using Aliyun.MNS.Common;
using System.Xml.Serialization;

namespace Aliyun.MNS.Model.Topic
{
    [XmlRoot(ElementName = "Topic", Namespace = MnsConstants.MNS_XML_NS)]
    public class TopicAttributeModel
    {
        public string TopicName { get; set; } // 主题名称
        public long CreateTime { get; set; } // 主题的创建时间，从 1970-1-1 00:00:00到现在的秒值
        public long LastModifyTime { get; set; } //  修改主题属性信息的最近时间，从 1970-1-1 00:00:00 到现在的秒值
        public int MaximumMessageSize { get; set; } //  发送到该主题的消息体最大长度，单位为 Byte
        public long MessageRetentionPeriod { get; set; } // 消息在主题中最长存活时间，从发送到该主题开始经过此参数指定的时间后，不论消息是否被成功推送给用户都将被删除，单位为秒
        public long MessageCount { get; set; } //    当前该主题中消息数目
        public bool LoggingEnabled { get; set; } //  是否开启日志管理功能，True表示启用，False表示停用
    }
}
