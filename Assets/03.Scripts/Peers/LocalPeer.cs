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
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            localPeerMessage.RemoveEvents();
        }

        protected override void SetUp()
        {
            localPeerMessage = new LocalPeerMessageHandler();

            nicknameText.text = UserProfileSetting.Nickname;

            OnNegotiationNeeded = () => { StartCoroutine(PeerNegotiationNeeded(peerConnection)); };

            peerConnection.OnIceConnectionChange = OnIceConnectionChangeDelegate;
            peerConnection.OnIceCandidate = OnIceCandidateDelegate;
            peerConnection.OnNegotiationNeeded = OnNegotiationNeeded;

            CaptureAudioStart();
            StartCoroutine(CaptureVideoStart());

            Call();
        }

        protected override void Call()
        {
            AddTracks();
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

        private IEnumerator SetRemoteDescription(RTCPeerConnection peer, RTCSessionDescription desc)
        {
            RTCSetSessionDescriptionAsyncOperation operation = peer.SetRemoteDescription(ref desc);
            yield return operation;

            if (!operation.IsError)
            {
                OnSetRemoteSuccess(peer);
            }
            else
            {
                var error = operation.Error;
                OnSetSessionDescriptionError(ref error);
            }
        }
    }

}