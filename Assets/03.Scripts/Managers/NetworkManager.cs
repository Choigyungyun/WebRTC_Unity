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
            if(this != null)
            {
                return;
            }
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
            UIEvent.SettingClickEvent += UpdateNetworkSetting;

            // Setting Panel Events
            DataEvent.ApplySettingDataEvent += ApplyWebSocketNetworkSetting;

            // Video Room Panel Evets
            UIEvent.HangUpVideoRoomEvent += webSocketHandler.DisconnectWebSocket;
        }

        private void OnDisable()
        {
            // Home Panel Events
            UIEvent.ConnectClickEvent -= webSocketHandler.ConnectWebSocket;
            UIEvent.SettingClickEvent -= UpdateNetworkSetting;

            // Setting Panel Events
            DataEvent.ApplySettingDataEvent -= ApplyWebSocketNetworkSetting;

            // Video Room Panel Events
            UIEvent.HangUpVideoRoomEvent -= webSocketHandler.DisconnectWebSocket;

            webSocketHandler.DisconnectWebSocket();
        }

        private void SetDefaultUserProfile() => UserProfileSetting.Nickname = UserProfileSetting.Default_Nickname;

        private void UpdateNetworkSetting()
        {
            webSocketHandler.UpdateWebSocketSetting();
            DataEvent.UpdateUserProfileDataEvent?.Invoke(UserProfileSetting.Nickname);
        }

        private void ApplyWebSocketNetworkSetting(string url, string protocol, string nickname, Vector2Int streamSize, RTCRtpCodecCapability videoCodec)
        {
            WebRTCSetting.StreamSize = streamSize;
            WebRTCSetting.VideoCodec = videoCodec;
            WebSocketSetting.WebSocketURL = url;
            WebSocketSetting.WebSocketProtocol = protocol;
            UserProfileSetting.Nickname = nickname;

            if(videoCodec != null)
            {
                Debug.Log($"The settings have been applied.\n" +
                          $"URL : {WebSocketSetting.WebSocketURL}\n" +
                          $"Protocol : {WebSocketSetting.WebSocketProtocol}\n" +
                          $"Nickname : {UserProfileSetting.Nickname}\n" +
                          $"Stream size : {WebRTCSetting.StreamSize}\n" +
                          $"Video Codec : {WebRTCSetting.VideoCodec.mimeType} {WebRTCSetting.VideoCodec.sdpFmtpLine}");
            }
            else
            {
                Debug.Log($"The settings have been applied.\n" +
                          $"URL : {WebSocketSetting.WebSocketURL}\n" +
                          $"Protocol : {WebSocketSetting.WebSocketProtocol}\n" +
                          $"Nickname : {UserProfileSetting.Nickname}\n" +
                          $"Stream size : {WebRTCSetting.StreamSize}\n" +
                          $"Video Codec : Default");
            }


            if (webSocketHandler.IsNullWebSocket())
            {
                return;
            }

            webSocketHandler.ClearAllWebSocket();
        }
    }
}
