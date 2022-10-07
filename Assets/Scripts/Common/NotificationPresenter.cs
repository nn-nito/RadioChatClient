using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Common
{
    public class NotificationPresenter : SingletonMonoBehaviour<NotificationPresenter>
    {
        [SerializeField] private NotificationManager _notificationManager;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        private const string _defaultTitle = "タイムアウトが発生しました";
        private const string _serverErrorTitle = "サーバーエラーが発生しました";
        private const string _defaultDescription = "ネットワーク接続を確認してください<br>下タブのアイコンを押下するとリロードできます";
        private const string _titleDescription = "ネットワーク接続を確認してください";
        private const string _serverErrorDescription = "再起動をお試しください";

        override protected void Awake()
        {
            base.Awake();
        }

        public void OpenNotification(string errorMessage, int specialId = 0)
        {
            if (errorMessage.Equals("Cannot connect to destination host"))
            {
                _title.text = _defaultTitle;
                _description.text = _defaultDescription;
                if (specialId == 1)
                {
                    _description.text = _titleDescription;
                }
                _notificationManager.OpenNotification();
            }

            if (errorMessage.Equals("Request timeout"))
            {
                _title.text = _defaultTitle;
                _description.text = _defaultDescription;
                if (specialId == 1)
                {
                    _description.text = _titleDescription;
                }
                _notificationManager.OpenNotification();
            }

            if (errorMessage.Equals("Cannot resolve destination host"))
            {
                _title.text = _serverErrorTitle;
                _description.text = _defaultDescription;
                if (specialId == 1)
                {
                    _description.text = _serverErrorDescription;
                }
                _notificationManager.OpenNotification();
            }
        }
    }
}
