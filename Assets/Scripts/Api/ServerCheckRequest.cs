using Api.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Api
{
    public class ServerCheckRequest : SingletonMonoBehaviour<ServerCheckRequest>
    {
        private string base_url = $"http://{ApiUtility.MainIp}/api/check";

        public async UniTask<(MessageModel.Message, AppVersionModel.AppVersion, SucceededModel)> Check(string urlParameter)
        {
            var request = UnityWebRequest.Get(base_url + urlParameter);
            request.timeout = ApiUtility.TimeOut;
            request.SetRequestHeader("Content-Type", "application/json");
            var result = await request.SendWebRequest();
            var maintenance = ApiUtility.IsMaintenance(request.downloadHandler.text);
            var appVersion = JsonUtility.FromJson<AppVersionModel.AppVersion>(request.downloadHandler.text);
            var succeeded = JsonUtility.FromJson<SucceededModel>(request.downloadHandler.text);

            return (maintenance, appVersion, succeeded);
        }
    }
}
