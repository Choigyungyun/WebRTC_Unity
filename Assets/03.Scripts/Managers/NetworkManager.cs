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

        private bool updateSession = false;
        private float time = 0.0f;

        private WebSocketHandler webSocketHandler = new();
        private MessageHandler messageHandler = new();
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
            if(updateSession == false)
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
            UIEvent.ConnectClickEvent += StartConnect;
            UIEvent.BackConnectPanelEvent += StopConnect;

            // Setting Panel Events
            UIEvent.ApplySettingClickEvent += ApplyWebSocketNetworkSetting;

            // Connect Panel Events
            UIEvent.VideoRoomClickEvent += StartUpdateWebRTC;
            UIEvent.VideoRoomClickEvent += SetVideoRoomPlugin;

            // Video Room Panel Evets
            UIEvent.HangUpVideoRoomEvent += StopUpdateWebRTC;

            DataEvent.OnMessageResponseEvent += webSocketHandler.SendMessage;
        }
        private void RemoveEvents()
        {
            // Home Panel Events
            UIEvent.ConnectClickEvent -= StartConnect;
            UIEvent.BackConnectPanelEvent -= StopConnect;

            //Setting Panel Events
            UIEvent.ApplySettingClickEvent -= ApplyWebSocketNetworkSetting;

            // Connect Panel Events
            UIEvent.VideoRoomClickEvent -= StartUpdateWebRTC;
            UIEvent.VideoRoomClickEvent -= SetVideoRoomPlugin;

            // Video Room Panel Events
            UIEvent.HangUpVideoRoomEvent -= StopUpdateWebRTC;

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

        private void StartConnect()
        {
            webSocketHandler.ConnectWebSocket();

            messageHandler.AddMessageEvent();
            messageHandler.CreateSession();

            updateSession = true;
        }

        private void StopConnect()
        {
            updateSession = false;

            messageHandler.RemoveMessageEvet();
            webSocketHandler.DisconnectWebSocket();
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
