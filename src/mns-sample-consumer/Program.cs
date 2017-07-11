using Aliyun.MNS.Utility;
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Aliyun.MNS.Common;
using System.Collections.Generic;

namespace Aliyun.MNS.Sample.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new BatchReceiveMessageModel()
            {
                Messages = new ReceiveMessageModel[] {
                    new ReceiveMessageModel(){ MessageId="1"},
                    new ReceiveMessageModel(){ MessageId="2" }
                }.ToList()
            };

            Console.WriteLine(XmlSerdeUtility.Serialize(test));
        }
    }
}