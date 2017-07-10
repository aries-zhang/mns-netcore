using Aliyun.MNS.Apis.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public class MNS
    {
        private MnsConfig config { get; set; }

        /* Queue 的操作：CreateQueue，DeleteQueue，ListQueue，GetQueueAttributes，SetQueueAttributes.
         * Message 的操作：SendMessage，BatchSendMessage，ReceiveMessage，BatchReceiveMessage，PeekMessage，BatchPeekMessage，DeleteMessage，BatchDeleteMessage，ChangeMessageVisibility.
         */
        public MNS(string host, string accessKeyId, string accessKeySecret)
        {
            this.config = new MnsConfig()
            {
                Endpoint = host,
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessKeySecret
            };
        }

        public async Task<GetQueueAttributeResult> GetQueueAttributes(string name)
        {
            return await new GetQueueAttributeApiRequest(this.config, name).Call();
        }

        //public ListQueueResult ListQueue()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<SendMessageApiResult> SendMessage(string queue, string message)
        {
            return await new SendMessageApiRequest(this.config, queue, message).Call();
        }

        //public PeekMessageResult PeekMessage()
        //{
        //    throw new NotImplementedException();
        //}

        //public DeleteMessageResult DeleteMessage()
        //{
        //    throw new NotImplementedException();
        //}

        //public ReceiveMessageResult ReceiveMessage()
        //{
        //    throw new NotImplementedException();
        //}
    }
}


namespace Iyibank.Aliyun.MNS
{
    public class MQHelper
    {
        private string url;
        private string accessKeyId;
        private string accessKeySecret;

        private string host;
        private string version = "2015-06-06";

        public MQHelper(string url, string accessKeyId, string accessKeySecret)
        {
            this.url = url;
            this.accessKeyId = accessKeyId;
            this.accessKeySecret = accessKeySecret;

            this.host = url.StartsWith("http://") ? url.Substring(7) : url;

        }
        /// <summary>
        /// URL 中的 Key，Tag以及 POST Content-Type 没有任何的限制，只要确保Key 和 Tag 相同唯一即可
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<bool> Pub(string name, string body)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("Host", this.host);
                    headers.Add("Date", DateTime.Now.ToUniversalTime().ToString("r"));
                    headers.Add("x-mns-version", this.version);
                    headers["Content-Type"] = "text/xml";
                    string url = string.Format("{0}/{1}", name, "messages");
                    headers.Add("Authorization", this.authorization("POST", headers, string.Format("{0}", "/queues/" + name + "/messages")));

                    foreach (var kv in headers)
                    {
                        if (kv.Key != "Content-Type")
                        {
                            httpClient.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                        }

                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" <Message> ");
                    sb.Append("<MessageBody>" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(body)) + "</MessageBody> ");
                    sb.Append("<DelaySeconds>0</DelaySeconds> ");
                    sb.Append(" <Priority>1</Priority>");
                    sb.Append("</Message>");
                    HttpContent content = new StringContent(sb.ToString());
                    content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                    httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                    var res = await httpClient.PostAsync(this.url + "/" + string.Format("queues/{0}/{1}", name, "messages"), content);
                    if (res.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch { return false; }
        }
        /// <summary>
        /// URL 中的 Key，Tag以及 POST Content-Type 没有任何的限制，只要确保Key 和 Tag 相同唯一即可
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<bool> Get(string name)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("Host", this.host);
                    headers.Add("Date", DateTime.Now.ToUniversalTime().ToString("r"));
                    headers.Add("x-mns-version", this.version);
                    headers["Content-Type"] = "text/xml";
                    string url = string.Format("{0}/{1}", name, "messages");
                    headers.Add("Authorization", this.authorization("POST", headers, string.Format("{0}", "/queues/" + name + "/messages")));

                    foreach (var kv in headers)
                    {
                        if (kv.Key != "Content-Type")
                        {
                            httpClient.DefaultRequestHeaders.Add(kv.Key, kv.Value);
                        }

                    }
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    HttpContent content = new StringContent("");
                    content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                    httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
                    var res = await httpClient.GetAsync(this.url + "/" + string.Format("queues/{0}", name));
                    Console.WriteLine(res.Content.ReadAsStringAsync().Result);
                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch { return false; }
        }
        /// <summary>
        /// 生成验证信息
        /// </summary>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private string authorization(string method, Dictionary<string, string> headers, string resource)
        {
            var auth = string.Format("MNS {0}:{1}", this.accessKeyId, this.signature("POST", headers, resource));

            return auth;
        }
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private string signature(string method, Dictionary<string, string> headers, string resource)
        {
            List<string> toSign = new List<string>();
            toSign.Add(method.ToString());
            toSign.Add(headers.ContainsKey("Content-MD5") ? headers["Content-MD5"] : string.Empty);
            toSign.Add(headers.ContainsKey("Content-Type") ? headers["Content-Type"] : string.Empty);
            toSign.Add(headers.ContainsKey("Date") ? headers["Date"] : DateTime.Now.ToUniversalTime().ToString("r"));

            foreach (KeyValuePair<string, string> header in headers.Where(kv => kv.Key.StartsWith("x-mns-")).OrderBy(kv => kv.Key))
            {
                toSign.Add(string.Format("{0}:{1}", header.Key, header.Value));
            }

            toSign.Add(resource);

            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(this.accessKeySecret));
            string key = string.Join("\n", toSign);
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(string.Join("\n", toSign)));
            return Convert.ToBase64String(hashBytes);
        }
    }
}