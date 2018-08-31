using Aliyun.MNS.Topic.Apis;
using Aliyun.MNS.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Aliyun.MNS.Tests
{
    public class PublishMessageParameterTest
    {
        [Fact]
        public void SerializationTest()
        {
            var parameter = new PublishMessageParameter();

            parameter.MessageBody = "test";
            parameter.MessageTag = "abc";
            parameter.MessageAttributes = null;

            var xml = XmlSerdeUtility.Serialize(parameter);

            Console.WriteLine(xml);
        }

        [Fact]
        public void SerializationTestWithMessageAttributesDirectMail()
        {
            var parameter = new PublishMessageParameter();

            parameter.MessageBody = "test";
            parameter.MessageTag = "abc";
            parameter.MessageAttributes = new TopicMessageAttributes()
            {
                DirectMail = "direct mail content"
            };

            var xml = XmlSerdeUtility.Serialize(parameter);

            Console.WriteLine(xml);
        }

        [Fact]
        public void SerializationTestWithMessageAttributesDirectSms()
        {
            var parameter = new PublishMessageParameter();

            parameter.MessageBody = "test";
            parameter.MessageTag = "abc";
            parameter.MessageAttributes = new TopicMessageAttributes()
            {
                DirectSMS = "test direct sms"
            };

            var xml = XmlSerdeUtility.Serialize(parameter);

            Console.WriteLine(xml);
        }
    }
}
