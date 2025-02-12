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
        [Header("Network Settings")]
        [SerializeField] private bool setDefaultValue = false;
        [Space(10)]
        [SerializeField] private float maxSessionTime = 0.0f;

        private bool updateWebRTC = false;
        private float time = 0.0f;

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

        private void Update()
        {
            if(updateWebRTC == false)
            {
                return;
            }

            time += Time.deltaTime;
            if(time >= maxSessionTime)
            {
                messageHandler.KeepAliveSession();
                time = 0.0f;
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

            DataEvent.OnMessageResponseEvent += webSocketHandler.SendMessage;
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

            DataEvent.OnMessageResponseEvent -= webSocketHandler.SendMessage;
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
            Debug.Log("WebRTC update start!");

            messageHandler = new MessageHandler();
            messageHandler.AddMessageEvent();

            updateWebRTC = true;
        }
        private void StopUpdateWebRTC()
        {
            if(updateCoroutine == null)
            {
                return;
            }
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
            Debug.Log("Stop WebRTC update.");

            messageHandler.RemoveMessageEvet();
            messageHandler = null;

            updateWebRTC = false;
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
