using Aliyun.MNS.Apis.Queue;
using Aliyun.MNS.Model;
using System;
using System.Collections.Generic;
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
            this.Config = config;
            this.Name = name;
        }

        public async Task<QueueAttributeModel> GetAttributes()
        {
            var response = await new GetQueueAttributeApiRequest(this.Config, this.Name).Call();

            return response.Result;
        }

        public async Task<SendMessageApiResultModel> SendMessage(string message, int delaySeconds = 0, int priority = 8)
        {
            var response = await new SendMessageApiRequest(this.Config, this.Name, message, delaySeconds: delaySeconds, priority: priority).Call();

            return response.Result;
        }

        public async Task<ReceiveMessageModel> ReceiveMessage(int waitSeconds = 10)
        {
            var response = await new ReceiveMessageApiRequest(this.Config, this.Name, waitSeconds: waitSeconds).Call();

            return response.Result;
        }

        public async Task<BatchReceiveMessageModel> BatchReceiveMessage(int waitseconds = 10, int numOfMessages = 16)
        {
            var request = new BatchReceiveMessageApiRequest(this.Config, this.Name, waitseconds: waitseconds, numOfMessages: numOfMessages);

            var result = await request.Call();

            return result.Result;
        }

        public async Task DeleteMessage(string receiptHandle)
        {
            await new DeleteMessageApiRequest(this.Config, this.Name, receiptHandle).Call();
        }

        public async Task BatchDeleteMessage(List<string> receiptHandles)
        {
            await new BatchDeleteMessageApiRequest(this.Config, this.Name, new BatchDeleteMessageApiParameter() { ReceiptHandles = receiptHandles }).Call();
        }
    }
}