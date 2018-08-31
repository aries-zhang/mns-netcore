using Aliyun.MNS.Utility;
using System;
using System.Net.Http;

namespace Aliyun.MNS.Common
{
    public class MnsException : Exception
    {
        public MnsError Error { get; set; }

        public MnsException(MnsError error)
        {
            this.Error = error;
        }

        public MnsException(string errorMessage)
        {
            this.Error = XmlSerdeUtility.Deserialize<MnsError>(errorMessage);
        }
    }

    public class UnknowException : MnsException
    {
        public HttpResponseMessage Response;

        public UnknowException(HttpResponseMessage response) : base(new MnsError() { Code = "Unknown", Message = "Unknown error" })
        {
        }
    }

    public class QueueAlreadyExistException : MnsException
    {
        public QueueAlreadyExistException(string errorMessage) : base(errorMessage)
        {
        }

        public QueueAlreadyExistException(MnsError error) : base(error)
        {
        }
    }

    public class QueueNumExceededLimitException : MnsException
    {
        public QueueNumExceededLimitException(string errorMessage) : base(errorMessage)
        {
        }

        public QueueNumExceededLimitException(MnsError error) : base(error)
        {
        }
    }

    public class QueueNotExistException : MnsException
    {
        public QueueNotExistException(string errorMessage) : base(errorMessage)
        {
        }

        public QueueNotExistException(MnsError error) : base(error)
        {
        }
    }

    public class MalformedXMLException : MnsException
    {
        public MalformedXMLException(MnsError error) : base(error)
        {
        }
    }

    public class InvalidArgumentException : MnsException
    {
        public InvalidArgumentException(string errorMessage) : base(errorMessage)
        {
        }

        public InvalidArgumentException(MnsError error) : base(error)
        {
        }
    }

    public class MessageNotExistException : MnsException
    {
        public MessageNotExistException(MnsError error) : base(error)
        {
        }
    }

    public class ReceiptHandleErrorException : MnsException
    {
        public ReceiptHandleErrorException(MnsError error) : base(error)
        {
        }
    }

    public class TopicAlreadyExistException : MnsException
    {
        public TopicAlreadyExistException(string errorMessage) : base(errorMessage)
        {
        }

        public TopicAlreadyExistException(MnsError error) : base(error)
        {
        }
    }

    public class TopicNameLengthErrorException : MnsException
    {
        public TopicNameLengthErrorException(string errorMessage) : base(errorMessage)
        {
        }

        public TopicNameLengthErrorException(MnsError error) : base(error)
        {
        }
    }

    public class TopicNotExistException : MnsException
    {
        public TopicNotExistException(string errorMessage) : base(errorMessage)
        {
        }

        public TopicNotExistException(MnsError error) : base(error)
        {
        }
    }

    public class SubscriptionAlreadyExists : MnsException
    {
        public SubscriptionAlreadyExists(string errorMessage) : base(errorMessage)
        {
        }

        public SubscriptionAlreadyExists(MnsError error) : base(error)
        {
        }
    }

    public class SubscriptionNameLengthErrorException : MnsException
    {
        public SubscriptionNameLengthErrorException(string errorMessage) : base(errorMessage)
        {
        }

        public SubscriptionNameLengthErrorException(MnsError error) : base(error)
        {
        }
    }

    public class SubscriptionNameInvalidException : MnsException
    {
        public SubscriptionNameInvalidException(string errorMessage) : base(errorMessage)
        {
        }

        public SubscriptionNameInvalidException(MnsError error) : base(error)
        {
        }
    }

    public class SubscriptionEndpointInvalidException : MnsException
    {
        public SubscriptionEndpointInvalidException(string errorMessage) : base(errorMessage)
        {
        }

        public SubscriptionEndpointInvalidException(MnsError error) : base(error)
        {
        }
    }

    public class SubscriptionInvalidArgumentException : MnsException
    {
        public SubscriptionInvalidArgumentException(string errorMessage) : base(errorMessage)
        {
        }

        public SubscriptionInvalidArgumentException(MnsError error) : base(error)
        {
        }
    }    

    public class SubscriptionNotExistException : MnsException
    {
        public SubscriptionNotExistException(string errorMessage) : base(errorMessage)
        {
        }

        public SubscriptionNotExistException(MnsError error) : base(error)
        {
        }
    }
}
