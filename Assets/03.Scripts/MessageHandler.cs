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
        Message,
        Publisher,
        Subscriber,
        LocalMessage,
        RemoteMessage,
        Trickle,
        KeepAlive
    }

    public class MessageHandler
    {
        public event Action<object> OnMessageResponse;

        Dictionary<string, object> websocketParameters = new();

        private WebRTCPluginMessageHandler pluginMessageHandler = new();
        private MessageProcessor messageProcessor = new();

        public void HandleMessage(MessageType messageType)
        {
            IMessageProcessor processor = messageProcessor.GetProcessor(messageType);
            object message = processor.ProcessMessage(websocketParameters);

            OnMessageResponse?.Invoke(message);
        }

        public void SetPlugin(PluginType plugin)
        {
            (string, object) parameter = pluginMessageHandler.OnPluginMessage(plugin);

            websocketParameters[parameter.Item1] = parameter.Item2;
        }

        public void MessageReceive(JObject data)
        {

        }
    }
}