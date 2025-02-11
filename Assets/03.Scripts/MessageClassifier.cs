using MultiPartyWebRTC.Handler;
using Newtonsoft.Json.Linq;

namespace MultiPartyWebRTC
{
    public interface IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data);
    }

    public class MessageClassifier
    {
        public IMessageClassifier GetClassifier(JObject data)
        {
            return null;
        }
    }

    public class CreateMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AttachMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            throw new System.NotImplementedException();
        }
    }

    public class PublisherMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            throw new System.NotImplementedException();
        }
    }

}
