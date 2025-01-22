using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC.Event
{
    public class UIEvent
    {
        // Home Panel Events
        public static Action SettingClickEvent;
        public static Action ConnectClickEvent;
        public static Action QuitClickEvent;

        // Setting Panel Event
        public static Action BackSettingPanelClickEvent;
        public static Action ApplySettingClickEvent;

        // Connect Panel Events
        public static Action BackConnectPanelEvent;
        public static Action EchoTestClickEvent;
        public static Action StreamingClickEvent;
        public static Action VideoCallClickEvent;
        public static Action SIPGatewayClickEvent;
        public static Action VideoRoomClickEvent;
        public static Action VideoRoomMultiStreamClickEvent;
        public static Action AudioBridgeClickEvent;
        public static Action TextRoomClickEvent;

        // Video Room Panel Event
        public static Action HangUpVideoRoomEvent;
        public static Action<bool> VideoRoomStreamToggleEvent;
        public static Action<bool> VideoRoomMicrophoneToggleEvent;
    }
}
