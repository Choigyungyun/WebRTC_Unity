using MultiPartyWebRTC;
using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class NetworkManager : MonoBehaviour
    {
        private WebSocketHandler webSocketHandler = new();

        private void Awake()
        {
            webSocketHandler.SetDefaultWebSocket();
        }

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            // Home Panel Events
            UIEvent.VideoRoomClickEvent += webSocketHandler.ConnectWebSocket;
            UIEvent.SettingClickEvent += webSocketHandler.UpdateWebSocketSetting;

            // Setting Panel Events
            UIEvent.ApplySettingEvent += ApplyWebSocketNetworkSetting;

            // Video Room Panel Evets
            UIEvent.BackVideoRoomEvent += webSocketHandler.DisconnectWebSocket;
        }

        private void OnDisable()
        {
            // Home Panel Events
            UIEvent.VideoRoomClickEvent -= webSocketHandler.ConnectWebSocket;
            UIEvent.SettingClickEvent -= webSocketHandler.UpdateWebSocketSetting;

            // Setting Panel Events
            UIEvent.ApplySettingEvent -= ApplyWebSocketNetworkSetting;

            // Video Room Panel Events
            UIEvent.BackVideoRoomEvent -= webSocketHandler.DisconnectWebSocket;

            webSocketHandler.DisconnectWebSocket();
        }

        private void ApplyWebSocketNetworkSetting(string url, string protocol, string nickname)
        {
            webSocketHandler.WebSocketURL = url;
            webSocketHandler.WebSocketProtocol = protocol;
            webSocketHandler.Nickname = nickname;

            if(webSocketHandler.IsNullWebSocket())
            {
                return;
            }

            webSocketHandler.ClearAllWebSocket();
        }
    }
}
