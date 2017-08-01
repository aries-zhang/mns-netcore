using Aliyun.MNS.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public interface IQueue
    {
        QueueAttributeModel GetAttributes();

        SendMessageApiResultModel SendMessage(string message, int delaySeconds = 0, int priority = 8);

        ReceiveMessageModel ReceiveMessage(int waitSeconds = 10);

        BatchReceiveMessageModel BatchReceiveMessage(int waitseconds = 10, int numOfMessages = 16);

        void DeleteMessage(string receiptHandle);

        void BatchDeleteMessage(List<string> receiptHandles);
    }
}