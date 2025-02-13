using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Handler;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public interface IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data);
    }

    public class MessageClassifier
    {
        public IMessageClassifier GetClassifier(MessageType type)
        {
            switch (type)
            {
                case MessageType.None:
                    return null;
                case MessageType.Create:
                    return new CreateMessageClassifier();
                case MessageType.Attach:
                    return new AttachMessageClassifier();
                case MessageType.Join_Publisher:
                    return new PublisherMessageClassifier();
                case MessageType.Join_Subscriber:
                    return new SubscriberMessageClassifier();
                case MessageType.Configure:
                    return new ConfigureMessageClassifier();
                case MessageType.Trickle:
                    return new TrickleMessageClassifier();
                default:
                    return null;
            }
        }
    }

    public class CreateMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            string janus = data["janus"].ToString();
            object session_id = string.Empty;

            if (janus == "success")
            {
                session_id = data["data"]["id"].ToString();
            }
            else
            {
                Debug.LogError("Session creation failed.");
            }

            return session_id != null ? ("session_id", session_id) : (null, null);
        }
    }

    public class AttachMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            string janus = data["janus"].ToString();
            object handle_id = string.Empty;

            if(janus == "success")
            {
                handle_id = data["data"]["id"].ToString();
            }
            else
            {

            }

            return handle_id != null ? ("handle_id",  handle_id) : (null, null);
        }
    }

    public class PublisherMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            string janus = data["janus"].ToString();

            if(janus == "event")
            {
                JArray jArray = (JArray)data["plugindata"]["data"]["publishers"];

                Debug.Log($"Room number : {data["plugindata"]["data"]["room"]}\n" +
                          $"Total Users : {jArray.Count}");

                if (jArray.Count > 0)
                {
                    List<JObject> publishers = jArray.ToObject<List<JObject>>();

                    foreach (JObject publisher in publishers)
                    {
                        Debug.Log($"Publisher data : {publisher}");
                    }
                }
            }

            return (null, null);
        }
    }

    public class SubscriberMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ConfigureMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TrickleMessageClassifier : IMessageClassifier
    {
        public (string, object) ClassifierMessage(JObject data)
        {
            throw new System.NotImplementedException();
        }
    }
}
