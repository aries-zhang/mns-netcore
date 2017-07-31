using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS
{
    public class ApiResult
    {
        protected HttpResponseMessage Response { get; private set; }

        public string ResponseText { get; set; }

        public ApiResult(HttpResponseMessage response)
        {
            if (response != null)
            {
                this.Response = response;
                this.ResponseText = response.Content.ReadAsStringAsync().Result;
            }
        }
    }

    public class ApiResult<T> : ApiResult where T : class
    {
        public T Result { get; set; }

        public ApiResult(HttpResponseMessage response) : base(response)
        {
            //if (this.Response.StatusCode == HttpStatusCode.OK || this.Response.StatusCode == HttpStatusCode.NoContent || this.Response.StatusCode == HttpStatusCode.Created)
            //{
            //    this.Result = XmlSerdeUtility.Deserialize<T>(this.ResponseText);
            //}
        }
    }
}
