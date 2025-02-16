using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Handler;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public class LocalPeerMessageHandler
    {
        private string transaction;

        private MessageType messageType = MessageType.None;

        private Dictionary<string, object> peerParameters = new();
        private List<RTCIceCandidate> candidates = new();
        private MessageProcessor messageProcessor = new();
        private MessageClassifier messageClassifier = new();

        public LocalPeerMessageHandler()
        {
            SetLocalPeerDatas();

            OnMessageResponse(MessageType.Attach);
        }

        public void AddEvents()
        {
            DataEvent.OnMessageReceiveEvent += OnMessageReceive;
            DataEvent.OnRoomConfigureUpdateEvent += OnMessageResponse;
        }

        public void RemoveEvents()
        {
            DataEvent.OnMessageReceiveEvent -= OnMessageReceive;
            DataEvent.OnRoomConfigureUpdateEvent -= OnMessageResponse;
        }

        public void SetLocalPeerSDP(string sdp)
        {
            peerParameters["sdp"] = sdp;
        }

        public void AddCandidates(RTCIceCandidate candidate)
        {
            candidates.Add(candidate);
        }

        private void SetLocalPeerDatas()
        {
            peerParameters["session_id"] = JanusDatas.Session_ID;
            peerParameters["plugin"] = JanusDatas.PluginOption.Item1;
            peerParameters["opaque_id"] = JanusDatas.PluginOption.Item2;
        }

        private void OnMessageResponse(MessageType type)
        {
            object data = null;

            switch (type)
            {
                case MessageType.None:
                    break;
                case MessageType.Attach:
                    data = ProcessorMessage(type);
                    break;
                case MessageType.Join_Publisher:
                    data = ProcessorMessage(type);
                    break;
                case MessageType.Configure:
                    data = ProcessorMessage(type);
                    SendTrickleResponse();
                    break;
                default:
                    break;
            }

            DataEvent.OnMessageResponseEvent?.Invoke("Local Peer", data);

            messageType = type;
        }

        private void SendTrickleResponse()
        {
            object data = null;

            foreach (RTCIceCandidate candidate in candidates)
            {
                peerParameters["candidate"] = candidate.Candidate;
                peerParameters["sdpMLineIndex"] = candidate.SdpMLineIndex;
                peerParameters["sdpMid"] = candidate.SdpMid;

                data = ProcessorMessage(MessageType.Trickle);

                DataEvent.OnMessageResponseEvent?.Invoke("Local Peer", data);
            }

            SendCandidateCompletedResponse();
        }

        private void SendCandidateCompletedResponse()
        {
            object data = null;

            data = ProcessorMessage(MessageType.Completed);

            DataEvent.OnMessageResponseEvent?.Invoke("Local Peer", data);
        }

        private void OnMessageReceive(JObject data)
        {
            switch (messageType)
            {
                case MessageType.None:
                    break;
                case MessageType.Attach:
                    ClassifierMessage(data);
                    break;
                case MessageType.Join_Publisher:
                    ClassifierMessage(data);
                    break;
                case MessageType.Configure:
                    ClassifierMessage(data);
                    break;
                default:
                    break;
            }
        }

        private object ProcessorMessage(MessageType type)
        {
            IMessageProcessor processor = messageProcessor.GetProcessor(type);
            (object, string) processorParameter = processor.ProcessMessage(peerParameters);
            if(processorParameter.Item2 != null)
            {
                transaction = processorParameter.Item2;
            }

            return processorParameter.Item1;
        }

        private void ClassifierMessage(JObject data)
        {
            if(data["transaction"] == null || transaction != data["transaction"].ToString())
            {
                return;
            }

            Debug.Log($"LocalPeer : Classifier datas - Confirmed that the previous transaction values were the same\n" +
                      $"{data}\n");

            IMessageClassifier classifier = messageClassifier.GetClassifier(messageType);
            (string key, object value) = classifier.ClassifierMessage(data);

            if(key == null && value == null)
            {
                return;
            }

            peerParameters[key] = value;

            if (messageType == MessageType.Attach)
            {
                OnMessageResponse(MessageType.Join_Publisher);
            }
        }
    }
}
