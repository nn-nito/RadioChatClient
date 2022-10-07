using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

namespace Home
{
    public class FavoriteRadioView : MonoBehaviour
    {
        [SerializeField] private Button button;
        public int RoomId { get; set; }
        public string RadioTitle { get; set; }


        private void Awake()
        {
            button.OnClickAsObservable()
                .Subscribe(_ => LoadScene())
                .AddTo(gameObject);
        }

        private void LoadScene()
        {
            Boot.Instance.BeforeScene = SceneManager.GetActiveScene().name;
            Boot.Instance.CurrentScene = SceneList.CHAT;

            Boot.Instance.RoomId = RoomId;
            Boot.Instance.RadioTitle = RadioTitle;
            SceneManager.LoadScene(SceneList.CHAT);
        }
    }
}
