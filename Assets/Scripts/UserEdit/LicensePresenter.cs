using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UserEdit
{
    public class LicensePresenter : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ModalWindowManager _modalWindowManager;

        void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => _modalWindowManager.OpenWindow());
        }
    }
}
