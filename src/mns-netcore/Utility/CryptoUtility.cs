using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http.Headers;

namespace Aliyun.MNS.Utility
{
    public static class CryptoUtility
    {
        public static string SignRequest(HttpRequestMessage request, string key)
        {
            List<string> list = new List<string>();

            list.Add(request.Method.ToString());
            list.Add(request.Content.Headers.ContentMD5 == null ? string.Empty : Convert.ToBase64String(request.Content.Headers.ContentMD5));
            list.Add(request.Content.Headers.ContentType.ToString());
            list.Add(request.Headers.Date.Value.UtcDateTime.ToString("r"));

            foreach (var header in request.Headers.Where(kv => kv.Key.StartsWith("x-mns-")).OrderBy(kv => kv.Key))
            {
                list.Add(string.Format("{0}:{1}", header.Key, header.Value.Last()));
            }

            list.Add(request.RequestUri.PathAndQuery);

            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            string raw = string.Join("\n", list);
            var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(raw)));

            return hash;
        }
        
        public static void AliCloudSign(this HttpRequestMessage message, MnsConfig config)
        {
            string signature = SignRequest(message, config.AccessKeySecret);

            message.Headers.Authorization = new AuthenticationHeaderValue("MNS", $"{config.AccessKeyId}:{signature}");
        }
    }
}
