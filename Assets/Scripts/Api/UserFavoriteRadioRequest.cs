using Api.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Api
{
    public class UserFavoriteRadioRequest : SingletonMonoBehaviour<UserFavoriteRadioRequest>
    {
        private string base_url = $"http://{ApiUtility.MainIp}/api/user_favorite_radio";

        public async UniTask<(RadioWithUserFavoriteModel, MessageModel.Message)> GetUserFavoriteRadios(string urlParameter = "")
        {
            var request = UnityWebRequest.Get(base_url + urlParameter);
            request.timeout = ApiUtility.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json");
            var result = await request.SendWebRequest();
            var radios = JsonUtility.FromJson<RadioWithUserFavoriteModel>(request.downloadHandler.text);
            var maintenance = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (radios, maintenance);
        }

        public async UniTask<(UserFavoriteRadioModel.UserFavoriteRadio, MessageModel.Message)> AddFavorite(int userId, int radioId)
        {
            // ボディに含めるパラメータ構築
            var favorite = new UserFavoriteRadioModel.UserFavoriteRadio();
            favorite.user_id = userId;
            favorite.radio_id = radioId;
            var json = JsonUtility.ToJson(favorite);

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
            var request = new UnityWebRequest(base_url, "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest();

            var result = JsonUtility.FromJson<UserFavoriteRadioModel.UserFavoriteRadio>(request.downloadHandler.text);
            var maintenance = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (result, maintenance);
        }

        public async UniTask<(bool, MessageModel.Message)> DeleteFavorite(int userId, int radioId)
        {
            // ボディに含めるパラメータ構築
            var favorite = new UserFavoriteRadioModel.UserFavoriteRadio();
            favorite.user_id = userId;
            favorite.radio_id = radioId;
            var json = JsonUtility.ToJson(favorite);

            byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
            var request = new UnityWebRequest(base_url + "/delete", "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest();

            var result = JsonUtility.FromJson<bool>(request.downloadHandler.text);
            var maintenance = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (result, maintenance);
        }
    }
}
