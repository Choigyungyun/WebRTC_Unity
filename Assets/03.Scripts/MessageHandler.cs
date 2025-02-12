using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Internal;
using MultiPartyWebRTC.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices.WindowsRuntime;
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

            JanusDatas.PluginOption = parameter;

            Debug.Log($"Add plugin : {type}\n" +
                      $"Plugin : {parameter.Item1}\n" +
                      $"Plugin opaque : {parameter.Item2}");
        }

        public void KeepAliveSession()
        {
            IMessageProcessor processor = messageProcessor.GetProcessor(MessageType.KeepAlive);
            (object, string) processorParameter = processor.ProcessMessage(null);

            DataEvent.OnMessageResponseEvent?.Invoke(processorParameter.Item1);
        }

        public MessageHandler()
        {
            CreateSession();
        }

        private void ReceiveMessage(JObject data)
        {
            SetJanusData(data);

            messageType = MessageType.None;
        }

        private void CreateSession()
        {
            messageType = MessageType.Create;

            IMessageProcessor processor = messageProcessor.GetProcessor(messageType);
            (object, string) processorParameter = processor.ProcessMessage(null);
            transaction = processorParameter.Item2;

            DataEvent.OnMessageResponseEvent?.Invoke(processorParameter.Item1);
            UIEvent.CreateSessionEvent?.Invoke();
        }

        private void SetJanusData(JObject data)
        {
            IMessageClassifier classifier = messageClassifier.GetClassifier(messageType);
            (string, object) parameter = classifier.ClassifierMessage(data);

            switch (messageType)
            {
                case MessageType.None:
                    break;
                case MessageType.Create:
                    JanusDatas.Session_ID = parameter.Item2.ToString();
                    break;
                default:
                    break;
            }
        }
    }
}