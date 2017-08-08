using Aliyun.MNS.Apis.Queue;
using Aliyun.MNS.Model;
using System.Collections.Generic;

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

        public Queue(MnsConfig config)
        {
        }

        public Queue(MnsConfig config, string name)
        {
            this.Config = config;
            this.Name = name;
        }

        public QueueListModel ListQueue(string prefix = "", int pageSize = 0, string nextMarker = "")
        {
            var result = new ListQueueApiRequest(this.Config, prefix, pageSize, nextMarker).Call();

            return result.Result;
        }

        public QueueAttributeModel GetAttributes()
        {
            var response = new GetQueueAttributeApiRequest(this.Config, this.Name).Call();

            return response.Result;
        }

        public SendMessageApiResultModel SendMessage(string message, int delaySeconds = 0, int priority = 8)
        {
            var response = new SendMessageApiRequest(this.Config, this.Name, message, delaySeconds: delaySeconds, priority: priority).Call();

            return response.Result;
        }

        public ReceiveMessageModel ReceiveMessage(int waitSeconds = 10)
        {
            var response = new ReceiveMessageApiRequest(this.Config, this.Name, waitSeconds: waitSeconds).Call();

            return response.Result;
        }

        public BatchReceiveMessageModel BatchReceiveMessage(int waitseconds = 10, int numOfMessages = 16)
        {
            var request = new BatchReceiveMessageApiRequest(this.Config, this.Name, waitseconds: waitseconds, numOfMessages: numOfMessages);

            var result = request.Call();

            return result.Result;
        }

        public void DeleteMessage(string receiptHandle)
        {
            new DeleteMessageApiRequest(this.Config, this.Name, receiptHandle).Call();
        }

        public void BatchDeleteMessage(List<string> receiptHandles)
        {
            new BatchDeleteMessageApiRequest(this.Config, this.Name, new BatchDeleteMessageApiParameter() { ReceiptHandles = receiptHandles }).Call();
        }

        public BatchSendMessageApiResultModel BatchSendMessage(BatchSendMessageApiParameter messages)
        {
            var result = new BatchSendMessageApiRequest(this.Config, this.Name, messages).Call();

            return result.Result;
        }

        public string CreateQueue(QueueAttributeParameter parameter)
        {
            var result = new CreateQueueApiRequest(this.Config, this.Name, parameter).Call();

            return result.Result;
        }

        public void DeleteQueue()
        {
            new DeleteQueueApiRequest(this.Config, this.Name).Call();
        }

        public void SetQueueAttributes(QueueAttributeParameter parameter)
        {
            new SetQueueAttributesApiRequest(this.Config, this.Name, parameter).Call();
        }
    }
}