using MultiPartyWebRTC.Handler;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Event
{
    public class DataEvent
    {
        public static Action<string, object> OnMessageResponseEvent;
        public static Action<JObject> OnMessageReceiveEvent;

        public static Action<MessageType> OnRoomConfigureUpdateEvent;
        public static Action<int, List<JObject>> OnRoomPublishersUpdateEvent;
        public static Action<string> OnAwnserSDPReceiveEvent;
        public static Action<string> OnOfferSDPReceiveEvent;
        public static Action RemotePeerCompletedEvent;
        public static Action<string> LeavingRemotePeerEvent;
    }
}
