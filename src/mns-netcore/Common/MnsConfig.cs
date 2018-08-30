using Aliyun.MNS.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliyun.MNS
{
    public class MnsConfig
    {
        public string Endpoint { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }

        public string EndpointWithoutProtocol()
        {
            var host = this.Endpoint.ToLower();
            var protocol = MnsConstants.HTTP_PROTOCOL_REGEX;

            return Regex.IsMatch(host, protocol) ? Regex.Replace(host, protocol, string.Empty) : host;
        }
    }
}
