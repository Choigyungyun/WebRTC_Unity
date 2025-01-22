using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using WebSocketSharp;

namespace MultiPartyWebRTC.Handler
{
    public class WebSocketHandler
    {
        private ConcurrentQueue<string> messageDefendQueue = new();
        
        private WebSocket webSocket;
        
        public void SetDefaultWebSocket()
        {
            WebSocketSetting.WebSocketURL = WebSocketSetting.Default_URL;
            WebSocketSetting.WebSocketProtocol = WebSocketSetting.Default_Protocol;
        }

        public void ConnectWebSocket()
        {
            InitWebSocket(WebSocketSetting.WebSocketURL, WebSocketSetting.WebSocketProtocol);
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


        #region WebScoket �̺�Ʈ
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
