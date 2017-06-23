using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.MNS
{
    /// <summary>
    /// http://$AccountId.mns.<Region>.aliyuncs.com/queues/$QueueName
    /// </summary>
    public class Queue
    {
        public SendQueueMessageResponse SendMessage(QueueMessage message)
        {
            throw new NotImplementedException();
        }

        public QueueMessage ReceiveMessage()
        {
            throw new NotImplementedException();
        }

        public void DeleteMessage(string receiptHandle)
        {
            throw new NotImplementedException();
        }
    }
}
