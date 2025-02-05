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
        /// 기본값 WebSocket URL 설정
        /// </summary>
        /// <remarks>
        /// Janus의 기본 WebSocket을 테스트 하시려면 <add>"wss://janus.conf.meetecho.com/ws"</add>를 추가하십시오.
        /// </remarks>
        public const string Default_URL = "wss://janus.conf.meetecho.com/ws";
        /// <summary>
        /// 기본값 WebSocket Protocol 설정
        /// </summary>
        /// <remarks>
        /// WebSocket Protocol을 변경하시려면 먼저 해당 서버의 WebSocket Protocol을 확인하십시오.
        /// Janus의 기본 WebSocket Protocol은 <c>"janus-protocol"</c>입니다.
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
