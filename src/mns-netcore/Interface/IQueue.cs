using Aliyun.MNS.Model;
using System.Collections.Generic;

namespace Aliyun.MNS
{
    public interface IQueue
    {
        QueueAttributeModel GetAttributes();

        SendMessageApiResultModel SendMessage(string message, int delaySeconds = 0, int priority = 8);

        BatchSendMessageApiResultModel BatchSendMessage(BatchSendMessageApiParameter messages);

        ReceiveMessageModel ReceiveMessage(int waitSeconds = 10);

        BatchReceiveMessageModel BatchReceiveMessage(int waitseconds = 10, int numOfMessages = 16);

        void DeleteMessage(string receiptHandle);

        void BatchDeleteMessage(List<string> receiptHandles);

        string CreateQueue(QueueAttributeParameter parameter = null);

        void SetQueueAttributes(QueueAttributeParameter parameter);

        void DeleteQueue();

        ReceiveMessageModel PeekMessage();

        BatchReceiveMessageModel BatchPeekMessage(int numOfMessages = 16);

        ChangeVisibilityModel ChangeMessageVisibility(string receiptHandle, int visibilityTimeout);
    }
}