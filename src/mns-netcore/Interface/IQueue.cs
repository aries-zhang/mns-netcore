using Aliyun.MNS.Apis.Queue;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public interface IQueue
    {
        Task<BatchReceiveMessageApiResult> BatchReceiveMessage();
        Task<DeleteMessageApiResult> DeleteMessage(string receiptHandle);
        Task<GetQueueAttributeResult> GetAttributes();
        Task<ReceiveMessageApiResult> ReceiveMessage(int waitSeconds = 10);
        Task<SendMessageApiResult> SendMessage(string message, int delaySeconds = 0, int priority = 8);
    }
}