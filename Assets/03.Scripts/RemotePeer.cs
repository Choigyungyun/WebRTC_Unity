using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

namespace MultiPartyWebRTC.Peer
{
    public class RemotePeer : PeerConnection
    {

        private MediaStream mediaStream;
        private MediaStream aduioStream;

        private DelegateOnTrack onTrack;

        protected override void OnEnable()
        {
            base.OnEnable();


        }

        protected override void SetUp()
        {

        }

        protected override void Call()
        {
            throw new System.NotImplementedException();
        }

        private IEnumerator SetRemoteDescription(RTCPeerConnection peer, RTCSessionDescription desk)
        {
            RTCSetSessionDescriptionAsyncOperation operation = peer.SetRemoteDescription(ref desk);
            yield return operation;

            if (!operation.IsError)
            {
                OnSetRemoteSuccess(peer);
                yield return OnCreateAnswer(peer, desk);
            }
            else
            {
                RTCError error = operation.Error;
                OnSetSessionDescriptionError(ref error);
            }
        }

        private IEnumerator OnCreateAnswer(RTCPeerConnection peer, RTCSessionDescription desk)
        {
            RTCSessionDescriptionAsyncOperation operation = peer.CreateAnswer();
            yield return operation;

            if (!operation.IsError)
            {
                yield return OnCreateAnswerSuccess(peer, desk);
            }
            else
            {
                OnCreateSessionDescriptionError(operation.Error);
            }
        }

        private IEnumerator OnCreateAnswerSuccess(RTCPeerConnection peer, RTCSessionDescription desc)
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
    }
}
