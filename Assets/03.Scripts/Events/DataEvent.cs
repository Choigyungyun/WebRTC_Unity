using System;
using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Event
{
    public class DataEvent
    {
        public static Action<string, string> UpdateWebSocketDataEvent;
        public static Action<string> UpdateUserProfileDataEvent;
        public static Action<Vector2Int, RTCRtpCodecCapability> UpdateVideoDataEvent;
        public static Action<string, string, string, Vector2Int, RTCRtpCodecCapability> ApplySettingDataEvent;
    }
}
