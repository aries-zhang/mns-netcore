using Aliyun.MNS.Apis.Queue;
using System.Threading.Tasks;

namespace Aliyun.MNS
{
    public class MNS
    {
        public MnsConfig Config { get; set; }

        private MNS(MnsConfig config)
        {
            this.Config = config;
        }

        public static MNS Configure(string endpoint, string accessKeyId, string accessKeySecret)
        {
            return new MNS(new MnsConfig()
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
            return new Queue(this.Config, name);
        }
    }
}