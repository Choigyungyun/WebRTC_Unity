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
        /// �⺻�� WebSocket URL ����
        /// </summary>
        /// <remarks>
        /// Janus�� �⺻ WebSocket�� �׽�Ʈ �Ͻ÷��� <add>"wss://janus.conf.meetecho.com/ws"</add>�� �߰��Ͻʽÿ�.
        /// </remarks>
        private readonly string Default_URL = $"wss://janus.conf.meetecho.com/ws";
        /// <summary>
        /// �⺻�� WebSocket Protocol ����
        /// </summary>
        /// <remarks>
        /// WebSocket Protocol�� �����Ͻ÷��� ���� �ش� ������ WebSocket Protocol�� Ȯ���Ͻʽÿ�.
        /// Janus�� �⺻ WebSocket Protocol�� <c>"janus-protocol"</c>�Դϴ�.
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

            // WebSocket ������ ��
            webSocket.OnOpen += (sender, e) =>
            {
                Debug.Log("Socket Opend");
            };

            // WebSocket �޼��� �̺�Ʈ�� �߻��� ��
            webSocket.OnMessage += (sender, e) =>
            {
                Debug.Log("WebSocket Message: \n" + e.Data);

                messageDefendQueue.Enqueue(e.Data);
            };

            // WebSocket ������ �߻��� ��
            webSocket.OnError += (sender, e) =>
            {
                Debug.LogError("WebSocket Error: \n" + e.Message);
            };

            // WebSocket ������ ��
            webSocket.OnClose += (sender, e) =>
            {
                Debug.Log("WebSocket Close");
            };
        }
    }
}
