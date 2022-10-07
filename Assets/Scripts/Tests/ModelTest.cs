using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 

namespace Test
{
    public class ModelTest
    {
        string access_url = "http://radio.test/api/chat/?user_id=1&room_id=1&time_sent=2010-10-07%2010:10:00";

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
            UnityWebRequest request = UnityWebRequest.Get(access_url);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            // Debug.Log(request.downloadHandler.text);

            Chats chats = JsonUtility.FromJson<Chats>("{\"chats\":" + request.downloadHandler.text + "}");
            Debug.Log(JsonUtility.ToJson(chats));
            foreach (var chat in chats.chats) {
                Debug.Log(chat.user_id);
                Debug.Log(chat.user_name);
                Debug.Log(chat.room_id);
                Debug.Log(chat.message);
                Debug.Log(chat.time_sent);
                Debug.Log("=========================");
            }

            // id = characterClass.characters[0].id;
            // name = characterClass.characters[0].name;
            // hp = characterClass.characters[0].hp;   
        }
    }
}
