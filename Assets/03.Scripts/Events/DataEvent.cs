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
        public static Action<object> OnMessageResponseEvent;
        public static Action<JObject> OnMessageReceiveEvent;

        public static Action<string, object> OfferSDPEvent;
        public static Action<string> OccurringCandidateEvent;
    }
}
