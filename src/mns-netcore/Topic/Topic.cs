using Aliyun.MNS.Interface;
using Aliyun.MNS.Model.Topic;
using Aliyun.MNS.Topic.Apis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.MNS.Topic
{
    public class Topic : ITopic
    {
        public MnsConfig Config { get; set; }

        public string Name { get; set; }

        public Topic(MnsConfig config)
        {
            this.Config = config;
        }

        public Topic(MnsConfig config, string name)
        {
            this.Config = config;
            this.Name = name;
        }

        public string CreateTopic(TopicAttributeParameter parameter = null)
        {
            if (parameter == null)
            {
                parameter = new TopicAttributeParameter();
            }

            return new CreateTopicApiRequest(this.Config, this.Name, parameter).Call().Result;
        }

        public void DeleteTopic()
        {
            new DeleteTopicApiRequest(Config, Name).Call();
        }

        public TopicAttributeModel GetAttributes()
        {
            return new GetTopicAttributesApiRequest(Config, Name).Call().Result;
        }

        public TopicListModel ListTopics(string prefix, int pageSize, string nextMarker)
        {
            return new ListTopicApiRequest(Config, prefix, pageSize, nextMarker).Call().Result;
        }

        public void SetTopicAttributes(TopicAttributeParameter parameter)
        {
            new SetTopicAttributesApiRequest(Config, Name, parameter).Call();
        }
    }
}
