using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using WebSocketSharp;

namespace MultiPartyWebRTC.Handler
{
    public class WebSocketHandler
    {
        private WebSocket webSocket;

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
            ClearAllWebSocket();
        }

        public void SendMessage(object message)
        {
            Debug.Log(message);
            string data = JsonConvert.SerializeObject(message);
            webSocket.Send(data);
        }

        public bool IsNullWebSocket()
        {
            return webSocket == null;
        }

        public void ClearAllWebSocket()
        {
            if (webSocket == null)
            {
                return;
            }

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


        #region WebScoket ¿Ã∫•∆Æ
        private void AttachWebSocketHandlers()
        {
            webSocket.OnOpen += WebSocketOnOpen;
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

            DataEvent.OnMessageReceiveEvent?.Invoke(JObject.Parse(e.Data));
        }

        private void WebSocketOnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError("WebSocket Error: \n" + e.Message);
            Debug.LogError($"WebSocket Error stack : {e.Exception.StackTrace}");
        }

        private void WebSocketOnClose(object sender, CloseEventArgs e)
        {
            Debug.Log("WebSocket Close");
        }
        #endregion
    }
}
