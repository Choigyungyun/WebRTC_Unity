using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Handler;
using MultiPartyWebRTC.Internal;
using Newtonsoft.Json.Linq;
using Unity.WebRTC;
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
        private int completedRemotePeers = 0;
        private float time = 0.0f;

        private WebSocketHandler websocketHandler = new();
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
            DequeueReceiveMessage();

            UpdateSessionTimer();
        }

        private void OnEnable()
        {
            AddEvents();
        }

        private void OnDisable()
        {
            RemoveEvents();

            StopUpdateWebRTC();

            websocketHandler.DisconnectWebSocket();

            completedRemotePeers = 0;
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
            UIEvent.VideoRoomClickEvent += StartVideoRoom;

            // Video Room Panel Evets
            UIEvent.HangUpVideoRoomEvent += StopConnect;

            DataEvent.OnMessageResponseEvent += websocketHandler.SendMessage;
            DataEvent.RemotePeerCompletedEvent += OnRemotePeerCompleted;
        }
        private void RemoveEvents()
        {
            // Home Panel Events
            UIEvent.ConnectClickEvent -= StartConnect;
            UIEvent.BackConnectPanelEvent -= StopConnect;

            //Setting Panel Events
            UIEvent.ApplySettingClickEvent -= ApplyWebSocketNetworkSetting;

            // Connect Panel Events
            UIEvent.VideoRoomClickEvent -= StartVideoRoom;

            // Video Room Panel Events
            UIEvent.HangUpVideoRoomEvent -= StopConnect;

            DataEvent.OnMessageResponseEvent -= websocketHandler.SendMessage;
            DataEvent.RemotePeerCompletedEvent -= OnRemotePeerCompleted;
        }
        #endregion

        private void ApplyWebSocketNetworkSetting()
        {
            if (websocketHandler.IsNullWebSocket())
            {
                return;
            }

            websocketHandler.ClearAllWebSocket();
        }

        private void StartConnect()
        {
            websocketHandler.ConnectWebSocket();

            messageHandler.AddMessageEvent();
            messageHandler.CreateSession();

            updateSession = true;
        }

        private void StopConnect()
        {
            updateSession = false;
            completedRemotePeers = 0;

            messageHandler.RemoveMessageEvet();
            websocketHandler.DisconnectWebSocket();

            StopUpdateWebRTC();
        }

        private void StartVideoRoom()
        {
            if (updateCoroutine != null)
            {
                return;
            }
            updateCoroutine = StartCoroutine(WebRTC.Update());
            Debug.Log("WebRTC update start!");

            messageHandler.SetPlugin(PluginType.VideoRoom);
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

        private void DequeueReceiveMessage()
        {
            JObject data = null;

            if (websocketHandler.IsNullWebSocket() == true || !websocketHandler.TryreceiveQueue(ref data))
            {
                return;
            }

            DataEvent.OnMessageReceiveEvent?.Invoke(data);
        }

        private void UpdateSessionTimer()
        {
            if (updateSession == false)
            {
                return;
            }

            time += Time.deltaTime;
            if (time >= maxSessionTime)
            {
                messageHandler.KeepAliveSession();
                time = 0.0f;
            }
        }

        private void OnRemotePeerCompleted()
        {
            completedRemotePeers++;

            if(completedRemotePeers != JanusDatas.TotalRemotePeers)
            {
                return;
            }

            JanusDatas.TotalRemotePeers = 0;
            completedRemotePeers = 1;
            DataEvent.OnRoomConfigureUpdateEvent?.Invoke(MessageType.Configure);
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
