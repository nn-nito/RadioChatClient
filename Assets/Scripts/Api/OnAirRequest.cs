using Api.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Api
{
    public class OnAirRequest : SingletonMonoBehaviour<OnAirRequest>
    {
        private string base_url = $"http://{ApiUtility.MainIp}/api/on_air";

        public async UniTask<(RadioWithUserFavoriteModel, MessageModel.Message)> GetOnAirRadio(string urlParameter)
        {
            var request = UnityWebRequest.Get(base_url + urlParameter);
            request.timeout = ApiUtility.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json");
            var result = await request.SendWebRequest();
            var radios = JsonUtility.FromJson<RadioWithUserFavoriteModel>(request.downloadHandler.text);
            var maintenance = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (radios, maintenance);
        }
    }
}
