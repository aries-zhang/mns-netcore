using System;

namespace Aliyun.MNS.Sample.Producer
{
    class Program
    {
        private static string Host = Environment.GetEnvironmentVariable("ALIYUN_MNS_HOST");
        private static string AccessKey = Environment.GetEnvironmentVariable("ALIYUN_MNS_ACCESSKEY");
        private static string AccessSecret = Environment.GetEnvironmentVariable("ALIYUN_MNS_ACCESSSECRET");

        private static IQueue Queue = MNS.Configure(Host, AccessKey, AccessSecret).Queue("MetalMessageQueue");

        static void Main(string[] args)
        {
            var attributes = Queue.GetAttributes().Result;

            var result = Queue.SendMessage("test").Result;
            
            Console.WriteLine("Press any key to continue..");
            Console.Read();
        }
    }
}