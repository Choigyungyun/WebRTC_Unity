using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Utility;
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
        public event Action<object> OnMessageResponse;

        private MessageType messageType = MessageType.None;

        private Dictionary<string, object> websocketParameters = new();
        private Dictionary<string, List<string>> RemoteCandidate = new();
        private List<string> localCandidate = new();
        private WebRTCPluginMessageHandler pluginMessageHandler = new();
        private MessageProcessor messageProcessor = new();
        private MessageClassifier messageClassifier = new();

        public void AddEvents()
        {
            DataEvent.OccurringCandidateEvent += SetLocalCandidate;
        }

        public void RemoveEvents()
        {
            DataEvent.OccurringCandidateEvent -= SetLocalCandidate;
        }

        public void SetPlugin(PluginType plugin)
        {
            (string, object) parameter = pluginMessageHandler.OnPluginMessage(plugin);

            websocketParameters[parameter.Item1] = parameter.Item2;
        }

        public void HandleMessage(MessageType type)
        {
            messageType = type;

            IMessageProcessor processor = messageProcessor.GetProcessor(type);
            object message = processor.ProcessMessage(websocketParameters);

            OnMessageResponse?.Invoke(message);
        }

        public void MessageReceive(JObject data)
        {
            IMessageClassifier classifier = messageClassifier.GetClassifier(data);
            (string, object) parameter = classifier.ClassifierMessage(data);

            websocketParameters[parameter.Item1] = parameter.Item2;
        }

        private void SetLocalCandidate(string candidate)
        {
            localCandidate.Add(candidate);
        }
    }
}