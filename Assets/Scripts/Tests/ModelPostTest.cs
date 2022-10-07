using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 

namespace Test
{
    public class ModelPostTest
    {
        string access_url = "http://radio.test/api/chat/";

        [System.Serializable]
        public class Chat
        {
            public int user_id;
            public string user_name;
            public int room_id;
            public string message;
            public string time_sent;
        }

        [System.Serializable]
        public class Chats
        {
            public Chat[] chats;
        }

        public IEnumerator ApiRequest()
        {
            var chat = new Chat();
            chat.user_id = 1;
            chat.user_name = "佐倉";
            chat.room_id = 1;
            chat.message = "通信できてるかなー？";
            chat.time_sent = System.DateTime.Now.ToString();
            var json = JsonUtility.ToJson(chat);


            byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
            var request = new UnityWebRequest(access_url, "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            Chats chats = JsonUtility.FromJson<Chats>(request.downloadHandler.text);
        }
    }
}
