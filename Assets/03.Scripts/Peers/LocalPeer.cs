using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Handler;
using MultiPartyWebRTC.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Peer
{
    public class LocalPeer : PeerConnection
    {
        [SerializeField] private AudioClip audioClip;

        private List<RTCRtpSender> peerSenders = new();
        private LocalPeerMessageHandler localPeerMessage;
        private VideoStreamTrack videoStreamTrack;
        private AudioStreamTrack audioStreamTrack;
        private DelegateOnNegotiationNeeded OnNegotiationNeeded;

        protected override void OnEnable()
        {
            base.OnEnable();

            SetUp();
            localPeerMessage.AddEvents();


            DataEvent.OnAwnserSDPReceiveEvent += HandleAnswerSDP;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            localPeerMessage.RemoveEvents();

            DataEvent.OnAwnserSDPReceiveEvent -= HandleAnswerSDP;
        }

        protected override void SetUp()
        {
            localPeerMessage = new LocalPeerMessageHandler();

            OnNegotiationNeeded = () => { StartCoroutine(PeerNegotiationNeeded(peerConnection)); };

            peerConnection.OnIceConnectionChange = OnIceConnectionChangeDelegate;
            peerConnection.OnIceCandidate = OnIceCandidateDelegate;
            peerConnection.OnNegotiationNeeded = OnNegotiationNeeded;

            nicknameText.text = UserProfileSetting.Nickname;

            CaptureAudioStart();
            StartCoroutine(CaptureVideoStart());

            AddTracks();
        }

        protected override void OnIceCandidate(RTCIceCandidate candidate)
        {
            localPeerMessage.AddCandidates(candidate);
        }

        private void AddTracks()
        {
            RTCRtpSender videoSender = peerConnection.AddTrack(videoStreamTrack);
            peerSenders.Add(videoSender);
            peerSenders.Add(peerConnection.AddTrack(audioStreamTrack));

            if (WebRTCSetting.VideoCodec != null)
            {
                RTCRtpCodecCapability[] codecs = new[] { WebRTCSetting.VideoCodec };
                RTCRtpTransceiver transceiver = peerConnection.GetTransceivers().First(t => t.Sender == videoSender);
                transceiver.SetCodecPreferences(codecs);
            }
        }

        private IEnumerator CaptureVideoStart()
        {
            if (!WebRTCSetting.UseWebCam)
            {
                videoStreamTrack = Camera.main.CaptureStreamTrack(WebRTCSetting.StreamSize.x, WebRTCSetting.StreamSize.y);
                videoDisplay.texture = Camera.main.targetTexture;

                yield break;
            }
        }

        private void CaptureAudioStart()
        {
            if (!WebRTCSetting.UseMicrophone)
            {
                audioChannel.clip = audioClip;
                audioChannel.loop = true;
                audioChannel.Play();
                audioStreamTrack = new AudioStreamTrack(audioChannel);
                return;
            }
        }

        private IEnumerator PeerNegotiationNeeded(RTCPeerConnection peer)
        {
            RTCSessionDescriptionAsyncOperation operation = peer.CreateOffer();
            yield return operation;

            if (!operation.IsError)
            {
                if (peerConnection.SignalingState != RTCSignalingState.Stable)
                {
                    Debug.LogError($"{peer} : {nicknameText.text} signaling state is not stable.");
                    yield break;
                }

                yield return StartCoroutine(OnCreateOfferSuccess(peer, operation.Desc));
            }
            else
            {
                OnCreateSessionDescriptionError(operation.Error);
            }
        }

        private IEnumerator OnCreateOfferSuccess(RTCPeerConnection peer, RTCSessionDescription desc)
        {
            RTCSetSessionDescriptionAsyncOperation operation = peer.SetLocalDescription(ref desc);
            localPeerMessage.SetLocalPeerSDP(desc.sdp);
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

        private void HandleAnswerSDP(string sdp)
        {
            StartCoroutine(SetRemoteDescription(sdp));
        }

        private IEnumerator SetRemoteDescription(string answerSDP)
        {
            RTCSessionDescription desc = new RTCSessionDescription
            {
                type = RTCSdpType.Answer,
                sdp = answerSDP,
            };
            RTCSetSessionDescriptionAsyncOperation operation = peerConnection.SetRemoteDescription(ref desc);
            yield return operation;

            if (!operation.IsError)
            {
                OnSetRemoteSuccess(peerConnection);
            }
            else
            {
                var error = operation.Error;
                OnSetSessionDescriptionError(ref error);
            }
        }
    }

}