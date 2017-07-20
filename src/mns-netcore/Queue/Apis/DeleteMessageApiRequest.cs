using System.Net.Http;

namespace Aliyun.MNS
{
    public class DeleteMessageApiRequest : ApiRequestBase<ApiResult>
    {
        private MnsConfig config;
        private string queneName;
        private string receiptHandle;

        public DeleteMessageApiRequest(MnsConfig config, string queneName, string receiptHandle) : base(config)
        {
            this.config = config;
            this.queneName = queneName;
            this.receiptHandle = receiptHandle;
        }

        protected override string Path => $"/queues/{this.queneName}/messages?ReceiptHandle={this.receiptHandle}";

        protected override HttpMethod Method => HttpMethod.Delete;
    }
}