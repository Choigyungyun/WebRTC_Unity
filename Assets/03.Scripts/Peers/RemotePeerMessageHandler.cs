using MultiPartyWebRTC.Event;
using MultiPartyWebRTC;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;
using MultiPartyWebRTC.Handler;
using MultiPartyWebRTC.Peer;

public class RemotePeerMessageHandler
{
    public RemotePeer remotePeer;

    private string transaction;

    private MessageType messageType = MessageType.None;

    private Dictionary<string, object> peerParameters = new();
    private List<RTCIceCandidate> candidates = new();
    private MessageProcessor messageProcessor = new();
    private MessageClassifier messageClassifier = new();

    public RemotePeerMessageHandler()
    {
        SetRemotePeerDatas();

        OnMessageResponse(MessageType.Attach);
    }

    public void AddEvents()
    {
        DataEvent.OnMessageReceiveEvent += OnMessageReceiveClassifier;
    }

    public void RemoveEvents()
    {
        DataEvent.OnMessageReceiveEvent -= OnMessageReceiveClassifier;
    }

    public void GetFeedID(string feedID)
    {
        peerParameters["feed_id"] = feedID;
    }

    public void GetStreams(string streams)
    {
        peerParameters["streams"] = streams;
    }

    public void SetRemotePeerSDP(string sdp)
    {
        peerParameters["sdp"] = sdp;
        OnMessageResponse(MessageType.Start);
    }

    public void AddCandidates(RTCIceCandidate candidate)
    {
        candidates.Add(candidate);
    }

    private void SetRemotePeerDatas()
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
            case MessageType.Join_Subscriber:
                data = ProcessorMessage(type);
                break;
            case MessageType.Start:
                data = ProcessorMessage(type);
                break;
            default:
                break;
        }

        if(data == null)
        {
            return;
        }

        DataEvent.OnMessageResponseEvent?.Invoke("Remote Peer", data);

        messageType = type;

        if(messageType == MessageType.Start)
        {
            DataEvent.RemotePeerCompletedEvent?.Invoke();
        }
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

            DataEvent.OnMessageResponseEvent?.Invoke("Remote Peer", data);

        }

        SendCandidateCompletedResponse();
    }

    private void SendCandidateCompletedResponse()
    {
        object data = null;

        data = ProcessorMessage(MessageType.Completed);

        DataEvent.OnMessageResponseEvent?.Invoke("Remote Peer", data);
    }

    private void OnMessageReceiveClassifier(JObject data)
    {
        if (data["transaction"] == null || transaction != data["transaction"].ToString())
        {
            return;
        }

        IMessageClassifier classifier = messageClassifier.GetClassifier(messageType);
        (string key, object value) = classifier.ClassifierMessage(data);

        if (key == null && value == null)
        {
            return;
        }

        peerParameters[key] = value;

        switch (messageType)
        {
            case MessageType.None:
                break;
            case MessageType.Attach:
                OnMessageResponse(MessageType.Join_Subscriber);
                break;
            case MessageType.Join_Subscriber:
                remotePeer.HandlerOfferSDP(peerParameters["sdp"].ToString());
                break;
            case MessageType.Start:
                SendCandidateCompletedResponse();
                break;
            default:
                break;
        }
    }

    private object ProcessorMessage(MessageType type)
    {
        IMessageProcessor processor = messageProcessor.GetProcessor(type);
        (object, string) processorParameter = processor.ProcessMessage(peerParameters);
        if (processorParameter.Item2 != null)
        {
            transaction = processorParameter.Item2;
        }

        return processorParameter.Item1;
    }
}
