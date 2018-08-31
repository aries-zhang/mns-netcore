using Aliyun.MNS.Common;
using System.Xml.Serialization;

namespace Aliyun.MNS.Model
{
    [XmlRootAttribute(ElementName = "Message", Namespace = MnsConstants.MNS_XML_NS)]
    public class ReceiveMessageModel
    {
        public string MessageId { get; set; } //消息编号，在一个 Queue 中唯一
        public string ReceiptHandle { get; set; } //本次获取消息产生的临时句柄，用于删除和修改处于 Inactive 消息，NextVisibleTime 之前有效。
        public string MessageBodyMD5 { get; set; } //消息正文的 MD5 值
        public string MessageBody { get; set; } //消息正文
        public long EnqueueTime { get; set; } //消息发送到队列的时间，从 1970年1月1日 00:00:00 000 开始的毫秒数
        public long NextVisibleTime { get; set; } //下次可被再次消费的时间，从1970年1月1日 00:00:00 000 开始的毫秒数
        public long FirstDequeueTime { get; set; } //第一次被消费的时间，从1970年1月1日 00:00:00 000 开始的毫秒数
        public long DequeueCount { get; set; } //总共被消费的次数
        public int Priority { get; set; } //消息的优先级权值
    }
}
