using Api.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Api
{
    public class ChatRequest : SingletonMonoBehaviour<ChatRequest>
    {
        private string baseUrl = $"http://{ApiUtility.MainIp}/api/chat";

        public async UniTask<(ChatModel.Chats, MessageModel.Message)> GetChat(string urlParameter)
        {
            var request = UnityWebRequest.Get(baseUrl + urlParameter);
            request.timeout = ApiUtility.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json");
            var result = await request.SendWebRequest();
            var chats = JsonUtility.FromJson<ChatModel.Chats>(request.downloadHandler.text);
            var maintenace = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (chats, maintenace);
        }

        public async UniTask<(ChatModel.Chats, MessageModel.Message)> GetAllChat(string urlParameter)
        {
            var chatUrl = baseUrl + "/all";
            var request = UnityWebRequest.Get(chatUrl + urlParameter);
            request.timeout = ApiUtility.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json");
            var result = await request.SendWebRequest();
            var chats = JsonUtility.FromJson<ChatModel.Chats>(request.downloadHandler.text);
            var maintenace = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (chats, maintenace);
        }

        public async UniTask<(ChatModel.Chat, MessageModel.Message)> SendChat(
            int user_id,
            string user_name,
            int room_id, string message,
            System.DateTime now
        )
        {
            // ボディに含めるパラメータ構築
            var chat = new ChatModel.Chat();
            chat.user_id = user_id;
            chat.user_name = user_name;
            chat.room_id = room_id;
            chat.message = message;
            chat.time_sent = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", now);
            var json = JsonUtility.ToJson(chat);

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
            var request = new UnityWebRequest(baseUrl, "POST");
            request.timeout = ApiUtility.TimeOut;
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest();

            var result = JsonUtility.FromJson<ChatModel.Chat>(request.downloadHandler.text);
            var maintenace = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (result, maintenace);
        }
    }
}
