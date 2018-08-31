using Aliyun.MNS.Common;
using Aliyun.MNS.Utility;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Aliyun.MNS.Topic.Apis
{
    public class SubscribeApiRequest : ApiRequestBase<SubscriptionParameter, SubscribeApiResult>
    {
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }

        protected override string Path => $"/topics/{TopicName}/subscriptions/{SubscriptionName}";

        protected override HttpMethod Method => HttpMethod.Put;

        public SubscribeApiRequest(MnsConfig config, string topicName, string subscriptionName, SubscriptionParameter parameter) : base(config, parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            TopicName = topicName;
            SubscriptionName = subscriptionName;

            parameter.Validate();
        }

        protected override string Body => XmlSerdeUtility.Serialize(Parameter);
    }

    public class SubscribeApiResult : ApiResult<string>
    {
        public SubscribeApiResult(HttpResponseMessage response) : base(response)
        {
        }

        public override void Validate()
        {
            switch (Response.StatusCode)
            {
                case HttpStatusCode.Created:
                case HttpStatusCode.NoContent:
                    Result = Response.Headers.Location.ToString();
                    break;
                case HttpStatusCode.Conflict:
                    throw new SubscriptionAlreadyExists(ResponseText);
                case HttpStatusCode.BadRequest:
                    var error = XmlSerdeUtility.Deserialize<MnsError>(this.ResponseText);
                    if (error.Code.Equals("SubscriptionNameLengthError"))
                    {
                        throw new SubscriptionNameLengthErrorException(error);
                    }
                    else if (error.Code.Equals("SubscriptionNameInvalid"))
                    {
                        throw new SubscriptionNameInvalidException(error);
                    }
                    else if (error.Code.Equals("EndpointInvalid"))
                    {
                        throw new SubscriptionEndpointInvalidException(error);
                    }
                    else
                    {
                        throw new SubscriptionInvalidArgumentException(error);
                    }
                default:
                    throw new UnknowException(Response);
            }
        }
    }

    [XmlRoot(ElementName = "Subscription", Namespace = MnsConstants.MNS_XML_NS)]
    public class SubscriptionParameter : ApiParameterBase
    {
        [XmlElement]
        public string Endpoint { get; set; }

        [XmlElement]
        public string FilterTag { get; set; } = string.Empty;

        [XmlElement]
        public string NotifyStrategy { get; set; } = MnsConstants.TOPIC_NOTIFY_STRATEGY_BACKOFF_RETRY;

        [XmlElement]
        public string NotifyContentFormat { get; set; } = MnsConstants.TOPIC_NOTIFY_CONTENT_FORMAT_XML;

        public void Validate()
        {
            ValidateEndpoint();

            if (FilterTag.Length > 16)
            {
                throw new ArgumentOutOfRangeException("FilterTag", "FilterTag should be no more than 16 charactors");
            }

            var notifyStrategies = new string[] {
                MnsConstants.TOPIC_NOTIFY_STRATEGY_BACKOFF_RETRY,
                MnsConstants.TOPIC_NOTIFY_STRATEGY_EXPONENTIAL_DECAY_RETRY
            };
            if (!notifyStrategies.Contains(NotifyStrategy))
            {
                throw new ArgumentOutOfRangeException("NotifyStrategy", $"NotifyStrategy should be one of {string.Join(" or ", notifyStrategies)}");
            }

            var notifyContentFormats = new string[] {
                MnsConstants.TOPIC_NOTIFY_CONTENT_FORMAT_JSON,
                MnsConstants.TOPIC_NOTIFY_CONTENT_FORMAT_SIMPLIFIED,
                MnsConstants.TOPIC_NOTIFY_CONTENT_FORMAT_XML
            };
            if (!notifyContentFormats.Contains(NotifyContentFormat))
            {
                throw new ArgumentOutOfRangeException("NotifyContentFormat", $"NotifyContentFormat should be one of {string.Join(" or ", notifyContentFormats)}");
            }
        }

        private void ValidateEndpoint()
        {
            if (string.IsNullOrEmpty(Endpoint))
            {
                throw new ArgumentException("Endpoint is required");
            }

            // TODO: 1. Regex, 2. optimize boolean operations
            if (!Endpoint.StartsWith("http://")
                && !Regex.Match(Endpoint, "acs:mns:{REGION}:{AccountID}:queues/{QueueName}").Success
                && !Regex.Match(Endpoint, "mail:directmail:{MailAddress}").Success
                && !Regex.Match(Endpoint, "sms:directsms:{Phone}").Success
                && !Endpoint.Equals("sms:directsms:anonymous", StringComparison.Ordinal))
            {
                throw new ArgumentException("Endpoint is invalid");
            }
        }
    }
}
