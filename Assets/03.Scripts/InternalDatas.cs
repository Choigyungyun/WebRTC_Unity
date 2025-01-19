using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Internal
{
    internal static class WebRTCSetting
    {
        public static readonly int Default_StreamWidth = 1920;
        public static readonly int Default_StreamHeight = 1080;

        public static Vector2Int StreamSize { get { return streamSize; } set { streamSize = value; } }
        public static RTCRtpCodecCapability VideoCodec { get { return videoCodec; } set { videoCodec = value; } }

        private static Vector2Int streamSize = new Vector2Int(Default_StreamWidth, Default_StreamHeight);
        private static RTCRtpCodecCapability videoCodec = null;
    }

    internal static class WebSocketSetting
    {
        /// <summary>
        /// �⺻�� WebSocket URL ����
        /// </summary>
        /// <remarks>
        /// Janus�� �⺻ WebSocket�� �׽�Ʈ �Ͻ÷��� <add>"wss://janus.conf.meetecho.com/ws"</add>�� �߰��Ͻʽÿ�.
        /// </remarks>
        public const string Default_URL = "wss://janus.conf.meetecho.com/ws";
        /// <summary>
        /// �⺻�� WebSocket Protocol ����
        /// </summary>
        /// <remarks>
        /// WebSocket Protocol�� �����Ͻ÷��� ���� �ش� ������ WebSocket Protocol�� Ȯ���Ͻʽÿ�.
        /// Janus�� �⺻ WebSocket Protocol�� <c>"janus-protocol"</c>�Դϴ�.
        /// </remarks>
        public const string Default_Protocol = "janus-protocol";

        public static string WebSocketURL { get { return socketURL; } set { socketURL = value; } }
        public static string WebSocketProtocol { get { return socketProtocol; } set { socketProtocol = value; } }

        private static string socketURL = string.Empty;
        private static string socketProtocol = string.Empty;
    }

    internal static class UserProfileSetting
    {
        public const string Default_Nickname = "User";

        public static string Nickname { get { return nickname; } set { nickname = value; } }
        private static string nickname = string.Empty;
    }

    public class InternalDatas
    {
    }
}
