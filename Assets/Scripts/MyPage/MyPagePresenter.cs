using Api;
using Api.Model;
using Common;
using Cysharp.Threading.Tasks;
using MyPage;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyPagePresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoUserCode;
    [SerializeField] private TextMeshProUGUI _infoUserName;
    [SerializeField] private TextMeshProUGUI _userName;
    [SerializeField] private Button _userUpdateButton;
    [SerializeField] private TMP_InputField _userNameText;
    [SerializeField] private InputValidator _inputValidator;
    [SerializeField] private TextMeshProUGUI _errorText;
    public string _errorExcludeNumberMessage = "含めることのできない文字が存在いいたします";

    void Start()
    {
        _userName.text = "ユーザー名：" + ZPlayerPrefs.GetString("userName");
        _infoUserName.text = "ユーザー名：" + ZPlayerPrefs.GetString("userName");
        _infoUserCode.text = "ID：" + Boot.Instance.UserCode;

        // 更新ボタン押下イベント登録
        _userUpdateButton.onClick.AddListener(
            async () =>
            {
                if (_inputValidator.Validate(_userNameText.text))
                {
                    _errorText.gameObject.SetActive(false);

                    LoadingManager.Instance.SetActive(true);
                    SucceededModel succeededModel = null;
                    MessageModel.Message maintenance = null;
                    try
                    {
                        (succeededModel, maintenance) = await UserRequest.Instance.Change(Boot.Instance.UserId, _userNameText.text, ZPlayerPrefs.GetString("authenticationCode"));
                    }
                    catch (UnityWebRequestException exception)
                    {
                        LoadingManager.Instance.SetActive(false);
                        NotificationPresenter.Instance.OpenNotification(exception.Error);
                        return;
                    }

                    if (maintenance.title != null)
                    {
                        LoadingManager.Instance.SetActive(false);
                        PopPresenter.Instance.OpenMaintenancePop(maintenance.title, maintenance.message);
                        return;
                    }

                    if (succeededModel.is_succeeded == false)
                    {
                        LoadingManager.Instance.SetActive(false);
                        _errorText.gameObject.SetActive(true);
                        _errorText.text = _errorExcludeNumberMessage;
                        return;
                    }

                    ZPlayerPrefs.SetString("userName", _userNameText.text);
                    ZPlayerPrefs.Save();
                    Boot.Instance.UserName = _userNameText.text;
                    _userName.text = "ユーザー名：" + _userNameText.text;
                    _infoUserName.text = "ユーザー名：" + _userNameText.text;

                    LoadingManager.Instance.SetActive(false);
                }
            }
        );
    }
}
