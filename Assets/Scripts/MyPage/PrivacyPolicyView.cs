using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MyPage
{
    public class PrivacyPolicyView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        public const string URL = "https://nn-nito.github.io/AnimeRadioChat/";

        void Start()
        {
            _button.onClick.AddListener( () => Open());
        }

        private void Open()
        {
            Application.OpenURL(URL);
        }
    }

}