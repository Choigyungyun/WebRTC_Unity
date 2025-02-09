using MultiPartyWebRTC.Handler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MultiPartyWebRTC
{
    public interface IMessageProcessor
    {
        public object ProcessMessage(Dictionary<string, object> parameters);
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
                case MessageType.Message:
                    return new PublishMessageProcessor();
                case MessageType.LocalMessage:
                    return new LocalMessageProcessor();
                case MessageType.Trickle:
                    return new TrickleMessageProcessor();
                default:
                    return null;
            }
        }
    }

    public class CreateMessageProcessor : IMessageProcessor
    {
        public object ProcessMessage(Dictionary<string, object> parameters)
        {
            return new
            {
            };
        }
    }

    public class AttachMessageProcessor : IMessageProcessor
    {
        public object ProcessMessage(Dictionary<string, object> parameters)
        {
            return new
            {
            };
        }
    }

    public class PublishMessageProcessor : IMessageProcessor
    {
        public object ProcessMessage(Dictionary<string, object> parameters)
        {
            return new
            {
            };
        }
    }

    public class LocalMessageProcessor : IMessageProcessor
    {
        public object ProcessMessage(Dictionary<string, object> parameters)
        {
            return new
            {
            };
        }
    }

    public class TrickleMessageProcessor : IMessageProcessor
    {
        public object ProcessMessage(Dictionary<string, object> parameters)
        {
            return new
            {
            };
        }
    }
}
