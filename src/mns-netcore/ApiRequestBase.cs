using Aliyun.MNS.Common;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public abstract class ApiRequestBase<R> where R : ApiResult
    {
        private static HttpClient Http = new HttpClient();

        private MnsConfig config { get; set; }
        private HttpRequestMessage request { get; set; }

        protected abstract string Path { get; }
        protected abstract HttpMethod Method { get; }
        protected virtual string Body { get; }

        public ApiRequestBase(MnsConfig config) : base()
        {
            this.config = config;
        }

        public R Call()
        {
            this.BuildRequest();

            var response = Http.SendAsync(this.request).Result;

            R result = (R)Activator.CreateInstance(typeof(R), response);
            result.Validate();

            return result;
        }

        protected virtual void AdditionalHeaders()
        {
        }

        private void BuildRequest()
        {
            this.request = new HttpRequestMessage(this.Method, $"{this.config.Endpoint}{this.Path}");
            this.request.Content = new StringContent(string.IsNullOrEmpty(this.Body) ? string.Empty : this.Body);
            this.request.Content.Headers.ContentType = new MediaTypeHeaderValue(MnsConstants.CONTENT_TYPE);

            var host = Regex.IsMatch(this.config.Endpoint.ToLower(), "^((http://)|(https://))") ? Regex.Replace(this.config.Endpoint.ToLower(), "^((http://)|(https://))", string.Empty) : this.config.Endpoint;
            this.request.Headers.Host = host;
            this.request.Headers.Add("Date", DateTime.UtcNow.ToString("r"));
            this.request.Headers.Add(MnsConstants.HTTP_HEADER_VERSION, MnsConstants.MNS_VERSION);

            this.AdditionalHeaders();

            var signature = Utility.CryptoUtility.SignRequest(this.request, this.config.AccessKeySecret);
            this.request.Headers.Authorization = new AuthenticationHeaderValue("MNS", $"{this.config.AccessKeyId}:{signature}");
        }
    }

    public abstract class ApiRequestBase<P, R> : ApiRequestBase<R> where P : ApiParameterBase where R : ApiResult
    {
        public P Parameter { get; set; }

        public ApiRequestBase(MnsConfig config, P parameter) : base(config)
        {
            this.Parameter = parameter;
        }
    }
}
