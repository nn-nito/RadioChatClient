using Common;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TopTYab
{
    public class HelpView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        void Start()
        {
            _button.OnClickAsObservable().Subscribe(_ => LoadScene());
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(SceneList.TUTORIAL);
        }
    }
}
