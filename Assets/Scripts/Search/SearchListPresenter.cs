using Api;
using Api.Model;
using Common;
using Cysharp.Threading.Tasks;
using Favorite;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Search
{
    public class SearchListPresenter : MonoBehaviour
    {
        [SerializeField] private RectTransform _radio;
        [SerializeField] private RectTransform _radioStationName;
        [SerializeField] private GameObject _parentView;
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private Button _searchButton;
        [SerializeField] private RectTransform _scrollViewRectTransform;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _resultTextObj;
        private List<string> _dayOfWeek = new List<string> { "日", "月", "火", "水", "木", "金", "土" };
        private RadioStationModel.RadioStations _radioStations;
        private MessageModel.Message _maintenance;

        async void Start()
        {
            LoadingManager.Instance.SetActive(true);

            var title = "";

            // 検索ボタン
            _searchButton.onClick.AddListener(
                async () =>
                {
                    title = "";
                    if (_searchInputField.text != "")
                    {
                        title = _searchInputField.text;
                    }
                    LoadingManager.Instance.SetActive(true);

                    foreach (Transform child in _parentView.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }

                    try
                    {
                        (_radioStations, _maintenance) = await RadioStationRequest.Instance.GetRadioStations();
                    }
                    catch (UnityWebRequestException exception)
                    {
                        NotificationPresenter.Instance.OpenNotification(exception.Error);
                        LoadingManager.Instance.SetActive(false);
                        return;
                    }

                    if (_maintenance.title != null)
                    {
                        LoadingManager.Instance.SetActive(false);
                        PopPresenter.Instance.OpenMaintenancePop(_maintenance.title, _maintenance.message);
                        return;
                    }

                    RadioWithUserFavoriteModel onAir = null;
                    try
                    {
                        (onAir, _maintenance) = await SearchRequest.Instance.SearchRadios($"/?user_id={Boot.Instance.UserId}&title={title}");
                    }
                    catch (UnityWebRequestException exception)
                    {
                        NotificationPresenter.Instance.OpenNotification(exception.Error);
                        LoadingManager.Instance.SetActive(false);
                        return;
                    }

                    if (_maintenance.title != null)
                    {
                        LoadingManager.Instance.SetActive(false);
                        PopPresenter.Instance.OpenMaintenancePop(_maintenance.title, _maintenance.message);
                        return;
                    }
                    CreateRadio(onAir);

                    LoadingManager.Instance.SetActive(false);
                }
            );

            // ラジオ局すべて取得
            try
            {
                (_radioStations, _maintenance) = await RadioStationRequest.Instance.GetRadioStations();
            }
            catch (UnityWebRequestException exception)
            {
                NotificationPresenter.Instance.OpenNotification(exception.Error);
                LoadingManager.Instance.SetActive(false);
                return;
            }

            if (_maintenance.title != null)
            {
                LoadingManager.Instance.SetActive(false);
                PopPresenter.Instance.OpenMaintenancePop(_maintenance.title, _maintenance.message);
                return;
            }

            title = "";
            // タイトルで検索しヒットしたラジオすべて取得
            if (SearchRequest.Instance == null)
            {
                return;
            }

            RadioWithUserFavoriteModel searchList = null;
            try
            {
                (searchList, _maintenance) = await SearchRequest.Instance.SearchRadios($"/?user_id={Boot.Instance.UserId}&title={title}");
            }
            catch (UnityWebRequestException exception)
            {
                NotificationPresenter.Instance.OpenNotification(exception.Error);
                LoadingManager.Instance.SetActive(false);
                return;
            }

            if (_maintenance.title != null)
            {
                LoadingManager.Instance.SetActive(false);
                PopPresenter.Instance.OpenMaintenancePop(_maintenance.title, _maintenance.message);
                return;
            }

            if (_radioStationName == null)
            {
                //LoadingManager.Instance.SetActive(false);
                return;
            }

            CreateRadio(searchList);

            LoadingManager.Instance.SetActive(false);
        }

        private void CreateRadio(RadioWithUserFavoriteModel searchListModel)
        {
            if (searchListModel.radios.Length == 0)
            {
                var result_obj = GameObject.Instantiate(_resultTextObj.transform) as RectTransform;
                result_obj.SetParent(_parentView.transform, false);
                result_obj.GetComponent<TMP_Text>().text = "検索結果がありませんでした";

                return;
            }

            var radioStationId = 0;
            foreach (var radio in searchListModel.radios)
            {
                if (radio.radio_station_id != radioStationId)
                {
                    radioStationId = radio.radio_station_id;
                    foreach (var radioStation in _radioStations.radio_stations)
                    {
                        if (radioStation.id == radioStationId)
                        {
                            // ラジオ局ごとにラジオ局名を頭に表示する
                            var station_obj = GameObject.Instantiate(_radioStationName) as RectTransform;
                            station_obj.SetParent(_parentView.transform, false);
                            station_obj.GetComponent<TextMeshProUGUI>().text = radioStation.name;
                        }
                    }
                }

                // リストに表示するラジオを設定
                var obj = GameObject.Instantiate(_radio) as RectTransform;
                obj.SetParent(_parentView.transform, false);
                // Title設定
                var header = obj.transform.Find("Radio/Horizon/Vertival/Header").gameObject.GetComponent<TextMeshProUGUI>();
                var title = radio.title;
                if (radio.is_main_air == false)
                {
                    // 再放送
                    title = "(再)" + title;
                }
                header.text = title;
                // 曜日と時間設定
                var time = obj.transform.Find("Radio/Horizon/Vertival/Time").gameObject.GetComponent<TextMeshProUGUI>();
                time.text = _dayOfWeek[radio.day_of_week] + "曜" + " " + radio.on_air_start_time.Remove(4, 3) + "～" + radio.on_air_end_time.Remove(4, 3);
                // 出演者設定
                var peformer = obj.transform.Find("Radio/Horizon/Vertival/Performer").gameObject.GetComponent<TextMeshProUGUI>();
                peformer.text = "出演：" + radio.performer;
                // ルームIDとラジオタイトル設定
                obj.transform.Find("Radio").GetComponent<SearchView>().RoomId = radio.room_id;
                obj.transform.Find("Radio").GetComponent<SearchView>().RadioTitle = radio.title;

                // お気に入り設定
                var favoriteView = obj.transform.Find("Radio/Horizon/Horizon/Favorite").gameObject.GetComponent<FavoriteView>();
                favoriteView.RoomId = radio.id;
                foreach (var userFavoriteRadio in searchListModel.user_favorite_radios)
                {
                    if (radio.id == userFavoriteRadio.radio_id)
                    {
                        // 既にお気に入り登録していた場合クリック済みに変更
                        favoriteView.ClickEvent();
                    }
                }
            }
        }
    }
}
