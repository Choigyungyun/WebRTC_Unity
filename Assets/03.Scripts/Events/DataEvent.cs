using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC.Event
{
    public class DataEvent
    {
        public static Action<string, string, string> SetDefaultWebSocketEvent;
        public static Action<string, string, string> SettingDataEvent;
    }
}
