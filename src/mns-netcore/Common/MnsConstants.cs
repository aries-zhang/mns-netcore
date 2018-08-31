using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.MNS.Common
{
    public static class MnsConstants
    {
        public const string MNS_VERSION = "2015-06-06";
        public const string CONTENT_TYPE = "text/xml";

        public const string HTTP_HEADER_VERSION = "x-mns-version";

        public const string MNS_XML_NS = "http://mns.aliyuncs.com/doc/v1";

        public const string HTTP_PROTOCOL_REGEX = "^((http://)|(https://))";

        public const string TOPIC_NOTIFY_STRATEGY_BACKOFF_RETRY = "BACKOFF_RETRY ";

        public const string TOPIC_NOTIFY_STRATEGY_EXPONENTIAL_DECAY_RETRY = "EXPONENTIAL_DECAY_RETRY";

        public const string TOPIC_NOTIFY_CONTENT_FORMAT_XML = "XML";

        public const string TOPIC_NOTIFY_CONTENT_FORMAT_JSON = "JSON";

        public const string TOPIC_NOTIFY_CONTENT_FORMAT_SIMPLIFIED = "SIMPLIFIED";
    }
}
