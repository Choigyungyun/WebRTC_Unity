using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Internal
{
    internal class WebRTCSetting
    {
        public const bool Default_UseWebCam = false;
        public const bool Default_UseMicrophone = false;
        public const int Default_StreamWidth = 1920;
        public const int Default_StreamHeight = 1080;
        public const string Default_ICEServer_URL = "stun:stun.l.google.com:19302";

        public static bool UseWebCam { get { return useWebCam; } set { useWebCam = value; } }
        public static bool UseMicrophone { get { return useMicrophone; } set { useMicrophone = value; } }
        public static string ICEServerURL { get { return iceURL; } set { iceURL = value; } }
        public static Vector2Int StreamSize { get { return streamSize; } set { streamSize = value; } }
        public static RTCRtpCodecCapability VideoCodec { get { return videoCodec; } set { videoCodec = value; } }

        private static bool useWebCam = false;
        private static bool useMicrophone = false;
        private static string iceURL = string.Empty;
        private static Vector2Int streamSize = new Vector2Int(Default_StreamWidth, Default_StreamHeight);
        private static RTCRtpCodecCapability videoCodec = null;
    }

    internal class WebSocketSetting
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

    internal class UserProfileSetting
    {
        public const string Default_Nickname = "User";

        public static string Nickname { get { return nickname; } set { nickname = value; } }
        private static string nickname = string.Empty;
    }

    public class SettingDatas
    {
    }
}
