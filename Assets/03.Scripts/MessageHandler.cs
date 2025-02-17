using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Peer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC.Handler
{
    public enum PeerType
    {
        None = 0,
        LocalPeer,
        RemotePeer,
    }

    public enum MessageType
    {
        None = 0,
        Create,
        Update,
        Attach,
        Join_Publisher,
        Join_Subscriber,
        Start,
        Configure,
        Completed,
        Trickle,
        Wait_Others,
        KeepAlive
    }

    public class MessageHandler
    {
        private string transaction;

        private MessageType messageType = MessageType.None;

        private Dictionary<string, object> messageParameter = new();
        private WebRTCPluginMessageHandler pluginMessageHandler = new();
        private MessageProcessor messageProcessor = new();
        private MessageClassifier messageClassifier = new();

        public void AddMessageEvent()
        {
            DataEvent.OnMessageReceiveEvent += ReceiveMessage;
        }
        public void RemoveMessageEvet()
        {
            DataEvent.OnMessageReceiveEvent -= ReceiveMessage;
        }

        public void SetPlugin(PluginType type)
        {
            (string, string) parameter = pluginMessageHandler.OnPluginMessage(type);
            messageParameter["plugin"] = parameter.Item1;
            messageParameter["plugin_opaque"] = parameter.Item2;

            JanusDatas.PluginOption = parameter;

            Debug.Log($"Add plugin : {type}\n" +
                      $"Plugin : {parameter.Item1}\n" +
                      $"Plugin opaque : {parameter.Item2}");
        }

        public void CreateSession()
        {
            messageType = MessageType.Create;

            IMessageProcessor processor = messageProcessor.GetProcessor(messageType);
            (object, string) processorParameter = processor.ProcessMessage(null);
            transaction = processorParameter.Item2;

            DataEvent.OnMessageResponseEvent?.Invoke("Client", processorParameter.Item1);
        }

        public void KeepAliveSession()
        {
            IMessageProcessor processor = messageProcessor.GetProcessor(MessageType.KeepAlive);
            (object, string) processorParameter = processor.ProcessMessage(messageParameter);

            DataEvent.OnMessageResponseEvent?.Invoke("Client", processorParameter.Item1);
        }

        private void ReceiveMessage(JObject data)
        {
            ClassifierMessage(data);
        }

        private void ClassifierMessage(JObject data)
        {
            IMessageClassifier classifier = null;
            (string key, object value) = (string.Empty, string.Empty);

            switch (messageType)
            {
                case MessageType.None:
                    break;
                case MessageType.Create:
                    classifier = messageClassifier.GetClassifier(messageType);
                    (key, value) = classifier.ClassifierMessage(data);

                    JanusDatas.Session_ID = value.ToString();
                    messageParameter[key] = value;

                    messageType = MessageType.Wait_Others;
                    break;
                case MessageType.Wait_Others:
                    if (data["transaction"] == null)
                    {
                        classifier = messageClassifier.GetClassifier(messageType);
                        (key, value) = classifier.ClassifierMessage(data);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}