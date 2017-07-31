using Aliyun.MNS.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public interface IQueue
    {
        Task<QueueAttributeModel> GetAttributes();

        Task<SendMessageApiResultModel> SendMessage(string message, int delaySeconds = 0, int priority = 8);

        Task<ReceiveMessageModel> ReceiveMessage(int waitSeconds = 10);

        Task<BatchReceiveMessageModel> BatchReceiveMessage(int waitseconds = 10, int numOfMessages = 16);

        Task DeleteMessage(string receiptHandle);

        Task BatchDeleteMessage(List<string> receiptHandles);
    }
}