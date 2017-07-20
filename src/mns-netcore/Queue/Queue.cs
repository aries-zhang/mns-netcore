using Aliyun.MNS.Apis.Queue;
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

        public async Task<ApiResult<QueueAttributeModel>> GetAttributes()
        {
            var request = new GetQueueAttributeApiRequest(this.Config, this.Name);
            return await request.Call();
        }

        public async Task<ApiResult<SendMessageApiResultModel>> SendMessage(string message, int delaySeconds = 0, int priority = 8)
        {
            return await new SendMessageApiRequest(this.Config, this.Name, message, delaySeconds: delaySeconds, priority: priority).Call();
        }

        public async Task<ApiResult<ReceiveMessageModel>> ReceiveMessage(int waitSeconds = 10)
        {
            var request = new ReceiveMessageApiRequest(this.Config, this.Name, waitSeconds: waitSeconds);

            var result = await request.Call();

            while (result.Error != null && string.Equals(result.Error.Code, "MessageNotExist", StringComparison.OrdinalIgnoreCase))
            {
                result = await request.Call();
            }

            return result;
        }

        public async Task<ApiResult<BatchReceiveMessageModel>> BatchReceiveMessage(int waitseconds = 10, int numOfMessages = 16)
        {
            var request = new BatchReceiveMessageApiRequest(this.Config, this.Name, waitseconds: waitseconds, numOfMessages: numOfMessages);

            var result = await request.Call();

            while (result.Error != null && string.Equals(result.Error.Code, "MessageNotExist", StringComparison.OrdinalIgnoreCase))
            {
                result = await request.Call();
            }

            return result;
        }

        public async Task<ApiResult> DeleteMessage(string receiptHandle)
        {
            return await new DeleteMessageApiRequest(this.Config, this.Name, receiptHandle).Call();
        }

        public async Task<ApiResult> BatchDeleteMessage(List<string> receiptHandles)
        {
            return await new BatchDeleteMessageApiRequest(this.Config, this.Name, new BatchDeleteMessageApiParameter() { ReceiptHandles = receiptHandles }).Call();
        }
    }
}
