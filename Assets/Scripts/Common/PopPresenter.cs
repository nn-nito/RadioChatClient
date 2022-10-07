using Cysharp.Threading.Tasks;
using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Api
{
    public class PopPresenter : SingletonMonoBehaviour<PopPresenter>
    {
        [SerializeField] private ModalWindowManager _modalWindow;
        public void OpenMaintenancePop(string title, string description)
        {
            _modalWindow.titleText = title;
            _modalWindow.descriptionText = description;
            _modalWindow.UpdateUI();
            _modalWindow.OpenWindow();
            Observable.Timer(TimeSpan.FromSeconds(10)).Subscribe(_ => SceneManager.LoadScene(SceneList.TITLE))
                .AddTo(this);
        }
    }
}
