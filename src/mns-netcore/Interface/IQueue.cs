using Aliyun.MNS.Apis.Queue;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public interface IQueue
    {
        Task<ApiResult<QueueAttributeModel>> GetAttributes();

        Task<ApiResult<SendMessageApiResultModel>> SendMessage(string message, int delaySeconds = 0, int priority = 8);

        Task<ApiResult<ReceiveMessageModel>> ReceiveMessage(int waitSeconds = 10);

        Task<ApiResult<BatchReceiveMessageModel>> BatchReceiveMessage();

        Task<ApiResult> DeleteMessage(string receiptHandle);
    }
}