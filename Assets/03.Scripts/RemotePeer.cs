using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

public class RemotePeer : PeerConnection
{
    private MediaStream mediaStream;
    private MediaStream aduioStream;

    protected override void SetUp()
    {
        throw new System.NotImplementedException();
    }

    protected override void Call()
    {
        throw new System.NotImplementedException();
    }
}
