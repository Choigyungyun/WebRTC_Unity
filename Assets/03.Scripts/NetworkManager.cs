using MultiPartyWebRTC;
using MultiPartyWebRTC.Event;
using System.Collections;
using System.Collections.Generic;
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
            webSocketHandler.ConnectWebSocket();
        }

        private void OnEnable()
        {
            UIEvent.SettingClickEvent += webSocketHandler.UpdateWebSocketSetting;
        }

        private void OnDisable()
        {
            UIEvent.SettingClickEvent -= webSocketHandler.UpdateWebSocketSetting;
        }


    }
}
