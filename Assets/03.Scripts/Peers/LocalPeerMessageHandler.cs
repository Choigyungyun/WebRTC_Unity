using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Handler;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class LocalPeerMessageHandler
    {
        private string transaction;
        private string nickname;

        private MessageType messageType = MessageType.None;

        private Dictionary<string, object> peerParameters = new();
        private MessageProcessor messageProcessor = new();
        private MessageClassifier messageClassifier = new();

        public LocalPeerMessageHandler()
        {
            SetDatas();

            OnMessageResponse(MessageType.Attach);
        }

        public void AddEvents()
        {
            DataEvent.OnMessageReceiveEvent += OnMessageReceive;
        }

        public void RemoveEvents()
        {
            DataEvent.OnMessageReceiveEvent -= OnMessageReceive;
        }

        private void SetDatas()
        {
            peerParameters["session_id"] = JanusDatas.Session_ID;
            peerParameters["plugin"] = JanusDatas.PluginOption.Item1;
            peerParameters["opaque_id"] = JanusDatas.PluginOption.Item2;
        }

        private void OnMessageResponse(MessageType type)
        {
            object data = null;

            switch (type)
            {
                case MessageType.None:
                    break;
                case MessageType.Attach:
                    data = ProcessorMessage(type);
                    break;
                case MessageType.Join_Publisher:
                    data = ProcessorMessage(type);
                    break;
                case MessageType.Configure:
                    data = ProcessorMessage(type);
                    break;
                case MessageType.Trickle:
                    data = ProcessorMessage(type);
                    break;
                default:
                    break;
            }

            DataEvent.OnMessageResponseEvent?.Invoke(data);

            messageType = type;
        }

        private void OnMessageReceive(JObject data)
        {
            switch (messageType)
            {
                case MessageType.None:
                    break;
                case MessageType.Attach:
                    ClassifierMessage(data);
                    OnMessageResponse(MessageType.Join_Publisher);
                    break;
                case MessageType.Join_Publisher:
                    ClassifierMessage(data);
                    break;
                case MessageType.Configure:
                    break;
                case MessageType.Trickle:
                    break;
                default:
                    break;
            }
        }

        private object ProcessorMessage(MessageType type)
        {
            IMessageProcessor processor = messageProcessor.GetProcessor(type);
            (object, string) processorParameter = processor.ProcessMessage(peerParameters);
            transaction = processorParameter.Item2;

            return processorParameter.Item1;
        }

        private void ClassifierMessage(JObject data)
        {
            IMessageClassifier classifier = messageClassifier.GetClassifier(messageType);
            (string key, object value) = classifier.ClassifierMessage(data);

            if(key == null && value == null)
            {
                return;
            }

            peerParameters[key] = value;
        }
    }
}
