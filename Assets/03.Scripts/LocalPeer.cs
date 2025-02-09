using MultiPartyWebRTC.Handler;
using MultiPartyWebRTC.Internal;
using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Peer
{
    public class LocalPeer : PeerConnection
    {
        [SerializeField] private AudioClip audioClip;

        private LocalPeerMessageHandler localPeerMessageHandler = new();

        private VideoStreamTrack videoStreamTrack;
        private AudioStreamTrack audioStreamTrack;
        private DelegateOnNegotiationNeeded onNegotiationNeeded;

        protected override void OnEnable()
        {
            base.OnEnable();

            SetUp();
        }

        protected override void SetUp()
        {
            nicknameText.text = UserProfileSetting.Nickname;
            CaptureAudioStart();
            StartCoroutine(CaptureVideoStart());
        }

        protected override void Call()
        {

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