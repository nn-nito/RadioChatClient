using Api.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Api
{
    public class SearchRequest : SingletonMonoBehaviour<SearchRequest>
    {
        private string base_url = $"http://{ApiUtility.MainIp}/api/search";

        public async UniTask<(RadioWithUserFavoriteModel, MessageModel.Message)> SearchRadios(string urlParameter)
        {
            var request = UnityWebRequest.Get(base_url + urlParameter);
            request.timeout = ApiUtility.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json");
            var result = await request.SendWebRequest();
            var radios = JsonUtility.FromJson<RadioWithUserFavoriteModel>(request.downloadHandler.text);
            var maintenace = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (radios, maintenace);
        }
    }
}
