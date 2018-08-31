using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Aliyun.MNS.Tests
{
    public class TopicOperationTests
    {
        [Fact]
        public void CreateTopicTest()
        {
            // TODO: Set config parameters
            var topic = new Topic.Topic(new MnsConfig(), "test-topic");
            topic.CreateTopic();
        }
    }
}
