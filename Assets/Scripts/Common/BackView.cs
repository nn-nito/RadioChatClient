using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

namespace Common
{
    public class BackView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.OnClickAsObservable()
                .Subscribe(_ => LoadScene())
                .AddTo(gameObject);
        }

        private void LoadScene()
        {
            Boot.Instance.CurrentScene = Boot.Instance.BeforeScene;

            SceneManager.LoadScene(Boot.Instance.BeforeScene);
        }
    }
}