using System;
using System.Collections;
using System.Collections.Generic;

namespace Api
{
    public class ChatModel
    {
        [Serializable]
        public class Chat
        {
            public int user_id;
            public string user_name;
            public int room_id;
            public string message;
            public string time_sent;
        }

        [Serializable]
        public class Chats
        {
            public Chat[] chats;
        }
    }
}
