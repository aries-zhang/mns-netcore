using Aliyun.MNS.Model.Topic;
using Aliyun.MNS.Topic.Apis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.MNS.Interface
{
    public interface ITopic
    {
        string CreateTopic(TopicAttributeParameter parameter);
        void DeleteTopic();
        TopicListModel ListTopics(string prefix, int pageSize, string nextMarker);
        TopicAttributeModel GetAttributes();
        void SetTopicAttributes(TopicAttributeParameter parameter);

        //    Subscribe,
        //    Unsubscribe,
        //    ListSubscriptionByTopic,
        //    GetSubscriptionAttributes,
        //    SetSubscriptionAttributes

        //    PublishMessage

        //    HttpEndpoint
    }
}
