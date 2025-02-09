using MultiPartyWebRTC.Event;
using MultiPartyWebRTC.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MultiPartyWebRTC.Handler
{
    public class LocalPeerMessageHandler
    {
        public void LocalMessageInteraction()
        {
        }

        //public void OnLocalMessageProperty(MessageType message)
        //{
        //    object msg = null;

        //    switch (message)
        //    {
        //        case MessageType.None:
        //            break;
        //        case MessageType.Create:
        //            msg = new
        //            {
        //                janus = "create",
        //                transaction = RandomStringUtility.GenerateRandomString(10)
        //            };

        //            DataEvent.OnMessageResponse?.Invoke(msg);
        //            break;
        //        case MessageType.Attach:
        //            msg = new
        //            {
        //                janus = "attach",
        //                plugin = _plugins,
        //                opaque_id = _pluginOpaque + RandomStringUtility.GenerateRandomString(12),
        //                transaction = RandomStringUtility.GenerateRandomString(10),
        //                session_id = Int64.Parse(_sessionID)
        //            };

        //            onMessageResponse?.Invoke(msg);
        //            break;
        //        case MessageType.Message:
        //            if (messageType.Equals(MessageType.Message))
        //            {
        //                msg = new
        //                {
        //                    janus = "message",
        //                    body = new { request = "join", room = roomNumber, ptype = "publisher", display = userID },
        //                    transaction = RandomStringUtility.GenerateRandomString(10),
        //                    session_id = Int64.Parse(_sessionID),
        //                    handle_id = Int64.Parse(_localHandleID)
        //                };
        //            }

        //            onMessageResponse?.Invoke(msg);
        //            break;
        //        case MessageType.LocalMessage:
        //            msg = new
        //            {
        //                janus = "message",
        //                body = new { request = "configure", audio = true, video = true },
        //                jsep = new { sdp = localOfferSDP.Content, type = localOfferSDP.Type.ToString() },
        //                transaction = RandomStringUtility.GenerateRandomString(10),
        //                session_id = Int64.Parse(_sessionID),
        //                handle_id = Int64.Parse(_localHandleID)
        //            };
        //            Debug.Log(msg);
        //            onMessageResponse?.Invoke(msg);
        //            break;
        //        case MessageType.Trickle:
        //            ExcuteLocalIceCandidates(localInteractionType, _sessionID, _localHandleID);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}