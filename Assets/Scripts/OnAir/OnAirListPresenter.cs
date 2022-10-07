using Api;
using Api.Model;
using Common;
using Cysharp.Threading.Tasks;
using Favorite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

namespace OnAir
{
    public class OnAirListPresenter : MonoBehaviour
    {
        [SerializeField] private RectTransform onAir;
        [SerializeField] private GameObject parentView;
        [SerializeField] private GameObject _resultTextObj;
        private Dictionary<int, string> _dayOfWeek = new Dictionary<int, string>
        {
            {0, "日"}, {1, "月"}, {2, "火"}, {3, "水"}, {4, "木"}, {5, "金"}, {6, "土"},
        };

        async void Start()
        {
            LoadingManager.Instance.SetActive(true);

            var dateTime = DateTime.Now;
            var currentTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dateTime);
            var dayOfWeek = (int)dateTime.DayOfWeek;

            RadioWithUserFavoriteModel onAirList = null;
            MessageModel.Message maintenance = null;
            try
            {
                (onAirList, maintenance) = await OnAirRequest.Instance.GetOnAirRadio($"/?user_id={Boot.Instance.UserId}&day_of_week={dayOfWeek}&current_time={currentTime}");
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

            if (onAir == null)
            {
                //LoadingManager.Instance.SetActive(false);
                return;
            }

            // ラジオIDをKeyとした不規則ラジオの配列作成
            Dictionary<int, IrregularRadioModel.IrregularRadio> irregularRadios = new Dictionary<int, IrregularRadioModel.IrregularRadio>();
            foreach (var irregularRadio in onAirList.irregular_radios)
            {
                irregularRadios.Add(irregularRadio.radio_id, irregularRadio);
            }

            var irregularCount = 0;
            foreach (var radio in onAirList.radios)
            {
                if (radio.is_irregular == true && irregularRadios.ContainsKey(radio.id) == false)
                {
                    irregularCount += 1;
                    // 不規則ラジオ でかつ そのラジオがスケジュールに入っていない場合現在放送していないのでスキップ
                    continue;
                }

                var obj = GameObject.Instantiate(onAir) as RectTransform;
                obj.SetParent(parentView.transform, false);
                // Title設定
                var header = obj.transform.Find("Radio/Header").gameObject.GetComponent<TextMeshProUGUI>();
                header.text = radio.title;
                // Time設定
                var time = obj.transform.Find("Radio/Time").gameObject.GetComponent<TextMeshProUGUI>();
                time.text = dateTime.ToString("yyyy年M月d日") + $"({_dayOfWeek[radio.day_of_week]})" + " " + radio.on_air_start_time.Remove(4, 3) + "~" + radio.on_air_end_time.Remove(4, 3);
                // Body設定
                var body = obj.transform.Find("Radio/Body").gameObject.GetComponent<TextMeshProUGUI>();
                body.text = radio.body;
                // 出演者設定
                var peformer = obj.transform.Find("Radio/Performer").gameObject.GetComponent<TextMeshProUGUI>();
                peformer.text = "出演：" + radio.performer;
                // ルームIDとラジオタイトル設定
                obj.transform.Find("Radio").GetComponent<OnAirView>().RoomId = radio.room_id;
                obj.transform.Find("Radio").GetComponent<OnAirView>().RadioTitle = radio.title;

                // お気に入り設定
                var favorite = obj.transform.Find("Radio/BottomGroup/Favorite").gameObject.GetComponent<FavoriteView>();
                favorite.RoomId = radio.id;
                foreach (var userFavoriteRadio in onAirList.user_favorite_radios)
                {
                    if (radio.id == userFavoriteRadio.radio_id)
                    {
                        // 既にお気に入り登録していた場合クリック済みに変更
                        favorite.ClickEvent();
                    }
                }
            }

            if (onAirList.radios.Length == 0 || irregularCount == onAirList.radios.Length)
            {
                // 放送中のラジオが無い or 放送中のラジオとして取得はしてきたが不定期配信で今回は放送しないもの
                var result_obj = GameObject.Instantiate(_resultTextObj.transform) as RectTransform;
                result_obj.SetParent(parentView.transform, false);
                result_obj.GetComponent<TMP_Text>().text = "放送中の番組がありません";
            }

            LoadingManager.Instance.SetActive(false);
        }
    }
}
