using MultiPartyWebRTC.Internal;
using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

public class LocalPeer : PeerConnection
{
    [SerializeField] private AudioClip audioClip;

    private VideoStreamTrack videoStreamTrack;
    private AudioStreamTrack audioStreamTrack;

    protected override void OnEnable()
    {
        base.OnEnable();

        Call();
    }

    protected override void SetUp()
    {

    }

    protected override void Call()
    {
        CaptureAudioStart();
        StartCoroutine(CaptureVideoStart());
    }

    private IEnumerator CaptureVideoStart()
    {
        yield return new WaitForEndOfFrame();

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
}
