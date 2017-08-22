using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;

namespace Aliyun.MNS
{
    public class ChangeMessageVisibilityApiRequest : ApiRequestBase<ChangeMessageVisibilityApiResult>
    {
        public string QueueName { get; set; }

        public string ReceiptHandle { get; set; }

        public int VisibilityTimeout { get; set; }

        protected override string Path => $"/queues/{this.QueueName}/messages?receiptHandle={this.ReceiptHandle}&visibilityTimeout={this.VisibilityTimeout}";

        protected override HttpMethod Method => HttpMethod.Put;

        public ChangeMessageVisibilityApiRequest(MnsConfig config, string queueName, string receiptHandle, int visibilityTimeout) : base(config)
        {
            this.QueueName = queueName;
            this.ReceiptHandle = receiptHandle;
            this.VisibilityTimeout = visibilityTimeout;
        }
    }

    public class ChangeMessageVisibilityApiResult : ApiResult<ChangeVisibilityModel>
    {
        public ChangeMessageVisibilityApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.OK:
                    this.Result = XmlSerdeUtility.Deserialize<ChangeVisibilityModel>(this.ResponseText);
                    break;
                case HttpStatusCode.NotFound:
                    {
                        var error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                        throw error.Code == "QueueNotExist" ? (MnsException)new QueueNotExistException(error) : new MessageNotExistException(error);
                    }
                case HttpStatusCode.BadRequest:
                    throw new InvalidArgumentException(this.ResponseText);
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }

    [XmlRoot(ElementName = "ChangeVisibility", Namespace = MnsConstants.MNS_XML_NS)]
    public class ChangeVisibilityModel
    {
        [XmlElement]
        public string ReceiptHandle { get; set; }

        [XmlElement]
        public long NextVisibleTime { get; set; }
    }
}
