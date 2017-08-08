using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Serialization;

namespace Aliyun.MNS
{
    public class ListQueueApiRequest : ApiRequestBase<ListQueueApiResult>
    {
        public string Prefix { get; set; } = string.Empty;

        public int PageSize { get; set; } = 0;

        public string NextMarker { get; set; } = string.Empty;

        protected override string Path => "/queues";

        protected override HttpMethod Method => HttpMethod.Get;

        public ListQueueApiRequest(MnsConfig config, string prefix, int pageSize, string nextMarker) : base(config)
        {
            this.Prefix = prefix;
            this.PageSize = pageSize;
            this.NextMarker = nextMarker;
        }

        protected override void AdditionalHeaders(HttpRequestHeaders headers)
        {
            if (!string.IsNullOrEmpty(this.Prefix))
            {
                headers.Add("x-mns-prefix", this.Prefix);
            }

            if (this.PageSize > 0)
            {
                headers.Add("x-mns-ret-number", this.PageSize.ToString());
            }

            if (!string.IsNullOrEmpty(this.NextMarker))
            {
                headers.Add("x-mns-marker", this.NextMarker);
            }
        }
    }

    public class ListQueueApiResult : ApiResult<QueueListModel>
    {
        public ListQueueApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (this.Response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    this.Result = XmlSerdeUtility.Deserialize<QueueListModel>(this.ResponseText);
                    break;
                default:
                    throw new UnknowException(this.Response);
            }
        }
    }

    [XmlRoot(ElementName = "Queues", Namespace = MnsConstants.MNS_XML_NS)]
    public class QueueListModel
    {
        [XmlElement(ElementName = "Queue")]
        public List<QueueListItem> Items { get; set; }

        [XmlElement(ElementName = "NextMarker")]
        public string NextMarker { get; set; }
    }

    public class QueueListItem
    {
        public string QueueURL { get; set; }
    }
}