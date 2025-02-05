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
        LocalMessage,
        RemoteMessage,
        Trickle,
        KeepAlive
    }

    public class MessageHandler
    {
        public string SessionID { get { return sessionID; } set { sessionID = value; } }
        private string sessionID = string.Empty;

        private WebRTCPluginMessageHandler pluginMessageHandler = new();

        public void SetPlugin(PluginType plugin)
        {
            pluginMessageHandler.OnPluginMessage(plugin);
        }
    }
}