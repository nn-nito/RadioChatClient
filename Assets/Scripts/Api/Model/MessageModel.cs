using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Api.Model
{
    public class MessageModel
    {
        [Serializable]
        public class Message
        {
            public string title;
            public string message;
        }

        [Serializable]
        public class Messages
        {
            public Message[] messages;
        }
    }
}
