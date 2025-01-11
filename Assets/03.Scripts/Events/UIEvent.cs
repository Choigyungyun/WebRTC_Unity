using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC.Event
{
    public class UIEvent
    {
        // Home Panel Events
        public static Action VideoRoomClickEvent;
        public static Action SettingClickEvent;

        // Setting Panel Event
        public static Action BackSettingEvent;
        public static Action<string, string, string> ApplySettingEvent;

        // Video Room Panel Event
        public static Action HangUpVideoRoomEvent;
        public static Action VideoRoomStreamClickEvent;
        public static Action VideoRoomMicrophoneClickEvent;
    }
}
