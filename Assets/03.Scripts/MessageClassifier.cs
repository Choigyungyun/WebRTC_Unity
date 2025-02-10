using MultiPartyWebRTC.Handler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPartyWebRTC
{
    public interface IMessageClassifier
    {

    }

    public class MessageClassifier
    {
        public IMessageClassifier GetClassifer(MessageType type)
        {
            return null;
        }
    }
}
