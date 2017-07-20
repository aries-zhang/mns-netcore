using Aliyun.MNS.Utility;
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Aliyun.MNS.Common;
using System.Collections.Generic;
using System.Threading;

namespace Aliyun.MNS.Sample.Consumer
{
    class Program
    {
        private static string Host = Environment.GetEnvironmentVariable("ALIYUN_MNS_HOST");
        private static string AccessKey = Environment.GetEnvironmentVariable("ALIYUN_MNS_ACCESSKEY");
        private static string AccessSecret = Environment.GetEnvironmentVariable("ALIYUN_MNS_ACCESSSECRET");
        
        static void Main(string[] args)
        {
            //var test = new BatchReceiveMessageModel()
            //{
            //    Messages = new ReceiveMessageModel[] {
            //        new ReceiveMessageModel(){ MessageId="1"},
            //        new ReceiveMessageModel(){ MessageId="2" }
            //    }.ToList()
            //};

            //Console.WriteLine(XmlSerdeUtility.Serialize(test));

            Console.WriteLine($"Host: {Host}");
            Console.WriteLine($"AccessKey: {AccessKey}");
            Console.WriteLine($"AccessSecret: {AccessSecret}");

            var Queue = MNS.Configure(Host, AccessKey, AccessSecret).Queue("aries-test");

            while (true)
            {
                var message = Queue.ReceiveMessage().Result.Result;

                Console.WriteLine("message: {0}, {1}", message.MessageId, message.MessageBody);
                Console.WriteLine("message processing..");
                Thread.Sleep(2000);

                Console.WriteLine("message processed");

                Queue.DeleteMessage(message.ReceiptHandle);

                Console.WriteLine("message deleted.");
            }
        }
    }
}