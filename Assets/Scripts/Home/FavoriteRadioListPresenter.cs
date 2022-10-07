using Api;
using Api.Model;
using Common;
using Cysharp.Threading.Tasks;
using Favorite;
using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Home
{
    public class FavoriteRadioListPresenter : MonoBehaviour
    {
        [SerializeField] private RectTransform _radioObj;
        [SerializeField] private RectTransform _dayOfWeekObj;
        [SerializeField] private RectTransform _timeObj;
        [SerializeField] private GameObject _parentView;
        [SerializeField] private GameObject _resultTextObj;
        private Dictionary<int, string> _dayOfWeek = new Dictionary<int, string> 
        {
            {0, "日"}, {1, "月"}, {2, "火"}, {3, "水"}, {4, "木"}, {5, "金"}, {6, "土"},
        };

        async void Start()
        {
            LoadingManager.Instance.SetActive(true);

            var currentTime = DateTime.Now;
            var firstDayOfWeek = currentTime.FirstDayOfWeek().ToString("yyyy-M-d 00:00:00");
            var lastDayOfWeek = currentTime.LastDayOfWeek().ToString("yyyy-M-d 23:59:59");

            // お気に入りのラジオすべて取得
            RadioWithUserFavoriteModel radios = null;
            MessageModel.Message maintenance = null;
            try
            {
                (radios, maintenance) = await UserFavoriteRadioRequest.Instance.GetUserFavoriteRadios($"/?user_id={Boot.Instance.UserId}&first_day_of_week={firstDayOfWeek}&last_day_of_week={lastDayOfWeek}");
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

            CreateRadio(radios);

            LoadingManager.Instance.SetActive(false);
        }

        private void CreateRadio(RadioWithUserFavoriteModel radios)
        {
            if (radios.radios.Length == 0)
            {
                var result_obj = GameObject.Instantiate(_resultTextObj.transform) as RectTransform;
                result_obj.SetParent(_parentView.transform, false);
                result_obj.GetComponent<TMP_Text>().text = "お気に入りに登録した番組の今週の予定がありません";

                return;
            }

            var currentTime = DateTime.Now;
            DateTime[] dateTimeDayOjWeek = new DateTime[7];
            dateTimeDayOjWeek[0] = currentTime.FirstDayOfWeek();
            dateTimeDayOjWeek[1] = dateTimeDayOjWeek[0].AddDays((int)DayOfWeek.Monday);
            dateTimeDayOjWeek[2] = dateTimeDayOjWeek[0].AddDays((int)DayOfWeek.Tuesday);
            dateTimeDayOjWeek[3] = dateTimeDayOjWeek[0].AddDays((int)DayOfWeek.Wednesday);
            dateTimeDayOjWeek[4] = dateTimeDayOjWeek[0].AddDays((int)DayOfWeek.Thursday);
            dateTimeDayOjWeek[5] = dateTimeDayOjWeek[0].AddDays((int)DayOfWeek.Friday);
            dateTimeDayOjWeek[6] = currentTime.LastDayOfWeek();

            // ラジオIDをKeyとした不規則ラジオの配列作成
            Dictionary<int, IrregularRadioModel.IrregularRadio> irregularRadios = new Dictionary<int, IrregularRadioModel.IrregularRadio>();
            foreach (var irregularRadio in radios.irregular_radios)
            {
                irregularRadios.Add(irregularRadio.radio_id, irregularRadio);
            }

            foreach (KeyValuePair<int, string> day in _dayOfWeek)
            {
                // リストに表示する曜日を設定
                //var dayOjWeekObj = GameObject.Instantiate(_dayOfWeekObj) as RectTransform;
                //dayOjWeekObj.SetParent(_parentView.transform, false);
                //// Body設定
                //var dayOfWeekBody = dayOjWeekObj.transform.Find("Body").gameObject.GetComponent<TextMeshProUGUI>();
                //dayOfWeekBody.text = day.Value + "曜日";

                // リストに表示する時刻とラジオ設定
                foreach (var radio in radios.radios)
                {
                    if (radio.day_of_week != day.Key)
                    {
                        continue;
                    }

                    if (radio.is_irregular == true && irregularRadios.ContainsKey(radio.id) == false)
                    {
                        // 不適測ラジオ でかつ そのラジオがスケジュールに入っていない場合今週は放送しないのでスキップ
                        continue;
                    }

                    // リストに表示する時刻を設定
                    //var timeObj = GameObject.Instantiate(_timeObj) as RectTransform;
                    //timeObj.SetParent(_parentView.transform, false);
                    //var timeBody = timeObj.transform.Find("Body").gameObject.GetComponent<TextMeshProUGUI>();
                    //timeBody.text = dateTimeDayOjWeek[day.Key].ToString("M/d") + " " + radio.on_air_start_time.Remove(4, 3) + "～" + radio.on_air_end_time.Remove(4, 3);

                    // リストに表示するラジオを設定
                    var radioObj = GameObject.Instantiate(_radioObj) as RectTransform;
                    radioObj.SetParent(_parentView.transform, false);

                    // Title設定
                    var header = radioObj.transform.Find("Radio/Header").gameObject.GetComponent<TextMeshProUGUI>();
                    header.text = radio.title;
                    // Time設定
                    var time = radioObj.transform.Find("Radio/Time").gameObject.GetComponent<TextMeshProUGUI>();
                    time.text = dateTimeDayOjWeek[day.Key].ToString("yyyy年M月d日") + $"({day.Value})" + " " + radio.on_air_start_time.Remove(4, 3) + "~" + radio.on_air_end_time.Remove(4, 3);
                    // Body設定
                    var body = radioObj.transform.Find("Radio/Body").gameObject.GetComponent<TextMeshProUGUI>();
                    body.text = radio.body;
                    // 出演者設定
                    var peformer = radioObj.transform.Find("Radio/Performer").gameObject.GetComponent<TextMeshProUGUI>();
                    peformer.text = "出演：" + radio.performer;
                    // ルームIDとラジオタイトル設定
                    radioObj.transform.Find("Radio").gameObject.GetComponent<FavoriteRadioView>().RoomId = radio.room_id;
                    radioObj.transform.Find("Radio").gameObject.GetComponent<FavoriteRadioView>().RadioTitle = radio.title;

                    // お気に入り設定
                    var favorite = radioObj.transform.Find("Radio/BottomGroup/Favorite").gameObject.GetComponent<FavoriteView>();
                    favorite.RoomId = radio.id;
                    favorite.ClickEvent();
                }
            }
        }
    }
}