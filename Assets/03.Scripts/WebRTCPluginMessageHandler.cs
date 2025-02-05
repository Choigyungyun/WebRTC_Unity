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

        public void OnPluginMessage(PluginType plugin)
        {
            switch (plugin)
            {
                case PluginType.EchoTest:
                    SetPlugin("janus.plugin.echotest", "");
                    break;
                case PluginType.Streaming:
                    SetPlugin("janus.plugin.audiobridge", "");
                    break;
                case PluginType.VideoCall:
                    SetPlugin("janus.plugin.videocall", "");
                    break;
                case PluginType.SIPGateway:
                    SetPlugin("janus.plugin.sip", "");
                    break;
                case PluginType.VideoRoom:
                    SetPlugin("janus.plugin.videoroom", "videoroomtest-");
                    break;
                case PluginType.VideoRoom_MultiStream:
                    SetPlugin("janus.plugin.mvideoroom", "");
                    break;
                case PluginType.AudioBridge:
                    SetPlugin("janus.plugin.audiobridge", "");
                    break;
                case PluginType.TextRoom:
                    SetPlugin("janus.plugin.textroom", "");
                    break;
                default:
                    break;
            }
        }

        private void SetPlugin(string plugin, string opaque)
        {
            this.plugin = plugin;
            pluginOpaque = opaque;
        }
    }
}
