using MultiPartyWebRTC.Handler;
using MultiPartyWebRTC.Internal;
using MultiPartyWebRTC.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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
                case MessageType.Start:
                    return new StartMessageProcessor();
                case MessageType.Configure:
                    return new ConfigureMessageProcessor();
                case MessageType.Completed:
                    return new CompletedMessageProcessor();
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
            JArray streamArray = JArray.Parse(parameters["streams"].ToString());
            List<object> streamList = new List<object>();
            foreach (JToken stream in streamArray)
            {
                JObject streamObject = stream as JObject;
                object streams = new
                {
                    feed = parameters.ContainsKey("feed_id") ? Int64.Parse(parameters["feed_id"].ToString()) : (long?)null,
                    mid = streamObject["mid"]?.ToString(),
                };
                streamList.Add(streams);
            }
            object message = new
            {
                janus = "message",
                body = new
                {
                    request = "join",
                    room = parameters.ContainsKey("room") ? parameters["room"] : 1234,
                    ptype = "subscriber",
                    streams = streamList,
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
                    room = parameters.ContainsKey("room") ? parameters["room"] : 1234
                },
                transaction = randomString,
                jsep = new
                {
                    sdp = parameters.ContainsKey("sdp") ? parameters["sdp"] : null,
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
                    sdp = parameters.ContainsKey("sdp") ? parameters["sdp"] : null,
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
                janus = "trickle",
                candidate = new
                {
                    candidate = parameters.ContainsKey("candidate") ? parameters["candidate"] : null,
                    sdpMLineIndex = parameters.ContainsKey("sdpMLineIndex") ? Int64.Parse(parameters["sdpMLineIndex"].ToString()) : (long?)null,
                    sdpMid = parameters.ContainsKey("sdpMid") ? parameters["sdpMid"] : null
                },
                transaction = RandomStringUtility.GenerateRandomString(10),
                session_id = parameters.ContainsKey("session_id") ? Int64.Parse(parameters["session_id"].ToString()) : (long?)null,
                handle_id = parameters.ContainsKey("handle_id") ? Int64.Parse(parameters["handle_id"].ToString()) : (long?)null
            };
            return (message, null);
        }
    }

    public class CompletedMessageProcessor : IMessageProcessor
    {
        public (object, string) ProcessMessage(Dictionary<string, object> parameters)
        {
            object message = new
            {
                janus = "trickle",
                candidate = new
                {
                    completed = true
                },
                transaction = RandomStringUtility.GenerateRandomString(10),
                session_id = parameters.ContainsKey("session_id") ? Int64.Parse(parameters["session_id"].ToString()) : (long?)null,
                handle_id = parameters.ContainsKey("handle_id") ? Int64.Parse(parameters["handle_id"].ToString()) : (long?)null
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
