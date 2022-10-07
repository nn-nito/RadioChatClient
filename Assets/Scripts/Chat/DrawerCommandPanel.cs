using Common;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using frame8.Logic.Misc.Visual.UI;
using frame8.Logic.Misc.Other.Extensions;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.Demos.Common.CommandPanels;
using Com.TheFallenGames.OSA.Demos.Common.Drawer;
using UniRx;
using Cysharp.Threading.Tasks;
using Api;
using Api.Model;

namespace Chat
{
	/// <summary>
	/// Versatile context menu drawer that is programmatically configured for each demo scene to contain specific UI controls
	/// </summary>
	public class DrawerCommandPanel : MonoBehaviour
	{
		public event Action<int> ItemCountChangeRequested;
		public event Action<ChatModel.Chat, int> AddItemRequested;
        public event Action<ChatModel.Chats, int> AddItemsRequested;

		public int AdaptersCount { get { return _Adapters == null ? 0 : _Adapters.Length; } }

		ScrollRect _ScrollRect;
		IOSA[] _Adapters;
		float _LastScreenWidth, _LastScreenHeight;
		string _lastTimeSent = "";
		[SerializeField] private GameObject _explanation;

		async void Awake()
		{
			var p = transform;
			while (!_ScrollRect && (p = p.parent))
				_ScrollRect = p.GetComponent<ScrollRect>();

			LoadingManager.Instance.SetActive(true);
			// ルーム内の全チャット取得
			ChatModel.Chats chats = null;
			MessageModel.Message maintenance = null;
			try
            {
				(chats, maintenance) = await ChatRequest.Instance.GetAllChat($"/?user_id={Boot.Instance.UserId}&room_id={Boot.Instance.RoomId}");
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

			if (chats.chats.Length > 0)
			{
				AddItemWithChecks(chats, true);
				var lastChat = chats.chats.Last();
				_lastTimeSent = lastChat.time_sent;
			}
			else
            {
				_explanation.SetActive(true);
			}

			LoadingManager.Instance.SetActive(false);

			TestAsync(this.GetCancellationTokenOnDestroy()).Forget();
		}

		void Start()
		{
            //TestAsync(this.GetCancellationTokenOnDestroy()).Forget();
		}

		void Update()
		{
			if (Screen.width != _LastScreenWidth && Screen.height != _LastScreenHeight)
			{
				_LastScreenWidth = Screen.width;
				_LastScreenHeight = Screen.height;
			}
		}

         private async UniTaskVoid TestAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
				ChatModel.Chats chats = null;
				MessageModel.Message maintenance = null;
				try
                {
					// ここで自身が送信したチャットは受け取らない。受け取ると重複する
					var timeSent = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", DateTime.Now);
					(chats, maintenance) = await ChatRequest.Instance.GetChat($"/?user_id={Boot.Instance.UserId}&room_id={Boot.Instance.RoomId}&before_time_sent={_lastTimeSent}&time_sent={timeSent}");

					if (maintenance != null && maintenance.title != null)
					{
						LoadingManager.Instance.SetActive(false);
						PopPresenter.Instance.OpenMaintenancePop(maintenance.title, maintenance.message);
						return;
					}
					if (chats.chats.Length > 0)
                    {
						_explanation.SetActive(false);
						_lastTimeSent = timeSent;
					}
				}
				catch (UnityWebRequestException exception)
				{
					NotificationPresenter.Instance.OpenNotification(exception.Error);
					LoadingManager.Instance.SetActive(false);
				}

				if (chats != null && chats.chats.Length > 0)
                {
                   	AddItemWithChecks(chats, true);
                }
                // 2秒待って繰り返す
                await UniTask.Delay(2000, cancellationToken: token);
            }
        }

		public void Init(
			IOSA adapter,
			bool addGravityCommand = true,
			bool addItemEdgeFreezeCommand = true,
			bool addContentEdgeFreezeCommand = true,
			bool addServerDelaySetting = true,
			bool addOneItemAddRemovePanel = true,
			bool addInsertRemoveAtIndexPanel = true
		) { Init(new IOSA[] { adapter }, addGravityCommand, addItemEdgeFreezeCommand, addContentEdgeFreezeCommand, addServerDelaySetting, addOneItemAddRemovePanel, addInsertRemoveAtIndexPanel); }

		public void Init(
			IOSA[] adapters, 
			bool addGravityCommand = true, 
			bool addItemEdgeFreezeCommand = true, 
			bool addContentEdgeFreezeCommand = true,
			bool addServerDelaySetting = true,
			bool addOneItemAddRemovePanel = true,
			bool addInsertRemoveAtIndexPanel = true
		) {
			_Adapters = adapters;
		}

		void AddItemWithChecks(ChatModel.Chats chats, bool atTail)
		{
			var index = _Adapters[0].GetItemsCount();

			if (AddItemRequested != null)
			{
				int c;
				if (index >= 0 && index <= (c=_Adapters[0].GetItemsCount()) && c < OSAConst.MAX_ITEMS)
					AddItemsRequested(chats, index);
			}
		}

        public void AddItem(ChatModel.Chat chat)
        {
            AddItemRequested(chat, GetItemCount());
        }

        private int GetItemCount()
        {
            return _Adapters[0].GetItemsCount();
        }
	}
}
