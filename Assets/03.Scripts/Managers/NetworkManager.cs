using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Handler;
using MultiPartyWebRTC.Internal;
using Unity.WebRTC;
using UnityEditor;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class NetworkManager : UniqueInstance<NetworkManager>
    {
        [SerializeField] private bool setDefaultValue = false;

        private NetworkManager instance;
        private WebSocketHandler webSocketHandler = new();
        private Coroutine updateCoroutine;

        private void Start()
        {
            if (setDefaultValue)
            {
                webSocketHandler.SetDefaultWebSocket();
                SetDefaultUserProfile();
                updateCoroutine = StartCoroutine(WebRTC.Update());
            }
            else
            {

            }
        }

        private void OnEnable()
        {
            // Home Panel Events
            UIEvent.ConnectClickEvent += webSocketHandler.ConnectWebSocket;

            // Setting Panel Events
            UIEvent.ApplySettingClickEvent += ApplyWebSocketNetworkSetting;

            // Video Room Panel Evets
            UIEvent.HangUpVideoRoomEvent += webSocketHandler.DisconnectWebSocket;
            UIEvent.HangUpVideoRoomEvent += StopUpdateWebRTC;
        }

        private void OnDisable()
        {
            // Home Panel Events
            UIEvent.ConnectClickEvent -= webSocketHandler.ConnectWebSocket;

            // Video Room Panel Events
            UIEvent.HangUpVideoRoomEvent -= webSocketHandler.DisconnectWebSocket;
            UIEvent.HangUpVideoRoomEvent -= StopUpdateWebRTC;

            webSocketHandler.DisconnectWebSocket();
            StopUpdateWebRTC();
        }

        private void StopUpdateWebRTC()
        {
            if(updateCoroutine == null)
            {
                return;
            }
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }

        private void SetDefaultUserProfile()
        {
            UserProfileSetting.Nickname = UserProfileSetting.Default_Nickname;
        }

        private void ApplyWebSocketNetworkSetting()
        {
            if (webSocketHandler.IsNullWebSocket())
            {
                return;
            }

            webSocketHandler.ClearAllWebSocket();
        }
    }
}
