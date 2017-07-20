using Aliyun.MNS.Apis.Queue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public interface IQueue
    {
        Task<ApiResult<QueueAttributeModel>> GetAttributes();

        Task<ApiResult<SendMessageApiResultModel>> SendMessage(string message, int delaySeconds = 0, int priority = 8);

        Task<ApiResult<ReceiveMessageModel>> ReceiveMessage(int waitSeconds = 10);

        Task<ApiResult<BatchReceiveMessageModel>> BatchReceiveMessage(int waitseconds = 10, int numOfMessages = 16);

        Task<ApiResult> DeleteMessage(string receiptHandle);

        Task<ApiResult> BatchDeleteMessage(List<string> receiptHandles);
    }
}