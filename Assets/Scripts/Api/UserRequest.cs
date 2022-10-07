using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;
using Api.Model;

namespace Api
{
    public class UserRequest : SingletonMonoBehaviour<UserRequest>
    {
        private string base_url = $"http://{ApiUtility.MainIp}/api/user";

        public async UniTask<(UserModel.User, MessageModel.Message)> SignUp(string registeredTime)
        {
            // ボディに含めるパラメータ構築
            var user = new UserModel.User();
            user.registered_time = registeredTime;
            var json = JsonUtility.ToJson(user);

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
            var request = new UnityWebRequest(base_url, "POST");
            request.timeout = ApiUtility.TimeOut;
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest();

            // エラーハンドリング
            var isError = ApiUtility.IsError(request);
            if (isError != ApiUtility.Succeeded)
            {
                return (null, null);
            }

            var result = JsonUtility.FromJson<UserModel.User>(request.downloadHandler.text);
            var maintenace = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (result, maintenace);
        }

        public async UniTask<(SucceededModel, MessageModel.Message)> Change(int userId, string userName, string authenticationCode)
        {
            var changeUrl = base_url + "/update";
            // ボディに含めるパラメータ構築
            var user = new UserModel.User();
            user.id = userId;
            user.name = userName;
            user.authentication_code = authenticationCode;
            var json = JsonUtility.ToJson(user);

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
            var request = new UnityWebRequest(changeUrl, "POST");
            request.timeout = ApiUtility.TimeOut;
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest();

            var result = JsonUtility.FromJson<SucceededModel>(request.downloadHandler.text);
            var maintenace = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (result, maintenace);
        }
    }
}
