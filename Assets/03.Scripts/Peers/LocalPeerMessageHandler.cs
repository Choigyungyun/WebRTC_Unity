using MultiPartyWebRTC.Event;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class LocalPeerMessageHandler
    {
        private string transaction;

        private Dictionary<string, object> peerParameters;
        private MessageProcessor messageProcessor = new();
        private MessageClassifier messageClassifier = new();

        public LocalPeerMessageHandler()
        {
            SetDatas();
        }

        public void AddEvents()
        {
            DataEvent.OnMessageReceiveEvent += MessageReceive;
        }

        public void RemoveEvents()
        {
            DataEvent.OnMessageReceiveEvent -= MessageReceive;
        }

        private void SetDatas()
        {
            peerParameters["session_id"] = JanusDatas.Session_ID;
            peerParameters[JanusDatas.PluginOption.Item1] = JanusDatas.PluginOption.Item2;
        }

        private void MessageReceive(JObject)
        {

        }
    }
}
