using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Handler;
using MultiPartyWebRTC.Internal;
using Unity.WebRTC;
using UnityEditor;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private bool setDefaultValue = false;

        private const string Default_ICEServerURL = "stun:stun.l.google.com:19302";

        private WebSocketHandler webSocketHandler = new();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if (setDefaultValue)
            {
                webSocketHandler.SetDefaultWebSocket();
                SetDefaultUserProfile();
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
        }

        private void OnDisable()
        {
            // Home Panel Events
            UIEvent.ConnectClickEvent -= webSocketHandler.ConnectWebSocket;

            // Video Room Panel Events
            UIEvent.HangUpVideoRoomEvent -= webSocketHandler.DisconnectWebSocket;

            webSocketHandler.DisconnectWebSocket();
        }

        private void SetDefaultUserProfile() => UserProfileSetting.Nickname = UserProfileSetting.Default_Nickname;

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
