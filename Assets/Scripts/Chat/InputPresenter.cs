using Api;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using Api.Model;

namespace Chat
{
    public class InputPresenter : MonoBehaviour
    {
        private DrawerCommandPanel drawer;
        [SerializeField] private GameObject _explanation;

        void Start()
        {
            drawer = GameObject.Find("DrawerCommandPanelTest").GetComponent<DrawerCommandPanel>();

            ChatProvider.Instance.chatButton.onClick.AddListener(
               async () =>
               {
                   if (ChatProvider.Instance.chatInputField.text != "")
                   {
                       ChatModel.Chat chat = new ChatModel.Chat
                       {
                           user_id = Boot.Instance.UserId,
                           user_name = Boot.Instance.UserName,
                           room_id = Boot.Instance.RoomId,
                           message = ChatProvider.Instance.chatInputField.text,
                           time_sent = DateTime.Now.ToString()
                       };

                       ChatProvider.Instance.chatButton.interactable = false;
                       try
                       {
                           var (chats, maintenance) = await ChatRequest.Instance.SendChat(Boot.Instance.UserId, Boot.Instance.UserName, Boot.Instance.RoomId, ChatProvider.Instance.chatInputField.text, System.DateTime.Now);
                           if (maintenance.title != null)
                           {
                               LoadingManager.Instance.SetActive(false);
                               PopPresenter.Instance.OpenMaintenancePop(maintenance.title, maintenance.message);
                               return;
                           }

                           // チャット一覧に表示
                           drawer.AddItem(chat);
                       }
                       catch (UnityWebRequestException exception)
                       {

                           NotificationPresenter.Instance.OpenNotification(exception.Error);
                       }
                       ChatProvider.Instance.chatInputField.text = "";
                       ChatProvider.Instance.chatButton.interactable = true;
                       _explanation.SetActive(false);
                   }
               }
           );
        }
    }
}
