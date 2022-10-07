using Common;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tutorial
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => LoadScene());
        }

        private void LoadScene()
        {
            if (ZPlayerPrefs.GetInt("userId", 0) == 0)
            {
                SceneManager.LoadScene(SceneList.HOME);
            }

            SceneManager.LoadScene(Boot.Instance.CurrentScene);
        }
    }
}
