using Aliyun.MNS.Common;
using Aliyun.MNS.Model;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace Aliyun.MNS.Sample.Producer
{
    class Program
    {

        static void Main(string[] args)
        {
            string Host = Environment.GetEnvironmentVariable("ALIYUN_MNS_HOST");
            string AccessKey = Environment.GetEnvironmentVariable("ALIYUN_MNS_ACCESSKEY");
            string AccessSecret = Environment.GetEnvironmentVariable("ALIYUN_MNS_ACCESSSECRET");
            string TestQueue = "aries-test";

            Console.WriteLine($"Host: {Host}");
            Console.WriteLine($"AccessKey: {AccessKey}");
            Console.WriteLine($"AccessSecret: {AccessSecret}");

            var mns = MNS.Configure(Host, AccessKey, AccessSecret);

            IQueue queue = mns.Queue(TestQueue);

            if (mns.ListQueue().Items.Where(q => q.QueueURL.Contains(TestQueue)).Count() > 0)
            {
                queue.DeleteQueue();
            }

            queue.CreateQueue();
            
            for (int i = 0; i < 8; i++)
            {
                var message = args.Length >= 1 ? args[0] + i : "this is test message #" + i;
                var result = queue.SendMessage(message);
                Console.WriteLine($"SEND: [{result.MessageId}] - {message}");
            }

            var messages = new BatchSendMessageApiParameter()
            {
                Messages = Enumerable.Range(0, 11).Select(i => new SendMessageApiParameter() { MessageBody = Convert.ToBase64String(Encoding.UTF8.GetBytes($"batch test #{i}")) }).ToList()
            };

            queue.BatchSendMessage(messages);

            Console.WriteLine($"BATCH SEND: {messages.Messages.Count} messages");

            Thread.Sleep(2000);

            var peakedMessage = queue.PeekMessage();

            Console.WriteLine($"PEAK: [{peakedMessage.ReceiptHandle}] - {peakedMessage.MessageBody}");

            var batchPeakedMessages = queue.BatchPeekMessage(10);
            foreach (var message in batchPeakedMessages.Messages)
            {
                Console.WriteLine($"BATCH PEAK: {message.MessageBody}");
            }

            while (true)
            {
                try
                {
                    var message = queue.ReceiveMessage();
                    Console.WriteLine($"Single Receive: {message.MessageBody}");

                    Console.WriteLine("Changing visible time for this message.");
                    var changeResult = queue.ChangeMessageVisibility(message.ReceiptHandle, 600);
                    Console.WriteLine($"NEXT VISIBLE: {changeResult.NextVisibleTime}");

                    queue.DeleteMessage(message.ReceiptHandle);
                    Console.WriteLine($"DELETE: {message.ReceiptHandle}");

                    var batchMessages = queue.BatchReceiveMessage();
                    batchMessages.Messages.ForEach(m => Console.WriteLine($"Batch Receive: {m.MessageBody}"));
                    queue.BatchDeleteMessage(batchMessages.Messages.Select(m => m.ReceiptHandle).ToList());
                }
                catch (MessageNotExistException ex)
                {
                    Console.WriteLine("No messages any more.");
                    break;
                }
            }

            queue.DeleteQueue();

            Console.WriteLine("Press any key to continue..");
            Console.Read();
        }
    }
}