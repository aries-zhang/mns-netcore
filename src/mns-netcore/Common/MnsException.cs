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
}
