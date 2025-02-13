using MultiPartyWebRTC.Handler;
using MultiPartyWebRTC.Internal;
using MultiPartyWebRTC.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

namespace MultiPartyWebRTC
{
    public interface IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters);
    }

    public class MessageProcessor
    {
        public IMessageProcessor GetProcessor(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.None:
                    return null;
                case MessageType.Create:
                    return new CreateMessageProcessor();
                case MessageType.Attach:
                    return new AttachMessageProcessor();
                case MessageType.Join_Publisher:
                    return new PublishMessageProcessor();
                case MessageType.Join_Subscriber:
                    return new SubscriberMessageProcessor();
                case MessageType.Configure:
                    return new ConfigureMessageProcessor();
                case MessageType.Trickle:
                    return new TrickleMessageProcessor();
                case MessageType.KeepAlive:
                    return new KeepAliveMessageProcessor();
                default:
                    return null;
            }
        }
    }

    public class CreateMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            string randomString = RandomStringUtility.GenerateRandomString(10);
            object message = new
            {
                janus = "create",
                transaction = randomString
            };

            return (message, randomString);
        }
    }

    public class AttachMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            string randomString = RandomStringUtility.GenerateRandomString(10);
            object message = new
            {
                janus = "attach",
                plugin = parameters.ContainsKey("plugin") ? parameters["plugin"] : null,
                opaque_id = parameters.ContainsKey("opaque_id") ? parameters["opaque_id"] : null,
                transaction = randomString,
                session_id = parameters.ContainsKey("session_id") ? Int64.Parse(parameters["session_id"].ToString()) : (long?)null
            };
            return (message, randomString);
        }
    }

    public class PublishMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            string randomString = RandomStringUtility.GenerateRandomString(10);
            object message = new
            {
                janus = "message",
                body = new
                {
                    request = "join",
                    room = parameters.ContainsKey("room") ? parameters["room"] : 1234,
                    ptype = "publisher",
                    display = UserProfileSetting.Nickname
                },
                transaction = randomString,
                session_id = parameters.ContainsKey("session_id") ? Int64.Parse(parameters["session_id"].ToString()) : (long?)null,
                handle_id = parameters.ContainsKey("handle_id") ? Int64.Parse(parameters["handle_id"].ToString()) : (long?) null
            };
            return (message, randomString);
        }
    }

    public class SubscriberMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            string randomString = RandomStringUtility.GenerateRandomString(10);
            object message = new
            {
                janus = "message",
                body = new
                {
                    request = "join",
                    room = parameters.ContainsKey("room") ? parameters["room"] : null,
                    ptype = "subscriber",
                    streams = parameters.ContainsKey("streams") ? parameters["streams"] : null,
                    use_msid = false,
                    private_id = long.Parse(RandomIntUtility.GenerateRandomInt(10))
                },
                transaction = randomString,
                session_id = parameters.ContainsKey("session_id") ? Int64.Parse(parameters["session_id"].ToString()) : (long?)null,
                handle_id = parameters.ContainsKey("handle_id") ? Int64.Parse(parameters["handle_id"].ToString()) : (long?)null
            };

            return (message, randomString);
        }
    }

    public class StartMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            string randomString = RandomStringUtility.GenerateRandomString(10);
            object message = new
            {
                janus = "message",
                body = new
                {
                    request = "start",
                    room = parameters.ContainsKey("room") ? parameters["room"] : null
                },
                transaction = randomString,
                jsep = new
                {
                    sdp = parameters.ContainsKey("remote_anwser_sdp") ? parameters["remote_anwser_sdp"] : null,
                    type = "answer"
                },
                session_id = parameters.ContainsKey("session_id") ? Int64.Parse(parameters["session_id"].ToString()) : (long?)null,
                handle_id = parameters.ContainsKey("handle_id") ? Int64.Parse(parameters["handle_id"].ToString()) : (long?)null
            };
            return (message, randomString);
        }
    }

    public class ConfigureMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            string randomString = RandomStringUtility.GenerateRandomString(10);
            object message = new
            {
                janus = "message",
                body = new
                {
                    request = "configure",
                    audio = true,
                    video = true
                },
                jsep = new
                {
                    sdp = parameters.ContainsKey("local_offer_sdp") ? parameters["local_offer_sdp"] : null,
                    type = "offer"
                },
                transaction = randomString,
                session_id = parameters.ContainsKey("session_id") ? Int64.Parse(parameters["session_id"].ToString()) : (long?)null,
                handle_id = parameters.ContainsKey("handle_id") ? Int64.Parse(parameters["handle_id"].ToString()) : (long?)null
            };
            return (message, randomString);
        }
    }

    public class TrickleMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            object message = new
            {

            };
            return (message, null);
        }
    }

    public class KeepAliveMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            object message = new
            {
                janus = "keepalive",
                session_id = parameters.ContainsKey("session_id") ? Int64.Parse(parameters["session_id"].ToString()) : (long?)null,
                transaction = RandomStringUtility.GenerateRandomString(10)
            };

            return (message, null);
        }
    }
}
