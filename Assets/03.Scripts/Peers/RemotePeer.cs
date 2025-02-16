using MultiPartyWebRTC.Event;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Peer
{
    public class RemotePeer : PeerConnection
    {
        public string nickname { get { return nicknameText.text; } set { nicknameText.text = value; } }
        public JObject peerData;

        private MediaStream mediaStream;
        private MediaStream audioStream;

        private DelegateOnTrack onTrack;
        private RemotePeerMessageHandler remotePeerMessageHandler;

        protected override void OnEnable()
        {
            base.OnEnable();

            SetUp();
            remotePeerMessageHandler.AddEvents();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            remotePeerMessageHandler.RemoveEvents();
        }

        protected override void SetUp()
        {
            remotePeerMessageHandler = new RemotePeerMessageHandler();
            remotePeerMessageHandler.remotePeer = this;

            onTrack = e =>
            {
                Debug.Log($"[RemotePeer] OnTrack event received: {e.Track.Kind}");
                if (e.Track is VideoStreamTrack video)
                {
                    Debug.Log("[RemotePeer] Video Track received!");
                    video.OnVideoReceived += tex =>
                    {
                        Debug.Log("[RemotePeer] Video Frame Received!");
                        videoDisplay.texture = tex;
                    };
                }

                if(e.Track is AudioStreamTrack audioTrack)
                {
                    audioChannel.SetTrack(audioTrack);
                    audioChannel.loop = true;
                    audioChannel.Play();
                }
            };

            peerConnection.OnIceConnectionChange = OnIceConnectionChangeDelegate;
            peerConnection.OnIceCandidate = OnIceCandidateDelegate;
            peerConnection.OnTrack = onTrack;
        }

        protected override void OnIceCandidate(RTCIceCandidate candidate)
        {
            Debug.Log($"{name} : Remote ICE Candidate : {candidate.Candidate}");
            remotePeerMessageHandler.AddCandidates(candidate);
        }

        public void SetRemotePeer(JObject publisherData)
        {
            peerData = publisherData;
            nickname = publisherData["display"].ToString();
            name = nickname;
            GetPublisherData(publisherData);
        }

        public void GetPublisherData(JObject publisher)
        {
            remotePeerMessageHandler.GetFeedID(publisher["id"].ToString());
            remotePeerMessageHandler.GetStreams(publisher["streams"].ToString());
        }

        public void HandlerOfferSDP(string sdp)
        {
            StartCoroutine(SetRemoteDescription(sdp));
        }

        private IEnumerator SetRemoteDescription(string offerSDP)
        {
            RTCSessionDescription desc = new RTCSessionDescription
            {
                type = RTCSdpType.Offer,
                sdp = offerSDP
            };
            RTCSetSessionDescriptionAsyncOperation operation = peerConnection.SetRemoteDescription(ref desc);
            yield return operation;

            if (!operation.IsError)
            {
                OnSetRemoteSuccess(peerConnection);
                yield return OnCreateAnswer(peerConnection, desc);
            }
            else
            {
                RTCError error = operation.Error;
                OnSetSessionDescriptionError(ref error);
            }
        }

        private IEnumerator OnCreateAnswer(RTCPeerConnection peer, RTCSessionDescription desc)
        {
            RTCSessionDescriptionAsyncOperation operation = peer.CreateAnswer();
            yield return operation;

            if (!operation.IsError)
            {
                yield return OnCreateAnswerSuccess(peer, operation.Desc);
            }
            else
            {
                OnCreateSessionDescriptionError(operation.Error);
            }
        }

        private IEnumerator OnCreateAnswerSuccess(RTCPeerConnection peer, RTCSessionDescription desc)
        {
            RTCSetSessionDescriptionAsyncOperation operation = peer.SetLocalDescription(ref desc);
            remotePeerMessageHandler.SetRemotePeerSDP(desc.sdp);
            yield return operation;

            if (!operation.IsError)
            {
                OnSetLocalSuccess(peer);
            }
            else
            {
                RTCError error = operation.Error;
                OnSetSessionDescriptionError(ref error);
            }
        }
    }
}
