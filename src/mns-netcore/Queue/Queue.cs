using Aliyun.MNS.Apis.Queue;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    /// <summary>
    /// Queue 的操作：CreateQueue，DeleteQueue，ListQueue，GetQueueAttributes，SetQueueAttributes.
    /// Message 的操作：SendMessage，BatchSendMessage，ReceiveMessage，BatchReceiveMessage，PeekMessage，BatchPeekMessage，DeleteMessage，BatchDeleteMessage，ChangeMessageVisibility.
    /// </summary>
    public class Queue : IQueue
    {
        public MnsConfig Config { get; set; }

        public string Name { get; set; }

        public Queue(MnsConfig config, string name)
        {
            this.Name = name;
        }

        public async Task<GetQueueAttributeResult> GetAttributes()
        {
            var request = new GetQueueAttributeApiRequest(this.Config, this.Name);
            return await request.Call();
        }

        public async Task<SendMessageApiResult> SendMessage(string message, int delaySeconds = 0, int priority = 8)
        {
            return await new SendMessageApiRequest(this.Config, this.Name, message, delaySeconds: delaySeconds, priority: priority).Call();
        }

        public async Task<ReceiveMessageApiResult> ReceiveMessage(int waitSeconds = 10)
        {
            return await new ReceiveMessageApiRequest(this.Config, this.Name, waitSeconds: waitSeconds).Call();
        }

        public async Task<BatchReceiveMessageApiResult> BatchReceiveMessage()
        {
            return await new BatchReceiveMessageApiRequest(this.Config, this.Name).Call();
        }

        public async Task<DeleteMessageApiResult> DeleteMessage(string receiptHandle)
        {
            return await new DeleteMessageApiRequest(this.Config, this.Name, receiptHandle).Call();
        }
    }
}
