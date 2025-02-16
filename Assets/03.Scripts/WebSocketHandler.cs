using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using UnityEngine;
using WebSocketSharp;

namespace MultiPartyWebRTC.Handler
{
    public class WebSocketHandler
    {
        private WebSocket websocket;
        private ConcurrentQueue<JObject> messageQueue = new ();

        public void ConnectWebSocket()
        {
            InitWebSocket(WebSocketSetting.WebSocketURL, WebSocketSetting.WebSocketProtocol);
            AttachWebSocketHandlers();
        }

        public bool TryreceiveQueue(ref JObject data)
        {
            return messageQueue.TryDequeue(out data);
        }

        public void SendMessage(string nickname, object message)
        {
            Debug.Log($"{nickname} : Send Message...\n" +
                      $"{message}");
            string data = JsonConvert.SerializeObject(message);
            websocket.Send(data);
        }

        public bool IsNullWebSocket()
        {
            return websocket == null;
        }

        public void DisconnectWebSocket()
        {
            if (websocket == null)
            {
                return;
            }
            ClearAllWebSocket();
        }

        public void ClearAllWebSocket()
        {
            if (websocket == null)
            {
                return;
            }

            websocket.Close();

            DetachWebSocketHandlers();

            websocket = null;
        }

        private void InitWebSocket(string url, string protocol)
        {
            if(websocket != null)
            {
                Debug.Log("This is a WebSocket that has already been created.");
                return;
            }

            websocket = new WebSocket(url, protocol);
            websocket.Connect();
            Debug.Log("WebSocket Message : WebSocket Connect - " + websocket.IsAlive);
        }


        #region WebScoket ¿Ã∫•∆Æ
        private void AttachWebSocketHandlers()
        {
            websocket.OnOpen += WebSocketOnOpen;
            websocket.OnMessage += WebSocketOnMessage;
            websocket.OnError += WebSocketOnError;
            websocket.OnClose += WebSocketOnClose;

            Debug.Log($"Attach WebSocket events.");
        }

        private void DetachWebSocketHandlers()
        {
            websocket.OnOpen -= WebSocketOnOpen;
            websocket.OnMessage -= WebSocketOnMessage;
            websocket.OnError -= WebSocketOnError;
            websocket.OnClose -= WebSocketOnClose;

            Debug.Log($"Detach WebSocket events.");
        }

        private void WebSocketOnOpen(object sender, EventArgs e)
        {
            Debug.Log("Socket Opend");
        }

        private void WebSocketOnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log("WebSocket Message: \n" + e.Data);

            messageQueue.Enqueue(JObject.Parse(e.Data));
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
