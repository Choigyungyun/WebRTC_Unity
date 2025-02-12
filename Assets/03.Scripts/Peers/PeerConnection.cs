using MultiPartyWebRTC.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.WebRTC;
using UnityEngine;
using UnityEngine.UI;

namespace MultiPartyWebRTC.Peer
{
    public abstract class PeerConnection : MonoBehaviour
    {
        [SerializeField] protected RawImage videoDisplay;
        [SerializeField] protected AudioSource audioChannel;
        [SerializeField] protected TMP_Text nicknameText;

        protected RTCConfiguration configuration = new RTCConfiguration
        {
            iceServers = new[] { new RTCIceServer { urls = new[] { "stun:stun.l.google.com:19302" } } }
        };
        protected RTCPeerConnection peerConnection;
        protected DelegateOnIceConnectionChange OnIceConnectionChangeDelegate;
        protected DelegateOnIceCandidate OnIceCandidateDelegate;

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected void InitializePeerConnection()
        {
            peerConnection = new RTCPeerConnection(ref configuration);

            OnIceConnectionChangeDelegate = OnIceConnectionChange;
            OnIceCandidateDelegate = OnIceCandidate;
        }

        protected abstract void SetUp();
        protected abstract void Call();

        protected virtual void OnIceConnectionChange(RTCIceConnectionState state)
        {
            switch (state)
            {
                case RTCIceConnectionState.New:
                    Debug.Log($"{nicknameText.text} IceConnectionState: New");
                    break;
                case RTCIceConnectionState.Checking:
                    Debug.Log($"{nicknameText.text} IceConnectionState: Checking");
                    break;
                case RTCIceConnectionState.Closed:
                    Debug.Log($"{nicknameText.text} IceConnectionState: Closed");
                    break;
                case RTCIceConnectionState.Completed:
                    Debug.Log($"{nicknameText.text} IceConnectionState: Completed");
                    break;
                case RTCIceConnectionState.Connected:
                    Debug.Log($"{nicknameText.text} IceConnectionState: Connected");
                    break;
                case RTCIceConnectionState.Disconnected:
                    Debug.Log($"{nicknameText.text} IceConnectionState: Disconnected");
                    break;
                case RTCIceConnectionState.Failed:
                    Debug.Log($"{nicknameText.text} IceConnectionState: Failed");
                    break;
                case RTCIceConnectionState.Max:
                    Debug.Log($"{nicknameText.text} IceConnectionState: Max");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        protected virtual void OnIceCandidate(RTCIceCandidate candidate)
        {
            peerConnection.AddIceCandidate(candidate);

            DataEvent.OccurringCandidateEvent?.Invoke(candidate.Candidate);

            Debug.Log($"{gameObject.name} ICE Candidate Received: {candidate.Candidate}");
        }

        protected void OnSetLocalSuccess(RTCPeerConnection peer)
        {
            Debug.Log($"{peer} : {nicknameText.text} SetLocalDescription complete");
        }

        protected void OnSetRemoteSuccess(RTCPeerConnection peer)
        {
            Debug.Log($"{peer} : {nicknameText.text} SetRemoteDescription complete");
        }

        protected void OnSetSessionDescriptionError(ref RTCError error)
        {
            Debug.LogError($"Error Detail Type: {error.message}");
        }

        protected void OnCreateSessionDescriptionError(RTCError error)
        {
            Debug.LogError($"Error Detail Type: {error.message}");
        }
    }
}
