using Aliyun.MNS.Common;
using System;
using System.Linq;
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
            Console.WriteLine($"Host: {Host}");
            Console.WriteLine($"AccessKey: {AccessKey}");
            Console.WriteLine($"AccessSecret: {AccessSecret}");

            var Queue = MNS.Configure(Host, AccessKey, AccessSecret).Queue("aries-test");

            while (true)
            {
                try
                {
                    var message = Queue.BatchReceiveMessage().Result;

                    Console.WriteLine("message [{0}]:", message.Messages.Count);
                    foreach (var msg in message.Messages)
                    {
                        Console.WriteLine("\t{0}: {1}", msg.MessageId, msg.MessageBody);
                    }
                    Console.WriteLine("message processing..");
                    Thread.Sleep(2000);

                    Console.WriteLine("message processed");

                    Queue.BatchDeleteMessage(message.Messages.Select(m => m.ReceiptHandle).ToList());

                    Console.WriteLine("message deleted.");
                }
                catch (Exception ex)
                {
                    if (ex is MessageNotExistException)
                        continue;
                    else
                        Console.WriteLine("{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }
    }
}