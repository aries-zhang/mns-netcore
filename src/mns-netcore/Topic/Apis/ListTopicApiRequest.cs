using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Serialization;

namespace Aliyun.MNS.Topic.Apis
{
    public class ListTopicApiRequest : ApiRequestBase<ListTopicApiResult>
    {
        public string Prefix { get; set; } = string.Empty;

        public int PageSize { get; set; } = 0;

        public string NextMarker { get; set; } = string.Empty;

        protected override string Path => "/topics";

        protected override HttpMethod Method => HttpMethod.Get;
        
        public ListTopicApiRequest(MnsConfig config, string prefix, int pageSize, string nextMarker) : base(config)
        {
            if (pageSize <= 0 || pageSize > 1000)
            {
                throw new InvalidArgumentException("Page size should be in range 1 - 10000");
            }

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

    public class ListTopicApiResult : ApiResult<TopicListModel>
    {
        public ListTopicApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (Response.StatusCode)
            {
                case HttpStatusCode.OK:
                    Result = XmlSerdeUtility.Deserialize<TopicListModel>(ResponseText);
                    break;
                default:
                    throw new UnknowException(Response);
            }
        }
    }

    [XmlRoot(ElementName = "Topics", Namespace = MnsConstants.MNS_XML_NS)]
    public class TopicListModel
    {
        [XmlElement(ElementName = "Topic")]
        public List<TopicListItem> Items { get; set; }

        [XmlElement(ElementName = "NextMarker")]
        public string NextMarker { get; set; }
    }

    public class TopicListItem
    {
        public string TopicURL { get; set; }
    }
}
