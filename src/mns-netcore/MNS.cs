using Aliyun.MNS.Apis.Queue;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public class MNS
    {
        public MnsConfig Config { get; set; }
        
        private static Dictionary<string, IQueue> queueMap = new Dictionary<string, IQueue>();

        private MNS(MnsConfig config)
        {
            this.Config = config;
        }

        public static MNS Configure(string endpoint, string accessKeyId, string accessKeySecret)
        {
            return MNS.Configure(new MnsConfig()
            {
                Endpoint = endpoint,
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessKeySecret
            });
        }

        public static MNS Configure(MnsConfig config)
        {
            return new MNS(config);
        }

        public IQueue Queue(string name)
        {
            if (!queueMap.ContainsKey(name))
            {
                queueMap.Add(name, new Queue(this.Config, name));
            }

            return queueMap[name];
        }
    }
}