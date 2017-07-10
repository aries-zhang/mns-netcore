using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS
{
    public abstract class ApiResultBase
    {
        protected HttpResponseMessage Response { get; private set; }

        public string ResponseText { get; set; }

        public MnsError Error { get; set; }

        public ApiResultBase(HttpResponseMessage response)
        {
            if (response != null)
            {
                this.Response = response;
                this.ResponseText = response.Content.ReadAsStringAsync().Result;
            }
        }

        protected virtual T ResolveResult<T>() where T : class
        {
            switch (this.Response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return XmlSerdeUtility.Deserialize<T>(this.ResponseText);
                default:
                    return null;
            }
        }
    }
}
