using MultiPartyWebRTC.Handler;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Event
{
    public class DataEvent
    {
        public static Action<MessageType> LocalMessagePropertyEvent;
        public static Action<MessageType> RemoteMessagePropertyEvent;

        public static Action<object> onMessageResponse;
    }
}
