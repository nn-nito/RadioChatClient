using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Api;
using System;
using Cysharp.Threading.Tasks;
using Common;
using UnityEngine.SceneManagement;
using Api.Model;

public class UserEditPresenter : MonoBehaviour
{
    [SerializeField] private Button _createButton;

    void Start()
    {
        _createButton.onClick.AddListener(
            async () =>
            {
                LoadingManager.Instance.SetActive(true);
                var currentTime = DateTime.Now.ToString();

                UserModel.User user = null;
                MessageModel.Message maintenance = null;
                try
                {
                    (user, maintenance) = await UserRequest.Instance.SignUp(currentTime);
                }
                catch (UnityWebRequestException exception)
                {
                    NotificationPresenter.Instance.OpenNotification(exception.Error, 1);
                    LoadingManager.Instance.SetActive(false);
                    return;
                }

                if (maintenance.title != null)
                {
                    LoadingManager.Instance.SetActive(false);
                    PopPresenter.Instance.OpenMaintenancePop(maintenance.title, maintenance.message);
                    return;
                }

                // ユーザー設定
                ZPlayerPrefs.SetInt("userId", user.id);
                ZPlayerPrefs.SetString("userName", user.name);
                ZPlayerPrefs.SetString("userCode", user.user_code);
                ZPlayerPrefs.SetString("registeredTime", user.registered_time);
                ZPlayerPrefs.SetString("authenticationCode", user.authentication_code);
                ZPlayerPrefs.Save();
                Boot.Instance.UserId = user.id;
                Boot.Instance.UserName = user.name;
                Boot.Instance.UserCode = user.user_code;

                LoadingManager.Instance.SetActive(false);
                //SceneManager.LoadScene(SceneList.TUTORIAL);
                SceneManager.LoadScene(SceneList.HOME);
            }
        );
    }
}
