using MultiPartyWebRTC.Event;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using WebSocketSharp;

namespace MultiPartyWebRTC
{
    public class WebSocketHandler
    {
        /// <summary>
        /// 기본값 WebSocket URL 설정
        /// </summary>
        /// <remarks>
        /// Janus의 기본 WebSocket을 테스트 하시려면 <add>"wss://janus.conf.meetecho.com/ws"</add>를 추가하십시오.
        /// </remarks>
        private readonly string Default_URL = $"wss://janus.conf.meetecho.com/ws";
        /// <summary>
        /// 기본값 WebSocket Protocol 설정
        /// </summary>
        /// <remarks>
        /// WebSocket Protocol을 변경하시려면 먼저 해당 서버의 WebSocket Protocol을 확인하십시오.
        /// Janus의 기본 WebSocket Protocol은 <c>"janus-protocol"</c>입니다.
        /// </remarks>
        private readonly string Default_Protocol = "janus-protocol";
        private readonly string Default_Nickname = "User";

        public string WebSocketURL { get { return socketURL; } set { socketURL = value; } }
        public string WebSocketProtocol { get { return socketProtocol; } set { socketProtocol = value; } }
        public string Nickname { get { return nickname; } set { nickname = value; } }

        private string socketURL = string.Empty;
        private string socketProtocol = string.Empty;
        private string nickname = string.Empty;

        private ConcurrentQueue<string> messageDefendQueue = new();
        
        private WebSocket webSocket;
        
        public void SetDefaultWebSocket()
        {
            socketURL = Default_URL;
            socketProtocol = Default_Protocol;
            nickname = Default_Nickname;
        }

        public void UpdateWebSocketSetting() => DataEvent.SetDefaultWebSocketEvent?.Invoke(socketURL, socketProtocol, nickname);

        public void ConnectWebSocket()
        {
            InitWebSocket(socketURL, socketProtocol);
            AttachWebSocketHandlers();
        }

        public void DisconnectWebSocket()
        {
            if(webSocket == null)
            {
                return;
            }
            webSocket.Close();
            DetachWebSocketHandlers();
        }

        public bool IsNullWebSocket()
        {
            return webSocket == null;
        }

        public void ClearAllWebSocket()
        {
            webSocket.Close();

            DetachWebSocketHandlers();

            webSocket = null;
        }

        private void InitWebSocket(string url, string protocol)
        {
            if(webSocket != null)
            {
                Debug.Log("This is a WebSocket that has already been created.");
                return;
            }

            webSocket = new WebSocket(url, protocol);
            webSocket.Connect();
            Debug.Log("WebSocket Message : WebSocket Connect - " + webSocket.IsAlive);
        }


        #region WebScoket 이벤트
        private void AttachWebSocketHandlers()
        {
            webSocket.OnMessage += WebSocketOnOpen;
            webSocket.OnMessage += WebSocketOnMessage;
            webSocket.OnError += WebSocketOnError;
            webSocket.OnClose += WebSocketOnClose;

            Debug.Log($"Attach WebSocket events.");
        }

        private void DetachWebSocketHandlers()
        {
            webSocket.OnOpen -= WebSocketOnOpen;
            webSocket.OnMessage -= WebSocketOnMessage;
            webSocket.OnError -= WebSocketOnError;
            webSocket.OnClose -= WebSocketOnClose;

            Debug.Log($"Detach WebSocket events.");
        }

        private void WebSocketOnOpen(object sender, EventArgs e)
        {
            Debug.Log("Socket Opend");
        }

        private void WebSocketOnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log("WebSocket Message: \n" + e.Data);

            messageDefendQueue.Enqueue(e.Data);
        }

        private void WebSocketOnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError("WebSocket Error: \n" + e.Message);
        }

        private void WebSocketOnClose(object sender, CloseEventArgs e)
        {
            Debug.Log("WebSocket Close");
        }
        #endregion
    }
}
