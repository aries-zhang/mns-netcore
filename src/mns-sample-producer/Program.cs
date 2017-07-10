using Iyibank.Aliyun.MNS;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Http;

namespace Aliyun.MNS.Sample.Producer
{
    class Program
    {
        private static string Host = Environment.GetEnvironmentVariable("ALIYUN_MNS_HOST");
        private static string AccessKey = Environment.GetEnvironmentVariable("ALIYUN_MNS_ACCESSKEY");
        private static string AccessSecret = Environment.GetEnvironmentVariable("ALIYUN_MNS_ACCESSSECRET");

        //private static Queue mns = new MNS(AccessKey, AccessSecret).GetQueue("test");

        static void Main(string[] args)
        {
            //mns.SendMessage(new QueueMessage()
            //{

            //});

            //var uri = new Uri("http://1162928508897603.mns.cn-hangzhou-internal.aliyuncs.com/queue/abc/a=1&b=2&c=3");
            //Console.WriteLine(uri.PathAndQuery);

            var mns = new MNS(Host, AccessKey, AccessSecret);
            var result = mns.GetQueueAttributes("MetalMessageQueue").Result;

            var result2 = mns.SendMessage("MetalMessageQueue", "test").Result;

            //var helper = new MQHelper(Host, AccessKey, AccessSecret);
            //var result = helper.Get("MetalMessageQueue").Result;

            Console.WriteLine("Press any key to continue..");
            Console.Read();
        }
    }

    class FuckMns
    {
        private string url;
        private string accessKeyId;
        private string accessKeySecret;

        private string host;
        private string version = "2015-06-06";
        private string contentType = "text/xml";

        private HttpClient client;

        public FuckMns(string url, string accessKeyId, string accessKeySecret)
        {
            this.url = url;
            this.accessKeyId = accessKeyId;
            this.accessKeySecret = accessKeySecret;

            this.host = url.StartsWith("http://") ? url.Substring(7) : url;

            //this.client = new HttpClient();
            //this.client.DefaultRequestHeaders.Add();
        }


        public string Get(string queue)
        {
            //var path = $"/queues/{queue}";
            //var method = "GET";

            //var fullUrl = $"{url}{path}";

            //var request = (HttpWebRequest)WebRequest.Create(fullUrl);
            //request.Method = "GET";
            //request.ContentType = contentType;
            //request.Headers.

            throw new NotImplementedException();
        }

        public string auth(string method, string contentType, string date, Dictionary<string, string> mnsHeaders, string resource, string accessKey, string accessSecret)
        {
            var sign = this.sign(method, contentType, date, mnsHeaders, resource, accessSecret);

            var auth = $"MNS {accessKey}:{sign}";

            Console.WriteLine(auth);

            return auth;
        }

        public string sign(string method, string contentType, string date, Dictionary<string,string> mnsHeaders, string resource, string secret)
        {
            /*
             HTTP_METHOD + "\n" 
                + CONTENT-MD5 + "\n"     
                + CONTENT-TYPE + "\n" 
                + DATE + "\n" 
                + CanonicalizedMNSHeaders
                + CanonicalizedResource
            */
            string format = method.ToUpper() + "\n"
                  + "\n"
                + contentType + "\n"
                + date + "\n";

            foreach (var key in mnsHeaders.Keys.OrderBy(k => k))
            {
                format += $"{key}:${mnsHeaders[key]}\n";
            }

            format += resource;

            Console.WriteLine(format);

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
            var result = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(format)));

            Console.WriteLine(result);

            return result;
        }
    }
}