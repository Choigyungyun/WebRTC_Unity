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

        private WebSocketHandler webSocketHandler = new();
        private MessageHandler messageHandler = null;
        private Coroutine updateCoroutine;

        private void Start()
        {
            if (setDefaultValue)
            {
                SetDefaultWebSocketSetting();
                SetDefaultWebRTCSetting();
                SetDefaultUserProfile();
            }
            else
            {

            }
        }

        private void OnEnable()
        {
            AddEvents();
        }

        private void OnDisable()
        {
            RemoveEvents();

            StopUpdateWebRTC();

            webSocketHandler.DisconnectWebSocket();
        }

        #region 이벤트
        private void AddEvents()
        {
            // Home Panel Events
            UIEvent.ConnectClickEvent += webSocketHandler.ConnectWebSocket;
            UIEvent.BackConnectPanelEvent += webSocketHandler.DisconnectWebSocket;

            // Setting Panel Events
            UIEvent.ApplySettingClickEvent += ApplyWebSocketNetworkSetting;

            // Connect Panel Events
            UIEvent.VideoRoomClickEvent += StartUpdateWebRTC;
            UIEvent.VideoRoomClickEvent += SetVideoRoomPlugin;

            // Video Room Panel Evets
            UIEvent.HangUpVideoRoomEvent += StopUpdateWebRTC;
            UIEvent.HangUpVideoRoomEvent += webSocketHandler.DisconnectWebSocket;
        }
        private void RemoveEvents()
        {
            // Home Panel Events
            UIEvent.ConnectClickEvent -= webSocketHandler.ConnectWebSocket;
            UIEvent.BackConnectPanelEvent -= webSocketHandler.DisconnectWebSocket;

            //Setting Panel Events
            UIEvent.ApplySettingClickEvent -= ApplyWebSocketNetworkSetting;

            // Connect Panel Events
            UIEvent.VideoRoomClickEvent -= StartUpdateWebRTC;
            UIEvent.VideoRoomClickEvent -= SetVideoRoomPlugin;

            // Video Room Panel Events
            UIEvent.HangUpVideoRoomEvent -= StopUpdateWebRTC;
            UIEvent.HangUpVideoRoomEvent -= webSocketHandler.DisconnectWebSocket;
        }

        private void AddMessageEvent()
        {

        }
        private void RemoveMessageEvet()
        {

        }
        #endregion

        private void ApplyWebSocketNetworkSetting()
        {
            if (webSocketHandler.IsNullWebSocket())
            {
                return;
            }

            webSocketHandler.ClearAllWebSocket();
        }

        // WebRTC 함수
        private void StartUpdateWebRTC()
        {
            if(updateCoroutine != null)
            {
                return;
            }
            updateCoroutine = StartCoroutine(WebRTC.Update());

            messageHandler = new MessageHandler();
            AddMessageEvent();

            Debug.Log("WebRTC update start!");
        }
        private void StopUpdateWebRTC()
        {
            if(updateCoroutine == null)
            {
                return;
            }
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;

            RemoveMessageEvet();
            messageHandler = null;

            Debug.Log("Stop WebRTC update.");
        }

        // Message Handler 함수
        private void SetVideoRoomPlugin()
        {
            messageHandler.SetPlugin(PluginType.VideoRoom);
        }

        #region 기본값 설정
        private void SetDefaultWebSocketSetting()
        {
            WebSocketSetting.WebSocketURL = WebSocketSetting.Default_URL;
            WebSocketSetting.WebSocketProtocol = WebSocketSetting.Default_Protocol;
        }
        private void SetDefaultWebRTCSetting()
        {
            WebRTCSetting.UseWebCam = WebRTCSetting.Default_UseWebCam;
            WebRTCSetting.UseMicrophone = WebRTCSetting.Default_UseMicrophone;
            WebRTCSetting.ICEServerURL = WebRTCSetting.Default_ICEServer_URL;
        }
        private void SetDefaultUserProfile()
        {
            UserProfileSetting.Nickname = UserProfileSetting.Default_Nickname;
        }
        #endregion
    }
}
