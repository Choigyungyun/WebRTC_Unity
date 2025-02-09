using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC.Handler
{
    public enum PluginType
    {
        None = 0,
        EchoTest,
        Streaming,
        VideoCall,
        SIPGateway,
        VideoRoom,
        VideoRoom_MultiStream,
        AudioBridge,
        TextRoom
    }

    public class WebRTCPluginMessageHandler
    {
        public string Plugin { get { return plugin; } set { plugin = value; } }
        public string PluginOpaque { get { return pluginOpaque; } set { pluginOpaque = value; } }

        private string plugin = string.Empty;
        private string pluginOpaque = string.Empty;

        public (string, object) OnPluginMessage(PluginType plugin)
        {
            switch (plugin)
            {
                case PluginType.EchoTest:
                    return ("janus.plugin.echotest", "");
                case PluginType.Streaming:
                    return ("janus.plugin.audiobridge", "");
                case PluginType.VideoCall:
                    return ("jreturn anus.plugin.videocall", "");
                case PluginType.SIPGateway:
                    return ("janus.plugin.sip", "");
                case PluginType.VideoRoom:
                    return ("janus.plugin.videoroom", "videoroomtest-");
                case PluginType.VideoRoom_MultiStream:
                    return ("janus.plugin.mvideoroom", "");
                case PluginType.AudioBridge:
                    return ("janus.plugin.audiobridge", "");
                case PluginType.TextRoom:
                    return ("janus.plugin.textroom", "");
                default:
                    return (null, null);
            }
        }
    }
}
