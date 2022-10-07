using Api;
using Api.Model;
using Common;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Schema;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace StartUp
{
    public class StartUpManager : MonoBehaviour
    {
        [SerializeField] private ModalWindowManager _modalWindow;
        [SerializeField] private TextMeshProUGUI _modalTitle;
        [SerializeField] private TextMeshProUGUI _modalContent;
        [SerializeField] private Button _tapButton;
        [SerializeField] private TextMeshProUGUI _confirmButtonText;
        [SerializeField] private TextMeshProUGUI _confirmButtonText2;
        [SerializeField] private GameObject _attentionObj;

        // Start is called before the first frame update
        async void Start()
        {
            if (ZPlayerPrefs.GetInt("userId", 0) == 0)
            {
                // 初起動時のみ広告が流れないので広告宣告を非アクティブ
                _attentionObj.SetActive(false);
            }
            var beforeStartingCount = ZPlayerPrefs.GetInt("startingCount", 0);
            // 起動回数をカウント
            ZPlayerPrefs.SetInt("startingCount", beforeStartingCount += 1);
            ZPlayerPrefs.Save();

            // サーバーと通信し現在最新のアプリバージョンかどうか確認
            // メンテかどうか確認
            var applicationVersion = (string)Application.version;
            var applicationPlatformId = (int)Application.platform;

#if UNITY_EDITOR
            //PlayerPrefs.DeleteAll();
            //ZPlayerPrefs.DeleteAll();
            // applicationVersion = "1.0.0";
            applicationPlatformId = (int)RuntimePlatform.Android;
#endif
            var token = this.GetCancellationTokenOnDestroy();
            UniTask.Void(async () =>
            {
                var handler = _tapButton.GetAsyncClickEventHandler(token);
                while (true)
                {
                    await handler.OnClickAsync();
                    OpenModal(applicationVersion, applicationPlatformId);
                }
            });
        }

        async void OpenModal(string applicationVersion, int applicationPlatformId)
        {
            LoadingManager.Instance.SetActive(true);

            MessageModel.Message maintenance = null;
            AppVersionModel.AppVersion appVersion = null;
            SucceededModel succeeded = null;
            try
            {
                (maintenance, appVersion, succeeded) = await ServerCheckRequest.Instance.Check($"/?version={applicationVersion}&platform_id={applicationPlatformId}");
                LoadingManager.Instance.SetActive(false);
            }
            catch (UnityWebRequestException exception)
            {
                NotificationPresenter.Instance.OpenNotification(exception.Error, 1);
                LoadingManager.Instance.SetActive(false);
                return;
            }

            if (maintenance.title != null)
            {
                _confirmButtonText.text = "了解";
                _confirmButtonText2.text = "了解";
                // メンテ中
                _modalWindow.OpenWindow();
                _modalTitle.text = maintenance.title;
                _modalContent.text = maintenance.message;
            }

            if (appVersion.version != null)
            {
                _confirmButtonText.text = "アプリ更新";
                _confirmButtonText2.text = "アプリ更新";
                // アプリが最新ではない
                _modalWindow.OpenWindow();
                _modalTitle.text = appVersion.title;
                _modalContent.text = appVersion.message;
                _modalWindow.confirmButton.OnClickAsObservable()
                    .Subscribe(_ => OpenUrl(applicationPlatformId, appVersion.url))
                    .AddTo(gameObject);
            }

            if (succeeded.is_succeeded == true)
            {
                Boot.Instance.CurrentScene = SceneList.HOME;
                Boot.Instance.BeforeScene = SceneList.HOME;

                if (ZPlayerPrefs.GetInt("userId", 0) == 0)
                {
                    // 新規
                    ZPlayerPrefs.SetString("beforeStartingDay", DateTime.Now.ToString("yyyy/MM/dd"));
                    ZPlayerPrefs.Save();
                    SceneManager.LoadScene(SceneList.USEREDIT);
                }
                else
                {
                    // ユーザー作成済み
                    Boot.Instance.UserId = ZPlayerPrefs.GetInt("userId");
                    Boot.Instance.UserName = ZPlayerPrefs.GetString("userName");
                    Boot.Instance.UserCode = ZPlayerPrefs.GetString("userCode");
                    ZPlayerPrefs.SetString("beforeStartingDay", DateTime.Now.ToString("yyyy/MM/dd"));
                    ZPlayerPrefs.Save();

                    SceneManager.LoadScene(SceneList.ADS);
                    //var beforeStartingDay = DateTime.Parse(ZPlayerPrefs.GetString("beforeStartingDay", "2010/05/05"));
                    //TimeSpan subTime = DateTime.Now - beforeStartingDay;
                    //if (subTime.Days > 0)
                    //{
                    //    ZPlayerPrefs.SetString("beforeStartingDay", DateTime.Now.ToString("yyyy/MM/dd"));
                    //    // 広告は一日に一回なので既に流れていなかったら広告へ
                    //    SceneManager.LoadScene(SceneList.ADS);
                    //}
                    //else
                    //{
                    //    SceneManager.LoadScene(SceneList.HOME);
                    //}
                }
            }
        }

        private void OpenUrl(int applicationPlatformId, string url)
        {
            if (applicationPlatformId == (int)RuntimePlatform.IPhonePlayer)
            {
                // IOSの場合
                Application.OpenURL(url);
            }
            if (applicationPlatformId == (int)RuntimePlatform.Android)
            {
                // Androidの場合
                Application.OpenURL(url);
            }
        }
    }
}
