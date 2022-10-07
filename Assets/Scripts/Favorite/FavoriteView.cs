using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using Api;
using Michsky.UI.ModernUIPack;
using Cysharp.Threading.Tasks;
using Api.Model;

namespace Favorite
{
    public class FavoriteView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AnimatedIconHandler _animatedIconHandler;
        public int RoomId { get; set; }

        void Start()
        {
            _button.onClick.AddListener(
                async () =>
                {
                    UserFavoriteRadioModel.UserFavoriteRadio favorite = null;
                    MessageModel.Message maintenance = null;
                    if (_animatedIconHandler.IsClicked)
                    {
                        try
                        {
                            (favorite, maintenance) = await UserFavoriteRadioRequest.Instance.AddFavorite(Boot.Instance.UserId, RoomId);
                        }
                        catch (UnityWebRequestException exception)
                        {
                            NotificationPresenter.Instance.OpenNotification(exception.Error);
                            LoadingManager.Instance.SetActive(false);
                            return;
                        }

                        if (maintenance.title != null)
                        {
                            LoadingManager.Instance.SetActive(false);
                            PopPresenter.Instance.OpenMaintenancePop(maintenance.title, maintenance.message);
                            return;
                        }
                    }
                    else
                    {
                        bool isSucceeded = false;
                        try
                        {
                            (isSucceeded, maintenance) = await UserFavoriteRadioRequest.Instance.DeleteFavorite(Boot.Instance.UserId, RoomId);
                        }
                        catch (UnityWebRequestException exception)
                        {
                            NotificationPresenter.Instance.OpenNotification(exception.Error);
                            LoadingManager.Instance.SetActive(false);
                            return;
                        }

                        if (maintenance.title != null)
                        {
                            LoadingManager.Instance.SetActive(false);
                            PopPresenter.Instance.OpenMaintenancePop(maintenance.title, maintenance.message);
                            return;
                        }
                    }
                }
            );
        }

        public void ClickEvent()
        {
            _animatedIconHandler.ClickEvent();
        }
    }
}