using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public string WebSocketURL { get { return socketURL; } set { socketURL = value; } }
        public string WebSocketProtocol { get { return socketProtocol; } set { socketProtocol = value; } }

        private string socketURL = string.Empty;
        private string socketProtocol = string.Empty;

        private ConcurrentQueue<string> messageDefendQueue = new();
        
        private WebSocket webSocket;

        public WebSocketHandler()
        {
            
        }

        private void InitWebSocket(string url, string protocol)
        {
            webSocket = new WebSocket(url, protocol);

            webSocket.Connect();
            Debug.Log("WebSocket Message : WebSocket Connect - " + webSocket.IsAlive);

            // WebSocket 열렸을 때
            webSocket.OnOpen += (sender, e) =>
            {
                Debug.Log("Socket Opend");
            };

            // WebSocket 메세지 이벤트가 발생할 때
            webSocket.OnMessage += (sender, e) =>
            {
                Debug.Log("WebSocket Message: \n" + e.Data);

                messageDefendQueue.Enqueue(e.Data);
            };

            // WebSocket 에러가 발생할 때
            webSocket.OnError += (sender, e) =>
            {
                Debug.LogError("WebSocket Error: \n" + e.Message);
            };

            // WebSocket 닫혔을 때
            webSocket.OnClose += (sender, e) =>
            {
                Debug.Log("WebSocket Close");
            };
        }
    }
}
