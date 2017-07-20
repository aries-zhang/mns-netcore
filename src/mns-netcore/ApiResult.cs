using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS
{
    public class ApiResult
    {
        protected HttpResponseMessage Response { get; private set; }

        public string ResponseText { get; set; }

        public MnsError Error { get; set; }

        public ApiResult(HttpResponseMessage response)
        {
            if (response != null)
            {
                this.Response = response;
                this.ResponseText = response.Content.ReadAsStringAsync().Result;

                if (this.Response.StatusCode == HttpStatusCode.NotFound || this.Response.StatusCode == HttpStatusCode.BadRequest || this.Response.StatusCode == HttpStatusCode.Conflict)
                {
                    this.Error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                    this.Error.StatusCode = this.Response.StatusCode;
                }
            }
        }
    }

    public class ApiResult<T> : ApiResult where T : class
    {
        public T Result { get; set; }

        public ApiResult(HttpResponseMessage response) : base(response)
        {
            if (this.Response.StatusCode == HttpStatusCode.OK)
            {
                this.Result = XmlSerdeUtility.Deserialize<T>(this.ResponseText);
            }
        }
    }
}
