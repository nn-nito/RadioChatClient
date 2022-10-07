using Common;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BottomTabPresenter : MonoBehaviour
{
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _onAirButton;
    [SerializeField] private Button _searchButton;
    [SerializeField] private Button _myPageButton;

    // Start is called before the first frame update
    void Awake()
    {
        if (Boot.Instance.CurrentScene == SceneList.HOME)
        {
            _homeButton.gameObject.GetComponent<Image>().color = new Color(73 / 255, 95 / 255, 255 / 255);
            _onAirButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _searchButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _myPageButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        if (Boot.Instance.CurrentScene == SceneList.ONAIR)
        {
            _homeButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _onAirButton.gameObject.GetComponent<Image>().color = new Color(73 / 255, 95 / 255, 255 / 255);
            _searchButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _myPageButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        if (Boot.Instance.CurrentScene == SceneList.SEARCH)
        {
            _homeButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _onAirButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _searchButton.gameObject.GetComponent<Image>().color = new Color(73 / 255, 95 / 255, 255 / 255);
            _myPageButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        if (Boot.Instance.CurrentScene == SceneList.MYPAGE)
        {
            _homeButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _onAirButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _searchButton.gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
            _myPageButton.gameObject.GetComponent<Image>().color = new Color(73 / 255, 95 / 255, 255 / 255);
        }

        _homeButton.OnClickAsObservable()
            //.Subscribe(x => LoadScene(SceneList.HOME))
            .Subscribe(x => StartCoroutine(LoadScene(SceneList.HOME)))
            .AddTo(gameObject);
        _onAirButton.OnClickAsObservable()
            //.Subscribe(x => LoadScene(SceneList.ONAIR))
            .Subscribe(x => StartCoroutine(LoadScene(SceneList.ONAIR)))
            .AddTo(gameObject);
        _searchButton.OnClickAsObservable()
            //.Subscribe(x => LoadScene(SceneList.SEARCH))
            .Subscribe(x => StartCoroutine(LoadScene(SceneList.SEARCH)))
            .AddTo(gameObject);
        _myPageButton.OnClickAsObservable()
            //.Subscribe(x => LoadScene(SceneList.MYPAGE))
            .Subscribe(x => StartCoroutine(LoadScene(SceneList.MYPAGE)))
            .AddTo(gameObject);
    }

    //void LoadScene(string sceneName)
    //{
    //    Boot.Instance.BeforeScene = SceneManager.GetActiveScene().name;
    //    Boot.Instance.CurrentScene = sceneName;

    //    SceneManager.LoadScene(sceneName);
    //}

    IEnumerator LoadScene(string sceneName)
    {
        Boot.Instance.BeforeScene = SceneManager.GetActiveScene().name;
        Boot.Instance.CurrentScene = sceneName;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (true)
        {
            yield return null;
            // 読み込み完了したら
            if (asyncLoad.progress >= 0.9f)
            {
                // シーン読み込み
                asyncLoad.allowSceneActivation = true;
                break;
            }
        }
    }
}
