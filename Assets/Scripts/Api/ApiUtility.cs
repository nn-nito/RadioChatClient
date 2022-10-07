using Api.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Api
{
    public class ApiUtility
    {
        public const string DevIp = "*****";
        public const string MainIp = "*****";
        public const int TimeOut = 60;
        public const int Succeeded = 0;
        public const int HttpError = 1;
        public const int NetWorkAnotherError = 2;
        public const int TimeOutError = 3;

        public static int IsError(UnityWebRequest request)
        {
            if (request.isHttpError)
            {
                // レスポンスコードを見て処理
                return HttpError;
            }
            
            if (request.isNetworkError)
            {
                // エラーメッセージを見て処理
                if (request.error == "Request timeout")
                {
                    // タイムアウト時の処理
                    return TimeOutError;
                }

                return NetWorkAnotherError;
            }

            // 成功
            return Succeeded;
        }

        public static MessageModel.Message IsMaintenance(string response)
        {
            return JsonUtility.FromJson<MessageModel.Message>(response);
        }
    }
}
