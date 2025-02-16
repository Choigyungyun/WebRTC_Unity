using MultiPartyWebRTC.Event;
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
        KeepAlive
    }

    public enum ResponseType
    {
        None = 0,
        Ack,
        Event,
        TimeOut,
        HangUp
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
            if (messageType != MessageType.Create)
            {
                return;
            }

            ClassifierMessage(data);
            messageType = MessageType.None;
        }

        private void ClassifierMessage(JObject data)
        {
            IMessageClassifier classifier = messageClassifier.GetClassifier(messageType);
            (string key, object value) = classifier.ClassifierMessage(data);

            JanusDatas.Session_ID = value.ToString();
            messageParameter["session_id"] = value;
        }
    }
}