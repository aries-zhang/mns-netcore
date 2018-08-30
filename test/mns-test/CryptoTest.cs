using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xunit;

namespace Aliyun.MNS.Tests
{
    public class CryptoTest
    {
        private MnsConfig config = new MnsConfig()
        {
            Endpoint = "http://1234.mns.cn-hangzhou.aliyuncs.com",
            AccessKeyId = "1234",
            AccessKeySecret = "abcd"
        };

        [Fact]
        public void RequestSignTest()
        {
            string queueName = "test-q";
            string path = $"/queues/{queueName}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"{config.Endpoint}{path}");

            request.Content = new StringContent(string.Empty);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(MnsConstants.CONTENT_TYPE);

            request.Headers.Host = config.EndpointWithoutProtocol();
            request.Headers.Add("Date", new DateTime(2018, 8, 30, 0, 0, 1, DateTimeKind.Utc).ToString("r"));
            request.Headers.Add(MnsConstants.HTTP_HEADER_VERSION, MnsConstants.MNS_VERSION);

            request.AliCloudSign(config);

            Assert.Equal("MNS", request.Headers.Authorization.Scheme);
            Assert.Equal("25GEzMEozafqStgPCTLTKP6x0Gk=", request.Headers.Authorization.Parameter.Split(':')[1]);
        }
    }
}
