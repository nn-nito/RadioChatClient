using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Api.Model;

namespace Api
{
    public class RadioStationRequest : SingletonMonoBehaviour<RadioStationRequest>
    {
        private string base_url = $"http://{ApiUtility.MainIp}/api/radio_station";

        public async UniTask<(RadioStationModel.RadioStations, MessageModel.Message)> GetRadioStations(string urlParameter = "")
        {
            var request = UnityWebRequest.Get(base_url + urlParameter);
            request.timeout = ApiUtility.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json");
            var result = await request.SendWebRequest();
            var radioStations = JsonUtility.FromJson<RadioStationModel.RadioStations>(request.downloadHandler.text);
            var maintenance = ApiUtility.IsMaintenance(request.downloadHandler.text);

            return (radioStations, maintenance);
        }
    }
}
